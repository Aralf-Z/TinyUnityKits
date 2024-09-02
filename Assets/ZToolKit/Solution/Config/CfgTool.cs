/*

*/

using System;
using System.Collections.Generic;
using System.IO;
using cfg;
using Cysharp.Threading.Tasks;
using Luban;
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
        
        public static async UniTask Init()
        {
            var tablesCtor = typeof(Tables).GetConstructors()[0];
            var loaderReturnType = tablesCtor.GetParameters()[0].ParameterType.GetGenericArguments()[1];
            
#if UNITY_WEBGL && !UNITY_EDITOR
            //
            // var fileMap = loaderReturnType == typeof(ByteBuf) ?
            //     
            //
            // var loader = loaderReturnType == typeof(ByteBuf) 
            //     ? new Func<string, ByteBuf>(LoadByteBuf_Web)
            //     : (Delegate)new Func<string, JSONNode>(LoadJson_Web);
            //
            // sTables = (Tables)tablesCtor.Invoke(new object[] {loader});
            
#else
            var loader = loaderReturnType == typeof(ByteBuf) 
                ? new Func<string, ByteBuf>(LoadByteBuf)
                : (Delegate)new Func<string, JSONNode>(LoadJson);
            
            sTables = (Tables)tablesCtor.Invoke(new object[] {loader});
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
        
        // private static UniTask<Dictionary<string, ByteBuf>> LoadByteBuf_Web(string file)
        // {
        //     using var request = UnityWebRequest.Get(Path.Combine(rFoldPath, $"{file}.bytes"));
        //     request.SendWebRequest();
        //     
        //     if (request.result == UnityWebRequest.Result.Success)
        //     {
        //         var bytes = request.downloadHandler.data;
        //         return new ByteBuf(bytes);
        //     }
        //     
        // }
        //
        // private static JSONNode LoadJson_Web(string file)
        // {
        //     try
        //     {
        //         using var request = UnityWebRequest.Get(Path.Combine(rFoldPath, $"{file}.json"));
        //         request.SendWebRequest().GetAwaiter().GetResult();
        //     
        //         Debug.LogError(request.result);
        //         Debug.LogError(request.result == UnityWebRequest.Result.Success);
        //         if (request.result == UnityWebRequest.Result.Success)
        //         {
        //             Debug.LogError(request.downloadHandler.text);
        //             var text = request.downloadHandler.text;
        //             return JSON.Parse(text);
        //         }
        //
        //         Debug.LogError(request.error);
        //     }
        //     catch (Exception e)
        //     {
        //         Debug.LogError(e);
        //         throw;
        //     }
        //     
        //     return null;
        // }
    }
} 