/*

*/

using System;
using System.IO;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ZToolKit.Editor
{
    public class ResourcesConfigPanel : PanelBase
    {
        public override int Priority => 6;
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
            try
            {
                var guids = AssetDatabase.FindAssets("", new string[] {"Assets/Resources"});
                var resConfig = new ResCatalog();
                int prePathLength = "Assets/Resources/".Length;
                for (int i = 0; i < guids.Length; ++i)
                {
                    var originalPath = AssetDatabase.GUIDToAssetPath(guids[i]);
                    var resPath = originalPath.Substring(prePathLength);
                    var path = resPath.Split('.');
                    if (path.Length == 1)
                    {
                        continue; //文件夹名跳过
                    }

                    var name = AssetDatabase.LoadAssetAtPath<Object>(originalPath).name;
                    LogTool.ToolInfo("ResTool", $"资源录入：{name}");
                    EditorUtility.DisplayProgressBar("ResourcesConfigBuilding", originalPath, (float) i / guids.Length);
                    resConfig.AddPair(name, path[0]);
                }
                
                CreateResConfig(resConfig);
                LogTool.ToolInfo("ResTool", "资源路径配置完成");
            }
            catch (Exception e)
            {
                LogTool.ToolError("ResTool", $"资源路径配置失败:{e.Message}");
                throw;
            }
            finally
            {
                EditorUtility.ClearProgressBar();
            }
        }

        private static void CreateResConfig(ResCatalog data)
        {
            var dataPath = Path.Combine(Application.streamingAssetsPath, ResTool.ResCatalog);
            var folders = ResTool.ResCatalog.Split("/");
            var tempPath = Application.streamingAssetsPath;

            if (!Directory.Exists(Application.streamingAssetsPath))
            {
                Directory.CreateDirectory(Application.streamingAssetsPath);
                AssetDatabase.Refresh();
            }

            for(int i = 0; i < folders.Length - 1; i++)
            {
                var fold = folders[i];
                
                tempPath = Path.Combine(tempPath, fold);
                
                if (!Directory.Exists(tempPath))
                {
                    Directory.CreateDirectory(tempPath);
                    AssetDatabase.Refresh();
                }
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