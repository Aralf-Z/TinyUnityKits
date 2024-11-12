using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace ZToolKit
{
    public enum SaveLocation
    {
        /// <summary> persistentDataPath </summary>
        Persistent = 1,

        /// <summary> Assets </summary>
        Assets = 2,
        
        /// <summary> PlayerPrefs </summary>
        PlayerPrefs = 3,
    }

    public enum SaveType
    {
        /// <summary> Json </summary>
        Json = 1,
        
        // /// <summary> 加密 </summary>
        // Encrypt = 2,
    }

    /// <summary>
    /// 档案, 可以理解成一个账号数据,只记录信息,不记录存档
    /// </summary>
    public class Archive
    {
        /// <summary> 档案名称 </summary>
        public string name;
        /// <summary> 档案下的所有存档 </summary>
        public List<string> saves;
        
        public Archive(string name)
        {
            this.name = name;
            saves = new List<string>();
        }

        /// <summary>
        /// 添加存档
        /// </summary>
        /// <param name="saveName">存档名称</param>
        /// <returns>是否重名</returns>
        public bool TryAddSave(string saveName)
        {
            if (saves.Contains(saveName))
            {
                return true;
            }
            saves.Add(saveName);
            return false;
        }

        /// <summary>
        /// 删除存档
        /// </summary>
        /// <param name="saveName">存档名称</param>
        /// <returns>是否重名</returns>
        public void RemoveSave(string saveName)
        {
            saves.Remove(saveName);
        }

        /// <summary>
        /// 删除最旧存档
        /// </summary>
        public void RemoveTheOldest()
        {
            saves.RemoveAt(0);
        }

        /// <summary>
        /// 存档个数
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            return saves.Count;
        }
        
        /// <summary>
        /// 是否有存档
        /// </summary>
        /// <param name="saveName"></param>
        /// <returns></returns>
        public bool Contains(string saveName)
        {
            return saves.Contains(saveName);
        }
    }

    /// <summary>
    /// 存档相关的配置文件
    /// </summary>
    public class SaveConfig
    {
        public string curArchive;
        public string curSave;
        public Dictionary<string, Archive> archives;
    }

    public static class SaveTool
    {
        private static readonly IGameSave gameSave;
        
        private static readonly ISaveConvert saveConvert;

        static SaveTool()
        {
            gameSave = GameConfig.SaveLocation switch
            {
                SaveLocation.Assets => new GameSaveAssets(),
                SaveLocation.Persistent => new GameSavePersistent(),
                SaveLocation.PlayerPrefs => new GameSavePlayerPrefs(),
                _ => throw new ArgumentOutOfRangeException()
            };

            saveConvert = GameConfig.SaveType switch
            {
                SaveType.Json => new JsonSaveConvert(),
                _ => throw new ArgumentOutOfRangeException(),
            };
        }

        public static T ReadSave<T>(Archive archive, string saveName) where T : SaveBase, new()
        {
            return gameSave.LoadGame<T>(archive.name, saveName);
        }

        public static bool WriteSave<T>(T save) where T : SaveBase, new()
        {
            return gameSave.SaveGame(save);
        }

        public static bool DeleteSave(string archiveName, string saveName)
        {
            return gameSave.DeleteGame(archiveName, saveName);
        }

        public static SaveConfig ReadConfig()
        {
            return gameSave.LoadCfg();
        }
        
        public static void WriteArchive(SaveConfig cfg)
        {
            gameSave.WriteCfg(cfg);
        }

        #region Convert

        private interface ISaveConvert
        {
            string GetSaveFileName(string saveName);
            T SaveConvert<T>(string saveStr);
            string SerializeSave<T>(T save);
        }
    
        private class JsonSaveConvert:
            ISaveConvert
        {
            public string GetSaveFileName(string saveName)
            {
                return $"{saveName}.json";
            }

            public T SaveConvert<T>(string str)
            {
                return JsonConvert.DeserializeObject<T>(str);
            }

            public string SerializeSave<T>(T save)
            {
                JsonSerializerSettings jsonSerializerSettings = new();
                jsonSerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                jsonSerializerSettings.NullValueHandling = NullValueHandling.Ignore;

                return JsonConvert.SerializeObject(save, jsonSerializerSettings);
            }
        }

        #endregion

        #region GameSave
        
        private interface IGameSave 
        {
            TSave LoadGame<TSave>(string archiveName, string saveName) where TSave : SaveBase, new();
            bool SaveGame<TSave>(TSave save)where TSave : SaveBase, new();
            bool DeleteGame(string archiveName, string saveName);

            SaveConfig LoadCfg();
            void WriteCfg(SaveConfig archive);
        }

        private abstract class GameSave:
            IGameSave
        {
            public TSave LoadGame<TSave>(string archiveName, string saveName) where TSave : SaveBase, new()
            {
                try
                {
                    var saveStr = ReadSave(archiveName, saveName);
                    return saveConvert.SaveConvert<TSave>(saveStr) ?? new TSave(){archive = archiveName, name = saveName};
                }
                catch (Exception e)
                {
                    LogTool.ToolError("SaveTool", e.Message);
                }

                return new TSave(){archive = archiveName, name = saveName};
            }

            public bool SaveGame<TSave>(TSave save)where TSave : SaveBase, new()
            {
                try
                {
                    WriteSave(saveConvert.SerializeSave(save), save.archive, save.name);
                    LogTool.ToolInfo("SaveTool", "Game saved successfully!");
                    return true;
                }
                catch (Exception e)
                {
                    LogTool.ToolError("SaveTool", e.Message);
                }

                return false;
            }

            public SaveConfig LoadCfg()
            {
                var config = ReadArchiveConfig();
                
                if (string.IsNullOrEmpty(config))
                {
                    var archives = new SaveConfig();
                    WriteArchiveConfig(JsonConvert.SerializeObject(archives));
                    return archives;
                }
                
                return JsonConvert.DeserializeObject<SaveConfig>(config);
            }

            public void WriteCfg(SaveConfig archives)
            {
                WriteArchiveConfig(JsonConvert.SerializeObject(archives));
            }

            public abstract bool DeleteGame(string archiveName, string saveName);
            protected abstract string ReadSave(string archiveName, string saveName);
            protected abstract void WriteSave(string saveStr, string archiveName, string saveName);
            protected abstract string ReadArchiveConfig();
            protected abstract void WriteArchiveConfig(string cfg);
        }

        private abstract class GameSaveFolder: GameSave
        {
            public abstract string saveFolder { get; }

            private string GetSavePath(string archiveName, string saveName)
            {
                if(!Directory.Exists(Path.Combine(saveFolder,archiveName)))
                {
                    Directory.CreateDirectory(Path.Combine(saveFolder, archiveName));
                }
                
                return Path.Combine(saveFolder, $"{archiveName}/{saveConvert.GetSaveFileName(saveName)}");
            }
            
            private string GetArcFilePath()
            {
                var path = Path.Combine(saveFolder, $"ArchiveCfg.json");
                if(!File.Exists(path))
                {
                    File.Create(path);
#if  UNITY_EDITOR
                 AssetDatabase.Refresh();   
#endif
                }

                return path;
            }
            
            public override bool DeleteGame(string archiveName, string saveName)
            {
                try 
                {
                    File.Delete(GetSavePath(archiveName, saveName));
                    return true;
                }
                catch (Exception e)
                {
                    LogTool.ToolError("SaveTool", e.Message);
                    return false;
                }
            }

            protected override string ReadSave(string archiveName, string saveName)
            {
                var savePath = GetSavePath(archiveName, saveName);
                if (!File.Exists(savePath))
                {
                    return string.Empty;
                }

                return File.ReadAllText(savePath);
            }

            protected override void WriteSave(string saveStr, string archiveName, string saveName)
            {
                var savePath = GetSavePath(archiveName, saveName);
                File.WriteAllText(savePath, saveStr);
            }
            
            
            protected override string ReadArchiveConfig()
            {
               return File.ReadAllText(GetArcFilePath());
            }

            protected override void WriteArchiveConfig(string cfg)
            {
                File.WriteAllText(GetArcFilePath(), cfg);
            }
        }
        
        private class GameSaveAssets : GameSaveFolder 
        {
            public override string saveFolder
            {
                get
                {
                    var path = Path.Combine(Application.dataPath, "Save");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    return path;
                }
            }
        }

        private class GameSavePersistent : GameSaveFolder 
        {
            public override string saveFolder
            {
                get
                {
                    var path = Path.Combine(Application.persistentDataPath, "Save");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    return Path.Combine(path);
                }
            }
        }
        
        private class GameSavePlayerPrefs: GameSave
        {
            private const string kArchiveConfigKey = "Archives";
            
            private string GetSaveKey(string archiveName, string saveName)
            {
                return $"Save{archiveName}{saveName}";
            }

            public override bool DeleteGame(string archiveName, string saveName)
            {
                try
                {
                    PlayerPrefs.DeleteKey(GetSaveKey(archiveName,saveName));
                    return true;
                }
                catch (Exception e)
                {
                    LogTool.ToolError("SaveTool", e.Message);
                    return false;
                }
            }

            protected override string ReadSave(string archiveName, string saveName)
            {
                return PlayerPrefs.GetString(GetSaveKey(archiveName,saveName));
            }

            protected override void WriteSave(string saveStr, string archiveName, string saveName)
            {
                PlayerPrefs.SetString(GetSaveKey(archiveName,saveName), saveStr);
            }

            protected override string ReadArchiveConfig()
            {
                return PlayerPrefs.GetString(kArchiveConfigKey);
            }

            protected override void WriteArchiveConfig(string cfg)
            {
                PlayerPrefs.SetString(kArchiveConfigKey, JsonConvert.SerializeObject(cfg));
            }
        }
        
        #endregion
        
    }
}