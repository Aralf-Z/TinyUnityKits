using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Cysharp.Threading.Tasks;
using UnityEngine;
using YooAsset;

namespace ZToolKit
{
    public static class YooAssetBehaviour
    {
        public static async UniTask InitYooAsset()
        {
            LogTool.ToolInfo("YooAsset", $"playMode: {GameConfig.PlayMode}");
            
            // 初始化资源系统
            YooAssets.Initialize();

            // 创建默认的资源包
            var package = YooAssets.CreatePackage("DefaultPackage");

            // 设置该资源包为默认的资源包，可以使用YooAssets相关加载接口加载该资源包内容。
            YooAssets.SetDefaultPackage(package);

            switch (GameConfig.PlayMode)
            {
                //编辑器运行模式
                case EPlayMode.EditorSimulateMode:
                    var initParametersEditor = new EditorSimulateModeParameters();
                    var simulateManifestFilePath = EditorSimulateModeHelper.SimulateBuild(EDefaultBuildPipeline.BuiltinBuildPipeline, "DefaultPackage");
                    initParametersEditor.SimulateManifestFilePath  = simulateManifestFilePath;
                    await package.InitializeAsync(initParametersEditor);
                    break;
                
                //单机运行模式
                case EPlayMode.OfflinePlayMode:
                    var initParametersOffline = new OfflinePlayModeParameters();
                    await package.InitializeAsync(initParametersOffline);
                    break;
                
                //联机运行模式 todo 联机运行模式
                case EPlayMode.HostPlayMode:
                    string defaultHostServerHost = "http://127.0.0.1/CDN/Android/v1.0";
                    string fallbackHostServerHost = "http://127.0.0.1/CDN/Android/v1.0";
                    var initParametersHost = new HostPlayModeParameters();
                    initParametersHost.BuildinQueryServices = new GameQueryServices(); 
                    initParametersHost.DecryptionServices = new FileOffsetDecryption();
                    initParametersHost.RemoteServices = new RemoteServices(defaultHostServerHost, fallbackHostServerHost);
                    var initOperationHost = package.InitializeAsync(initParametersHost);
                    await initOperationHost;
                    if (initOperationHost.Status == EOperationStatus.Succeed)
                    {
                        LogTool.Info("YooAsset", "HostPlayMode Init Succeed", "bd868dff".Hex2Color());
                    }
                    else
                    {
                        LogTool.Error("YooAsset", $"HostPlayMode Init Failed: {initOperationHost.Error}");
                    }
                    
                    break;
                //WebGL运行模式 todo webgl运行模式
                case EPlayMode.WebPlayMode:
                    string path =  Path.Combine(Application.streamingAssetsPath, "yoo");
                    string defaultHostServerWeb = path;
                    string fallbackHostServerWeb = path;
                    var initParametersWeb = new WebPlayModeParameters();
                    initParametersWeb.BuildinQueryServices = new GameQueryServices();
                    initParametersWeb.RemoteServices = new RemoteServices(defaultHostServerWeb, fallbackHostServerWeb);
                    var initOperationWeb = package.InitializeAsync(initParametersWeb);
                    await initOperationWeb;
                    if (initOperationWeb.Status == EOperationStatus.Succeed)
                    {
                        LogTool.Info("YooAsset", "WebPlayMode Init Succeed", "bd868dff".Hex2Color());
                    }
                    else
                    {
                        LogTool.Error("YooAsset", $"WebPlayMode Init Failed: {initOperationWeb.Error}");
                    }
                    break;
            }
        }
        
        #region GameQueryServices

        /// <summary>
        /// 资源文件查询服务类
        /// </summary>
        private class GameQueryServices : IBuildinQueryServices
        {
            /// <summary>
            /// 查询内置文件的时候，是否比对文件哈希值
            /// </summary>
            public static bool CompareFileCRC = false;

            public bool Query(string packageName, string fileName, string fileCRC)
            {
                // 注意：fileName包含文件格式
                return StreamingAssetsHelper.FileExists(packageName, fileName, fileCRC);
            }
        }
    
#if UNITY_EDITOR
        private static class StreamingAssetsHelper
        {
            public static void Init()
            {
            }

