/*

*/

using System.IO;
using cfg;
using Cysharp.Threading.Tasks;
using Luban;
using Newtonsoft.Json;
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
#if UNITY_WEBGL
            //todo 现在使用的是阻塞主线程加载的方法，web端需要使用懒加载避免卡顿
            Tables = new Tables(LoadByteBuf_Web);
#else
            Tables = new Tables(LoadByteBuf);
#endif
            Debug.Log(Tables.TbAudio.DataList[0].ClickBtn);
        }

        private static ByteBuf LoadByteBuf(string file)
        {
            return new (File.ReadAllBytes(Path.Combine(rFoldPath, $"{file}.bytes")));
        }

        private static ByteBuf LoadByteBuf_Web(string file)
        {
            using var request = UnityWebRequest.Get(Path.Combine(rFoldPath, $"{file}.bytes"));
            request.SendWebRequest().GetAwaiter().GetResult();
            
            if (request.result == UnityWebRequest.Result.Success)
            {
                var data = request.downloadHandler.data;
                return new ByteBuf(data);
            }

            Debug.LogError(request.error);
            return null;
        }
    }
} 