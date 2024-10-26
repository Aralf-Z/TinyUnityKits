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
    internal enum SaveFolder
    {
        /// <summary> 设备默认位置 </summary>
        Device = 1,

        /// <summary> 游戏文件位置 </summary>
        Game = 2
    }
    
    internal class GameSaveFolder :
        IGameSave
    {
        public static readonly Dictionary<SaveFolder, string> SaveFolderDic = new()
        {
            [SaveFolder.Device] = Path.Combine(Application.persistentDataPath, "Save"),
            [SaveFolder.Game] = Path.Combine(Application.dataPath, "Save"),
        };

        private const string kSaveFile = "Save.json";
        
        public Save LoadGame()
        {
#if UNITY_EDITOR
            if (!EditorPrefs.GetBool(EditorPrefsKeys.LoadSaveInEditor))
            {
                return new Save();
            }
#endif
            return LoadSave();
        }

        public void SaveGame(Save save)
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
            LogTool.ToolInfo("GameSave","Game saved successfully!");
        }

        private Save LoadSave()
        {
            try
            {
                var filePath = GetSavePath();
                return LoadSave(filePath);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }

            return new Save();
        }

        private Save LoadSave(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return new Save();
            }
            
            var saveJson = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<Save>(saveJson);
        }
        
        private string GetSavePath()
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