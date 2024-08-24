/*

*/

using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

namespace ZToolKit
{
    public enum SaveFolder
    {
        /// <summary> 设备默认位置 </summary>
        Device = 1,

        /// <summary> 游戏文件位置 </summary>
        Game = 2
    }
    
    public static class GameSave
    {
        public static readonly Dictionary<SaveFolder, string> SaveFolderDic = new()
        {
            [SaveFolder.Device] = Path.Combine(Application.persistentDataPath, "Save"),
            [SaveFolder.Game] = Path.Combine(Application.dataPath, "Save"),
        };

        private const string kSaveFile = "Save.json";
        
        public static TSave LoadGame<TSave >() where TSave : new()
        {
#if UNITY_EDITOR
            if (!EditorPrefs.GetBool(EditorPrefsKeys.LoadSaveInEditor))
            {
                return new TSave ();
            }
#endif
            return LoadSave<TSave>();
        }

        public static void SaveGame<TSave>(TSave save) where TSave: new()
        {
#if UNITY_EDITOR
            if (!EditorPrefs.GetBool(EditorPrefsKeys.CanSaveInEditor)) return;
#endif
            JsonSerializerSettings jsonSerializerSettings = new();
            jsonSerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            jsonSerializerSettings.NullValueHandling = NullValueHandling.Ignore;

            var saveJson = JsonConvert.SerializeObject(save, jsonSerializerSettings);
            var savePath = GetSavePath();
            
            File.WriteAllText(savePath, saveJson);
            LogTool.ZToolKitLog("GameSave","Game saved successfully!");
        }

        private static TSave LoadSave<TSave>() where TSave: new()
        {
            try
            {
                var filePath = GetSavePath();
                return LoadSave<TSave>(filePath);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }

            return new TSave();
        }

        private static TSave LoadSave<TSave>(string filePath) where TSave: new()
        {
            if (!File.Exists(filePath))
            {
                return new TSave();
            }
            
            var saveJson = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<TSave>(saveJson);
        }
        
        private static string GetSavePath()
        {
            var path = SaveFolderDic[SaveFolder.Game];
            // var gameConfig = ResTool.Load<GameConfig>(nameof(GameConfig));
            //
            // if (gameConfig)
            // {
            //     path = SaveFolderDic[gameConfig.saveFolder]; 
            // }
            // else
            // {
            //     LogTool.ZToolKitLogError("ResLoad", "Failed To Load GameConfig");
            // }
            //
            //
            // if (!Directory.Exists(path))
            // {
            //     Directory.CreateDirectory(path);
            // }

            return Path.Combine(path, kSaveFile);
        }
    }
}