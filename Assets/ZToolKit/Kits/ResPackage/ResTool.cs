using System;
using System.Collections.Generic;
using System.IO;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using Object = UnityEngine.Object;

#if (UNITY_WEBGL || UNITY_ANDROID) && !UNITY_EDITOR
using UnityEngine.Networking;
#endif

namespace ZToolKit
{
    public static class ResTool
    {
        public const string ResCatalog = "ZTool/Resources/ResCatalog.json";

        private static Dictionary<string, string> sNamePathDic;
        
        private static bool sInited;

        public static async UniTask Init()
        {
            string filePath = Path.Combine(Application.streamingAssetsPath, ResCatalog);

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
                sNamePathDic = JsonConvert.DeserializeObject<ResCatalog>(await File.ReadAllTextAsync(filePath))?.namePathDic;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                throw;
            }
#endif
            sInited = true;
        }
        
        
        
        public static T Load<T>(string resName) where T : Object
        {
            if (!sInited)
                throw new NullReferenceException();
            
            return sNamePathDic.TryGetValue(resName, out string value) ? Resources.Load<T>(value) : null;
        }
    }
}