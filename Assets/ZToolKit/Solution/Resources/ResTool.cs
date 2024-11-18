using System;
using System.Collections.Generic;
using System.IO;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using YooAsset;
#if (UNITY_WEBGL || UNITY_ANDROID) && !UNITY_EDITOR
using UnityEngine.Networking;
#endif
using Object = UnityEngine.Object;

namespace ZToolKit
{
    public enum ResMode
    {
        ResourcesLoad,
        YooAsset,
    }
    
    public static class ResTool
    {
        public const string ResCatalog = "ZTool/Resources/ResCatalog.json";
#if UNITY_EDITOR
        private static bool sInited;
#endif
        
        private static ResLoadHandlerBase sCurHandler;

        public static async UniTask Init()
        {
            sCurHandler = GameConfig.ResMode switch
            {
                ResMode.ResourcesLoad => new ResourcesHandler(),
                ResMode.YooAsset => new YooAssetHandler(),
                _ => throw new ArgumentOutOfRangeException()
            };

            await sCurHandler.InitHandler();
#if UNITY_EDITOR    
            sInited = true;
#endif
        }
        
        public static T Load<T>(string resName) where T : Object
        {
            CheckInit(); 
            return sCurHandler.LoadAsset<T>(resName);
        }
        
#if UNITY_EDITOR      
        private static void CheckInit()
        {
            if (!sInited)
            {
                try
                {
                    if (GameConfig.ResMode == ResMode.ResourcesLoad)
                    {
                        sCurHandler = new ResourcesHandler();
                    }
                    else if (GameConfig.ResMode == ResMode.YooAsset)
                    {
                        LogTool.ToolError("ResTool", "YooAsset Not Supported Lazy Load");
                    }
                    sCurHandler.LazyInitHandler();
                    LogTool.ToolInfo("ResTool", "Lazy Load");
                }
                catch (Exception e)
                {
                    LogTool.ToolError("ResTool", "Lazy Load Error");
                }
            }
        }
#endif
        
        private abstract class ResLoadHandlerBase
        {
#if UNITY_EDITOR
            public virtual void LazyInitHandler(){}
#endif
            public abstract UniTask InitHandler();
            public abstract T LoadAsset<T>(string resName) where T : Object;
        }

        private class ResourcesHandler: ResLoadHandlerBase
        {
            private static Dictionary<string, string> sNamePathDic;
#if UNITY_EDITOR
            public override void LazyInitHandler()
            {
                try
                { 
                    string filePath = Path.Combine(Application.streamingAssetsPath, ResCatalog);
                    sNamePathDic = JsonConvert.DeserializeObject<ResCatalog>(File.ReadAllText(filePath))?.namePathDic;
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                    throw;
                }
            }
#endif
            public override async UniTask InitHandler()
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
            }

            public override T LoadAsset<T>(string resName)
            {
                if (sNamePathDic.ContainsKey(resName))
                {
                    return Resources.Load<T>(sNamePathDic[resName]);
                }

                return null;
            }
        }

        private class YooAssetHandler : ResLoadHandlerBase
        {
            public override async UniTask InitHandler()
            {
                await YooAssetBehaviour.InitYooAsset();
            }
            
            public override T LoadAsset<T>(string resName)
            {
                return YooAssets.LoadAssetSync<T>(resName).GetAssetObject<T>();
            }
        }
    }
}