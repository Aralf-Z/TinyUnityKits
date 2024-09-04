/*

*/

using System;
using System.Collections.Generic;
using System.IO;
using cfg;
using Cysharp.Threading.Tasks;
using Luban;
using Newtonsoft.Json;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Networking;

namespace ZToolKit
{
    public static class CfgTool
    {
        public static Tables Tables
        {
            get
            {
                if (!sInited)
                {
                    Init().GetAwaiter().GetResult();
                    LogTool.ZToolKitLog("CfgTool", "Lazy Load");
                }

                return sTables;
            }
        }
        
        public static Audio Audio => Tables.TbAudio.DataList[0];
        
        private static Tables sTables;

        private static readonly string rFoldPath = Path.Combine(Application.streamingAssetsPath, "TableConfig");
        
        private static bool sInited;

        public static readonly string fileListPath = Path.Combine(Application.streamingAssetsPath, "cfgFileList.json"); 
        
        public static async UniTask Init()
        {
            var tablesCtor = typeof(Tables).GetConstructors()[0];
            var loaderReturnType = tablesCtor.GetParameters()[0].ParameterType.GetGenericArguments()[1];
            
#if (UNITY_WEBGL || UNITY_ANDROID) && !UNITY_EDITOR
            //只有这里采取异步的方式，因为UniTask不能使用.GetAwaiter().GetResult();故懒加载是不支持web和安卓的
            try
            {
                using var request = UnityWebRequest.Get(fileListPath);
                await request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    var fileContent = request.downloadHandler.text;
                    var fileList = JsonConvert.DeserializeObject<List<string>>(fileContent);
                    var byteMaps = new Dictionary<string, ByteBuf>();
                    var jsonMaps = new Dictionary<string, JSONNode>();


                    if (loaderReturnType == typeof(ByteBuf))
                    {
                        byteMaps = await LoadByteBuf_Web(fileList);
                    }
                    else
                    {
                        jsonMaps = await LoadJson_Web(fileList);
                    }

                    var loader = loaderReturnType == typeof(ByteBuf)
                        ? new Func<string, ByteBuf>(file => byteMaps[file])
                        : (Delegate) new Func<string, JSONNode>(file => jsonMaps[file]);

                    sTables = (Tables) tablesCtor.Invoke(new object[] {loader});
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
                var loader = loaderReturnType == typeof(ByteBuf) 
                    ? new Func<string, ByteBuf>(LoadByteBuf)
                    : (Delegate)new Func<string, JSONNode>(LoadJson);
            
                sTables = (Tables)tablesCtor.Invoke(new object[] {loader});
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                throw;
            }
#endif
            sInited = true;
        }
        

        private static ByteBuf LoadByteBuf(string file)
        {
            return new (File.ReadAllBytes(Path.Combine(rFoldPath, $"{file}.bytes")));
        }

        private static JSONNode LoadJson(string file)
        {
            return JSON.Parse(File.ReadAllText(Path.Combine(rFoldPath, $"{file}.json")));
        }
        
        private static async UniTask<Dictionary<string, ByteBuf>> LoadByteBuf_Web(List<string> files)
        {
            var bytesMap = new Dictionary<string, ByteBuf>();
            
            foreach (var file in files)
            {
                using var request = UnityWebRequest.Get(Path.Combine(rFoldPath, $"{file}.bytes"));
                await request.SendWebRequest();
            
                if (request.result == UnityWebRequest.Result.Success)
                {
                    var bytes = request.downloadHandler.data;
                    bytesMap.Add(file, new ByteBuf(bytes)); 
                }
            }

            return bytesMap;
        }

        private static async UniTask<Dictionary<string, JSONNode>> LoadJson_Web(List<string> files)
        {
            var jsonMaps = new Dictionary<string, JSONNode>();
            
            foreach (var file in files)
            {
                using var request = UnityWebRequest.Get(Path.Combine(rFoldPath, $"{file}.json"));
                await request.SendWebRequest();
            
                if (request.result == UnityWebRequest.Result.Success)
                {
                    var jsonStr = request.downloadHandler.text;
                    jsonMaps.Add(file, JSON.Parse(jsonStr)); 
                }
            }

            return jsonMaps;
        }
    }
} 