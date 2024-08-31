/*

*/

using System;
using System.IO;
using cfg;
using Cysharp.Threading.Tasks;
using Luban;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Networking;

namespace ZToolKit
{
    public static class Config
    {
        public static Tables Tables;

        public static Audio audio => Tables.TbAudio.DataList[0];
        
        private static readonly string rFoldPath = Path.Combine(Application.streamingAssetsPath, "TableConfig");
        
        public static async UniTask Init()
        {
            var tablesCtor = typeof(Tables).GetConstructors()[0];
            var loaderReturnType = tablesCtor.GetParameters()[0].ParameterType.GetGenericArguments()[1];
            
#if UNITY_WEBGL
            var loader = loaderReturnType == typeof(ByteBuf) 
                ? new Func<string, ByteBuf>(LoadByteBuf_Web)
                : (Delegate)new Func<string, JSONNode>(LoadJson_Web);
            
            Tables = (Tables)tablesCtor.Invoke(new object[] {loader});
            
#else
            var loader = loaderReturnType == typeof(ByteBuf) 
                ? new Func<string, ByteBuf>(LoadByteBuf)
                : (Delegate)new Func<string, JSONNode>(LoadJson);
            
            Tables = (Tables)tablesCtor.Invoke(new object[] {loader});
#endif
        }

        private static ByteBuf LoadByteBuf(string file)
        {
            return new (File.ReadAllBytes(Path.Combine(rFoldPath, $"{file}.bytes")));
        }

        private static JSONNode LoadJson(string file)
        {
            return JSON.Parse(File.ReadAllText(Path.Combine(rFoldPath, $"{file}.json")));
        }
        
        //todo 现在使用的是阻塞主线程加载的方法，web端需要使用懒加载避免卡顿
        private static ByteBuf LoadByteBuf_Web(string file)
        {
            using var request = UnityWebRequest.Get(Path.Combine(rFoldPath, $"{file}.bytes"));
            request.SendWebRequest().GetAwaiter().GetResult();
            
            if (request.result == UnityWebRequest.Result.Success)
            {
                var bytes = request.downloadHandler.data;
                return new ByteBuf(bytes);
            }

            Debug.LogError(request.error);
            return null;
        }
        
        private static JSONNode LoadJson_Web(string file)
        {
            using var request = UnityWebRequest.Get(Path.Combine(rFoldPath, $"{file}.bytes"));
            request.SendWebRequest().GetAwaiter().GetResult();
            
            if (request.result == UnityWebRequest.Result.Success)
            {
                var text = request.downloadHandler.text;
                return JSON.Parse(text);
            }

            Debug.LogError(request.error);
            return null;
        }
    }
} 