using System;
using System.Collections.Generic;
using System.IO;
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
        /// <summary> 最大存档数量 </summary>
        public int maxCount;
        
        public Archive(string name)
        {
            this.name = name;
            saves = new List<string>();
            maxCount = 25;
        }
    }

    /// <summary>
    /// 存档相关的配置文件
    /// </summary>
    public class SaveConfig
    {
        public string curArchive;
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

        public static void WriteSave<T>(T save) where T : SaveBase, new()
        {
            gameSave.SaveGame(save);
        }

        public static void DeleteSave(string archiveName, string saveName)
        {
            gameSave.DeleteGame(archiveName, saveName);
        }

        public static SaveConfig ReadConfig()
        {
            return gameSave.LoadCfg();
        }
        
        public static void WriteConfig(SaveConfig cfg)
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
            void SaveGame<TSave>(TSave save)where TSave : SaveBase, new();
            void DeleteGame(string archiveName, string saveName);

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

            public void SaveGame<TSave>(TSave save)where TSave : SaveBase, new()
            {
                WriteSave(saveConvert.SerializeSave(save), save.archive, save.name);
                LogTool.ToolInfo("SaveTool", $"{save.archive}:{save.name} saved.");
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

            public abstract void DeleteGame(string archiveName, string saveName);
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
            
            public override void DeleteGame(string archiveName, string saveName)
            {
                File.Delete(GetSavePath(archiveName, saveName));
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

            public override void DeleteGame(string archiveName, string saveName)
            {
                PlayerPrefs.DeleteKey(GetSaveKey(archiveName,saveName));
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