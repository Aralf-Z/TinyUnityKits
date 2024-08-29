/*

*/

using System.Collections;
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
        public static string ResConfig = "ResourcesCatalog.json";
        
        private static Dictionary<string, string> sNamePathDic;

        static ResTool()
        {
            
        }

        public static async UniTask Init()
        {
            await LoadJson();
        }
        
        public static T Load<T>(string prefabName) where T : Object
        {
            if (sNamePathDic.ContainsKey(prefabName))
            {
                return Resources.Load<T>(sNamePathDic[prefabName]);
            }
            
            LogTool.EditorLogError($"ResLoad---Failed To Load {prefabName}");
            return null;
        }
        
        public static async UniTask LoadJson()
        {
            string filePath = Path.Combine(Application.streamingAssetsPath, ResConfig);

#if UNITY_WEBGL
            using var request = UnityWebRequest.Get(filePath);
            await request.SendWebRequest().ToUniTask();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string fileContent = request.downloadHandler.text;
                sNamePathDic = JsonConvert.DeserializeObject<ResourcesCatalog>(fileContent)?.namePathDic;
            }
            else
            {
                Debug.LogError(request.error);
            }
#else
            if (File.Exists(filePath))
            {
                sNamePathDic = JsonConvert.DeserializeObject<ResourcesCatalog>(File.ReadAllText(filePath))?.namePathDic;
            }
            else
            {
                Debug.LogError(request.error);
            }
#endif
        }
    }
}