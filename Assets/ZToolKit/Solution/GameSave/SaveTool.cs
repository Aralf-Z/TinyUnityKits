using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

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
    /// 存档档案->可以理解成一个账号数据,只记录信息，不记录存档
    /// </summary>
    public class SaveArchive
    {
        /// <summary> 档案名称 </summary>
        public string archiveName;
        public List<string> saves;
        /// <summary> 档案下的所有存档 </summary>

        public SaveArchive(string archiveName)
        {
            this.archiveName = archiveName;
            saves = new List<string>();
        }

        /// <summary>
        /// 添加存档
        /// </summary>
        /// <param name="saveName">存档名称</param>
        /// <returns>是否重名</returns>
        public bool AddSave(string saveName)
        {
            if (saves.Contains(saveName))
            {
                return true;
            }
            saves.Add(saveName);
            return false;
        }

        public bool Contains(string saveName)
        {
            return saves.Contains(saveName);
        }
    }
    
    public static class SaveTool
    {
        private static IGameSave gameSave;
        
        public static ISaveConvert saveConvert;
        
        public static SaveArchive curArchive;

        public static void Init()
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

        public static T Load<T>(string saveName) where T : SaveBase, new()
        {
            return gameSave.LoadGame<T>(curArchive.archiveName, saveName);
        }

        public static bool Save<T>(T save) where T : SaveBase, new()
        {
            return gameSave.SaveGame(save);
        }
        

        #region Convert

        public interface ISaveConvert
        {
            T StringConvert<T>(string str);
        }
    
        public class JsonSaveConvert:
            ISaveConvert
        {
            public T StringConvert<T>(string str)
            {
                return JsonConvert.DeserializeObject<T>(str);
            }
        }

        #endregion

        #region GameSave
        
        internal interface IGameSave 
        {
            /// <summary>
            /// 加载存档列表
            /// </summary>
            /// <returns></returns>
            List<SaveArchive> LoadSaveList();
        
            TSave LoadGame<TSave>(string archiveName, string saveName) where TSave : SaveBase, new();
            bool SaveGame<TSave>(TSave save)where TSave : SaveBase, new();

            UniTask<TSave> LoadGameAsync<TSave>(string archiveName, string saveName)where TSave : SaveBase, new();
            UniTask SaveGameAsync<TSave>(TSave save)where TSave : SaveBase, new();
        }

        internal abstract class GameSaveFolder :
            IGameSave
        {
            public abstract string saveFolder { get; }

            public List<SaveArchive> LoadSaveList()
            {
                throw new NotImplementedException();
            }

            public TSave LoadGame<TSave>(string archiveName, string saveName) where TSave : SaveBase, new()
            {
                try
                {
                    if (!File.Exists(saveFolder))
                    {
                        return new TSave();
                    }

                    var saveStr = File.ReadAllText(saveFolder);
                    return SaveTool.saveConvert.StringConvert<TSave>(saveStr);
                }
                catch (Exception e)
                {
                    LogTool.ToolError("SaveTool", e.Message);
                }

                return new TSave();
            }

            public bool SaveGame<TSave>(TSave save) where TSave : SaveBase, new()
            {
                try
                {
                    JsonSerializerSettings jsonSerializerSettings = new();
                    jsonSerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    jsonSerializerSettings.NullValueHandling = NullValueHandling.Ignore;

                    var saveJson = JsonConvert.SerializeObject(save, jsonSerializerSettings);

                    File.WriteAllText(saveFolder, saveJson);
                    LogTool.ToolInfo("GameSave", "Game saved successfully!");
                    return true;
                }
                catch (Exception e)
                {
                    LogTool.ToolError("SaveTool", e.Message);
                }

                return false;
            }

            public UniTask<TSave> LoadGameAsync<TSave>(string archiveName, string saveName)
                where TSave : SaveBase, new()
            {
                throw new NotImplementedException();
            }

            public UniTask SaveGameAsync<TSave>(TSave save) where TSave : SaveBase, new()
            {
                throw new NotImplementedException();
            }
        }

        public class GameSaveAssets : 
            IGameSave
        {
            public List<SaveArchive> LoadSaveList()
            {
                throw new NotImplementedException();
            }

            public TSave LoadGame<TSave>(string archiveName, string saveName) where TSave : SaveBase, new()
            {
                throw new NotImplementedException();
            }

            public bool SaveGame<TSave>(TSave save) where TSave : SaveBase, new()
            {
                throw new NotImplementedException();
            }

            public UniTask<TSave> LoadGameAsync<TSave>(string archiveName, string saveName) where TSave : SaveBase, new()
            {
                throw new NotImplementedException();
            }

            public UniTask SaveGameAsync<TSave>(TSave save) where TSave : SaveBase, new()
            {
                throw new NotImplementedException();
            }
        }

        public class GameSavePersistent :
            IGameSave
        {
            public List<SaveArchive> LoadSaveList()
            {
                throw new NotImplementedException();
            }

            public TSave LoadGame<TSave>(string archiveName, string saveName) where TSave : SaveBase, new()
            {
                throw new NotImplementedException();
            }

            public bool SaveGame<TSave>(TSave save) where TSave : SaveBase, new()
            {
                throw new NotImplementedException();
            }

            public UniTask<TSave> LoadGameAsync<TSave>(string archiveName, string saveName) where TSave : SaveBase, new()
            {
                throw new NotImplementedException();
            }

            public UniTask SaveGameAsync<TSave>(TSave save) where TSave : SaveBase, new()
            {
                throw new NotImplementedException();
            }
        }
        
        internal class GameSavePlayerPrefs:
            IGameSave
        {
            public List<SaveArchive> LoadSaveList()
            {
                throw new NotImplementedException();
            }

            public TSave LoadGame<TSave>(string archiveName, string saveName) where TSave : SaveBase, new()
            {
                throw new NotImplementedException();
            }

            public bool SaveGame<TSave>(TSave save) where TSave : SaveBase, new()
            {
                throw new NotImplementedException();
            }

            public UniTask<TSave> LoadGameAsync<TSave>(string archiveName, string saveName) where TSave : SaveBase, new()
            {
                throw new NotImplementedException();
            }

            public UniTask SaveGameAsync<TSave>(TSave save) where TSave : SaveBase, new()
            {
                throw new NotImplementedException();
            }
        }
        
        #endregion
        
        
    }
}