/*

*/

using System;
using System.Collections.Generic;
using System.IO;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using Object = UnityEngine.Object;

namespace ZToolKit
{
    public static class ResTool
    {
        private static bool mInited;
        
        public static string ResConfig = "ResCatalog.json";

        private static Dictionary<string, string> sNamePathDic;

        public static async UniTask Init()
        {
            await LoadJson();
            mInited = true;
        }
        
        public static T Load<T>(string prefabName) where T : Object
        {
            CheckInit();
            
            if (sNamePathDic.ContainsKey(prefabName))
            {
                return Resources.Load<T>(sNamePathDic[prefabName]);
            }
            
            LogTool.EditorLogError($"ResLoad---Failed To Load {prefabName}");
            return null;
        }
        
        internal static bool IsExist(string prefabName)
        {
            CheckInit();
            return sNamePathDic.ContainsKey(prefabName);
        }

        private static void CheckInit()
        {
            if (!mInited)
            {
                try
                {
                    Init().GetAwaiter().GetResult();
                    LogTool.ZToolKitLog("ResTool", "Lazy Load");
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                    LogTool.ZToolKitLogError("ResTool", "Lazy Load Error");
                }
            }
        }
        
        public static async UniTask LoadJson()
        {
            string filePath = Path.Combine(Application.streamingAssetsPath, ResConfig);

#if (UNITY_WEBGL || UNITY_ANDROID) && !UNITY_EDITOR
            try
            {
                using var request = UnityWebRequest.Get(filePath);
                await request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    string fileContent = request.downloadHandler.text;
                    sNamePathDic = JsonConvert.DeserializeObject<ResCatalog>(fileContent)?.namePathDic;
                }
                else
                {
                    Debug.LogError(request.error);
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                throw;
            }
#else
            try
            { 
                sNamePathDic = JsonConvert.DeserializeObject<ResCatalog>(File.ReadAllText(filePath))?.namePathDic;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                throw;
            }
#endif
        }
    }
}