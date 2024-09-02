/*

*/

using System.IO;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

namespace ZToolKit.Editor
{
    public class ResourcesConfigPanel : PanelBase
    {
        public override int Priority => 5;
        public override string PanelName => "[编辑器] 资源加载设置";
        
        public override void Init()
        {
            
        }

        public override void DrawPanel(Rect windowRect)
        {
            using (var h = new GUILayout.HorizontalScope())
            {
                GUILayout.FlexibleSpace();
                GUI.backgroundColor = Color.green;

                if (GUILayout.Button("构建Resources资源目录", EditorStyles.miniButton, GUILayout.Width(150)))
                {
                    ConfigBuild();
                }

                GUI.backgroundColor = Color.white;
            }
        }
        
        private static void ConfigBuild()
        {
            var guids = AssetDatabase.FindAssets("",new string[]{"Assets/Resources"});
            var resConfig = new ResCatalog();
            int prePathLength = "Assets/Resources/".Length;
            for(int i = 0; i< guids.Length;++i)
            {
                var originalPath = AssetDatabase.GUIDToAssetPath(guids[i]);
                var resPath = originalPath.Substring(prePathLength);
                var path = resPath.Split('.');
                if(path.Length == 1) 
                {
                    continue;//文件夹名跳过
                }
                EditorUtility.DisplayProgressBar("ResourcesConfigBuilding",originalPath,(float)i/guids.Length);
                resConfig.AddPair(AssetDatabase.LoadAssetAtPath<Object>(originalPath).name, path[0]);
            }
            
            EditorUtility.ClearProgressBar();
            CreateResConfig(resConfig);
            LogTool.ZToolKitLog("Resources","资源路径配置完成");
        }

        private static void CreateResConfig(ResCatalog data)
        {
            var dataPath = Path.Combine(Application.streamingAssetsPath, ResTool.ResConfig);
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