            public static bool FileExists(string packageName, string fileName, string fileCRC)
            {
                string filePath = Path.Combine(Application.streamingAssetsPath, StreamingAssetsDefine.RootFolderName,
                    packageName, fileName);
                if (File.Exists(filePath))
                {
                    if (GameQueryServices.CompareFileCRC)
                    {
                        string crc32 = YooAsset.HashUtility.FileCRC32(filePath);
                        return crc32 == fileCRC;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }
        }
#else
        private static class StreamingAssetsHelper
        {
            private class PackageQuery
            {
                public readonly Dictionary<string, BuildinFileManifest.Element> Elements =
                    new Dictionary<string, BuildinFileManifest.Element>(1000);
            }

            private static bool _isInit = false;

            private static readonly Dictionary<string, PackageQuery> _packages =
                new Dictionary<string, PackageQuery>(10);

            /// <summary>
            /// 初始化
            /// </summary>
            public static void Init()
            {
                if (_isInit == false)
                {
                    _isInit = true;

                    var manifest = Resources.Load<BuildinFileManifest>("BuildinFileManifest");
                    if (manifest != null)
                    {
                        foreach (var element in manifest.BuildinFiles)
                        {
                            if (_packages.TryGetValue(element.PackageName, out PackageQuery package) == false)
                            {
                                package = new PackageQuery();
                                _packages.Add(element.PackageName, package);
                            }

                            package.Elements.Add(element.FileName, element);
                        }
                    }
                }
            }

            /// <summary>
            /// 内置文件查询方法
            /// </summary>
            public static bool FileExists(string packageName, string fileName, string fileCRC32)
            {
                if (_isInit == false)
                    Init();

                if (_packages.TryGetValue(packageName, out PackageQuery package) == false)
                    return false;

                if (package.Elements.TryGetValue(fileName, out var element) == false)
                    return false;

                if (GameQueryServices.CompareFileCRC)
                {
                    return element.FileCRC32 == fileCRC32;
                }
                else
                {
                    return true;
                }
            }

            /// <summary>
            /// 内置资源清单
            /// </summary>
            public class BuildinFileManifest : ScriptableObject
            {
                [Serializable]
                public class Element
                {
                    public string PackageName;
                    public string FileName;
                    public string FileCRC32;
                }

                public List<Element> BuildinFiles = new List<Element>();
            }
        }
#endif
        private static class StreamingAssetsDefine
        {
            /// <summary>
            /// 根目录名称（保持和YooAssets资源系统一致）
            /// </summary>
            public const string RootFolderName = "yoo";
        }
    
        #endregion

        #region FileOffsetDecryption

        /// <summary>
        /// 资源文件流加载解密类
        /// </summary>
        private class FileOffsetDecryption : IDecryptionServices
        {
            /// <summary>
            /// 同步方式获取解密的资源包对象
            /// 注意：加载流对象在资源包对象释放的时候会自动释放
            /// </summary>
            AssetBundle IDecryptionServices.LoadAssetBundle(DecryptFileInfo fileInfo, out Stream managedStream)
            {
                BundleStream bundleStream =
                    new BundleStream(fileInfo.FileLoadPath, FileMode.Open, FileAccess.Read, FileShare.Read);
                managedStream = bundleStream;
                return AssetBundle.LoadFromStream(bundleStream, fileInfo.ConentCRC, GetManagedReadBufferSize());
            }

            /// <summary>
            /// 异步方式获取解密的资源包对象
            /// 注意：加载流对象在资源包对象释放的时候会自动释放
            /// </summary>
            AssetBundleCreateRequest IDecryptionServices.LoadAssetBundleAsync(DecryptFileInfo fileInfo,
                out Stream managedStream)
            {
                BundleStream bundleStream =
                    new BundleStream(fileInfo.FileLoadPath, FileMode.Open, FileAccess.Read, FileShare.Read);
                managedStream = bundleStream;
                return AssetBundle.LoadFromStreamAsync(bundleStream, fileInfo.ConentCRC, GetManagedReadBufferSize());
            }

            private static uint GetManagedReadBufferSize()
            {
                return 1024;
            }
        }

        /// <summary>
        /// 资源文件解密流
        /// </summary>
        private class BundleStream : FileStream
        {
            public const byte Key = 64;

            public BundleStream(string path, FileMode mode, FileAccess access, FileShare share) : base(path, mode,
                access, share)
            {
            }

            public BundleStream(string path, FileMode mode) : base(path, mode)
            {
            }

            public override int Read(byte[] array, int offset, int count)
            {
                var index = base.Read(array, offset, count);
                for (int i = 0; i < array.Length; i++)
                {
                    array[i] ^= Key;
                }

                return index;
            }
        }
        #endregion
        
        #region RemoteServices
    
        /// <summary>
        /// 远端资源地址查询服务类
        /// </summary>
        private class RemoteServices : IRemoteServices
        {
            private readonly string mDefaultHostServer;
            private readonly string mFallbackHostServer;

            public RemoteServices(string defaultHostServer, string fallbackHostServer)
            {
                mDefaultHostServer = defaultHostServer;
                mFallbackHostServer = fallbackHostServer;
            }
            string IRemoteServices.GetRemoteMainURL(string fileName)
            {
                return $"{mDefaultHostServer}/{fileName}";
            }
            string IRemoteServices.GetRemoteFallbackURL(string fileName)
            {
                return $"{mFallbackHostServer}/{fileName}";
            }
        }
        #endregion
    }
}
