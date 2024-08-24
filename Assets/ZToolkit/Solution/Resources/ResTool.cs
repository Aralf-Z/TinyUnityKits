/*

*/

using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using Object = UnityEngine.Object;

namespace ZToolKit
{
    public static class ResTool
    {
        private static Dictionary<string, string> sNamePathDic;

        static ResTool()
        {
            // var jsonString = File.ReadAllText(Path.Combine(Application.streamingAssetsPath, "AllResPathData.json"));
            // sNamePathDic = JsonConvert.DeserializeObject<AllResourcesNamePathPairs>(jsonString)?.namePathDic;
        }

        public static void Init()
        {
            GameEntry.Instance.StartCoroutine(LoadJson());
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
        
        private static IEnumerator LoadJson()
        {
            // StreamingAssets目录下的文件路径
            string filePath = Path.Combine(Application.streamingAssetsPath, "myFile.json");
        
            // 使用UnityWebRequest加载文件
            UnityWebRequest request = UnityWebRequest.Get(filePath);
            
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(request.error);
            }
            else
            {
                // 获取文本内容
                string fileContent = request.downloadHandler.text;
                sNamePathDic = JsonConvert.DeserializeObject<AllResourcesNamePathPairs>(fileContent)?.namePathDic;
            
                // 如果是二进制文件，可以使用以下代码：
                // byte[] fileBytes = request.downloadHandler.data;
            }
        }
    }
}