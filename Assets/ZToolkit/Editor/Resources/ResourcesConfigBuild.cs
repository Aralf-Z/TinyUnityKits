/*

*/

using System.IO;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

namespace ZToolKit.Editor
{
    public class ResourcesConfigBuild : UnityEditor.Editor
    {
        [MenuItem("ZToolKit/ResourcesConfigBuild")]
        private static void ConfigBuild()
        {
            var GUIDs = AssetDatabase.FindAssets("",new string[]{"Assets/Resources"});
            var resConfig = new AllResourcesNamePathPairs();
            int prePathLength = "Assets/Resources/".Length;
            for(int i = 0; i< GUIDs.Length;++i)
            {
                var originalPath = AssetDatabase.GUIDToAssetPath(GUIDs[i]);
                var resPath = originalPath.Substring(prePathLength);
                var path = resPath.Split('.');
                if(path.Length == 1) continue;//文件夹名跳过
                EditorUtility.DisplayProgressBar("ResourcesConfigBuilding",originalPath,(float)i/GUIDs.Length);
                resConfig.AddPair(AssetDatabase.LoadAssetAtPath<Object>(originalPath).name, path[0]);
            }
            EditorUtility.ClearProgressBar();
            CreateAllResPathData(resConfig);
            LogTool.ZToolKitLog("Resources","资源路径配置完成");
        }

        private static void CreateAllResPathData(AllResourcesNamePathPairs data)
        {
            var dataPath = Path.Combine(Application.streamingAssetsPath, "AllResPathData.json");
            if (!Directory.Exists(Application.streamingAssetsPath))
            {
                Directory.CreateDirectory(Application.streamingAssetsPath);
                AssetDatabase.Refresh();
            }
            if (!File.Exists(dataPath))
            {
                File.Create(dataPath);
                AssetDatabase.Refresh();
            }
            File.WriteAllText(dataPath, JsonConvert.SerializeObject(data));
            AssetDatabase.Refresh();
        }
    }
}