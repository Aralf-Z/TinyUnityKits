/*

*/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Path = System.IO.Path;

namespace ZToolKit.Editor
{   
    public class LubanConfigPanel: PanelBase
    {
        private const string kJson = "json";
        private const string kByte = "bin";

        private string Schema 
        {
            get
            {
                var fileName = TableFile;

                return   $@"
                {{
                ""groups"": [
                    {{""names"":[""c""], ""default"":true}},
                    {{""names"":[""s""], ""default"":true}},
                    {{""names"":[""e""], ""default"":true}}
                    ],
                ""schemaFiles"": [
                    {{""fileName"":""Defines"", ""type"":""""}},
                    {{""fileName"":""tables@{fileName}"", ""type"":""table""}},
                    {{""fileName"":""beans@{fileName}"", ""type"":""bean""}},
                    {{""fileName"":""enums@{fileName}"", ""type"":""enum""}}
                    ],
                ""dataDir"": """",
                ""targets"": [
                    {{""name"":""server"", ""manager"":""Tables"", ""groups"":[""s""], ""topModule"":""cfg""}},
                    {{""name"":""client"", ""manager"":""Tables"", ""groups"":[""c""], ""topModule"":""cfg""}},
                    {{""name"":""all"", ""manager"":""Tables"", ""groups"":[""c"",""s"",""e""], ""topModule"":""cfg""}}
                    ]
                 }}";
            }
        }
        
        private const string kBuiltinXml = @"
            <module name="""">
                <bean name=""vector2"" valueType=""1"" sep="","">
                    <var name=""x"" type=""float""/>
                    <var name=""y"" type=""float""/>
                </bean>
                <bean name=""vector3"" valueType=""1"" sep="","">
                    <var name=""x"" type=""float""/>
                    <var name=""y"" type=""float""/>
                    <var name=""z"" type=""float""/>
                </bean>
                <bean name=""vector4"" valueType=""1"" sep="","">
                    <var name=""x"" type=""float""/>
                    <var name=""y"" type=""float""/>
                    <var name=""z"" type=""float""/>
                    <var name=""w"" type=""float""/>
                </bean>
            </module>";
        //     <mapper target=""client"" codeTarget=""cs-bin"">
        // <option name=""type"" value=""UnityEngine.Vector2""/>
        // <option name=""constructor"" value=""ExternalTypeUtil.NewVector2""/>
        // </mapper> //类型映射
        
        
        public override int Priority => 101;
        public override string PanelName => "[编辑器] Luban设置";

        private string mLubanPath;
        private string mLubanConfigPath;
        private string mLubanConfigUrl;

        private readonly string[] mOutputTypes = {kJson, kByte};
        private int mOutputTypeIndex;
        private string TableFile => Path.GetFileName(mLubanConfigPath);

        public override void Init()
        {
            mLubanPath = EditorPrefs.GetString(EditorPrefsKeys.LubanPath);
            mLubanConfigPath = EditorPrefs.GetString(EditorPrefsKeys.LubanConfigPath);
            mLubanConfigUrl = EditorPrefs.GetString(EditorPrefsKeys.LubanConfigUrl);
            mOutputTypeIndex = EditorPrefs.GetInt(EditorPrefsKeys.LubanOutputType);
            
            if (mLubanPath == string.Empty)
            {
                mLubanPath = Path.Combine(Directory.GetParent(Application.dataPath).FullName, "Config");
            }
            
            if (mLubanConfigPath == string.Empty)
            {
                mLubanConfigPath = Path.Combine(Directory.GetParent(Application.dataPath).FullName, "Config");
            }
            
            if (mLubanConfigUrl == string.Empty)
            {
                mLubanConfigUrl = "https://kdocs.cn/l/cc1fUd19bWib";
            }
        }

        public override void DrawPanel(Rect windowRect)
        {
            var titleFont = new GUIStyle {fontSize = 13, normal = new GUIStyleState{textColor = Color.white}};
            using (new GUILayout.VerticalScope("HelpBox"))
            {
                Analysis(titleFont);
                GUILayout.Space(5);
            }
            
            using (new GUILayout.VerticalScope("HelpBox"))
            {
                ConfigSetting(titleFont, windowRect);
                GUILayout.Space(5);
            }
            
            using (new GUILayout.VerticalScope("HelpBox"))
            {
                Help(titleFont);
                GUILayout.Space(5);
            }
        }

        private void Analysis(GUIStyle titleFont)
        {
            GUILayout.Label("解析", titleFont);
            GUILayout.Space(3);
            using (var h = new GUILayout.HorizontalScope())
            {
                mOutputTypeIndex = EditorGUILayout.Popup("配置导出格式", mOutputTypeIndex, mOutputTypes, GUILayout.Width(200));
                EditorPrefs.SetInt(EditorPrefsKeys.LubanOutputType, mOutputTypeIndex);
                
                GUILayout.FlexibleSpace();
                
                GUI.backgroundColor = Color.green;
                
                if (GUILayout.Button("解析本地配置表", EditorStyles.miniButton, GUILayout.Width(100)))
                {
                    ExecuteBatFile();
                }

                GUI.backgroundColor = Color.white;
            }
        }

        private void ConfigSetting(GUIStyle titleFont, Rect windowRect)
        {
            GUILayout.Label("配置", titleFont);
            GUILayout.Space(3);
            
            using (var h = new GUILayout.HorizontalScope())
            {
                GUILayout.TextField(mLubanPath, GUILayout.Width(windowRect.width * 5 / 9));
                
                GUILayout.Space(5);
                
                if (GUILayout.Button("选择Luban位置", EditorStyles.miniButton, GUILayout.Width(100)))
                {
                    var path = EditorUtility.OpenFolderPanel("路径", mLubanPath, "");
                    if (path != string.Empty)
                    {
                        mLubanPath = Path.Combine(path);
                        EditorPrefs.SetString(EditorPrefsKeys.LubanPath, mLubanPath);
                    }
                }
                
                GUILayout.FlexibleSpace();
                GUILayout.Space(5);
                GUI.backgroundColor = Color.green;

                if (GUILayout.Button("打开Luban位置", EditorStyles.miniButton, GUILayout.Width(100)))
                {
                    try
                    {
                        Process.Start(mLubanPath);
                    }
                    catch (Exception e)
                    {
                        LogTool.EditorLogError(e);
                        throw;
                    }
                } 
                
                GUI.backgroundColor = Color.white;
            }

            GUILayout.Space(5);
            
            using (var h = new GUILayout.HorizontalScope())
            {
                GUILayout.TextField(mLubanConfigPath, GUILayout.Width(windowRect.width * 5 / 9));

                GUILayout.Space(5);

                if (GUILayout.Button("选择本地配置表", EditorStyles.miniButton, GUILayout.Width(100)))
                {
                    var path = EditorUtility.OpenFilePanel("路径", mLubanConfigPath, "xlsx");

                    if (path != string.Empty)
                    {
                        mLubanConfigPath = Path.Combine(path);
                        EditorPrefs.SetString(EditorPrefsKeys.LubanConfigPath, mLubanConfigPath);
                    }
                }

                GUILayout.Space(5);
                GUILayout.FlexibleSpace();
                GUI.backgroundColor = Color.green;

                if (GUILayout.Button("打开本地配置表", EditorStyles.miniButton, GUILayout.Width(100)))
                {
                    try
                    {
                        Process.Start(mLubanConfigPath);
                    }
                    catch (Exception e)
                    {
                        LogTool.EditorLogError(e);
                        throw;
                    }
                }

                GUI.backgroundColor = Color.white;
            }
            
            GUILayout.Space(5);
            
            using (var h = new GUILayout.HorizontalScope())
            {
                mLubanConfigUrl = GUILayout.TextField(mLubanConfigUrl, GUILayout.Width(windowRect.width * 5 / 9));
                
                GUILayout.Space(5);
                GUILayout.FlexibleSpace();
                GUI.backgroundColor = Color.green;

                if (GUILayout.Button("打开云端配置表", EditorStyles.miniButton, GUILayout.Width(100)))
                {
                    Application.OpenURL(mLubanConfigUrl);
                }

                GUI.backgroundColor = Color.white;
            }
        }

        private void Help(GUIStyle titleFont)
        {
            GUILayout.Label("帮助", titleFont);
            GUILayout.Space(3);
            
            using (var h = new GUILayout.HorizontalScope())
            {
                if (GUILayout.Button("打开本地简易说明", GUILayout.Width(120)))
                {
                    var path = Path.Combine(Application.dataPath, "ZToolKit/Editor/Panels/Luban简易说明.docx");
                    
                    Process.Start(new ProcessStartInfo()
                    {
                        FileName = path,
                        UseShellExecute = true
                    });
                }
                
                if (GUILayout.Button("打开Luban官方文档", GUILayout.Width(120)))
                {
                    Application.OpenURL("https://luban.doc.code-philosophy.com/docs/manual/excel");
                }

                GUILayout.Space(5);

                if (GUILayout.Button(new GUIContent("下载安装.net8(?)", "Luban需要安装.Net8才能运行"), GUILayout.Width(120)))
                {
                    Application.OpenURL("https://dotnet.microsoft.com/zh-cn/download/dotnet/8.0");
                }

                GUILayout.Space(5);

                if (GUILayout.Button("使用命令行", GUILayout.Width(120)))
                {
                    ProcessStartInfo processInfo = new ProcessStartInfo
                    {
                        FileName = "cmd.exe",
                        Arguments = "/k",
                        UseShellExecute = true
                    };

                    Process.Start(processInfo);
                }
            }
        }
        
        private void ExecuteBatFile()
        {
            var workspace = "..";
            var lubanDll = @".\Luban\Luban.dll";
            var confRoot = Path.GetDirectoryName(mLubanConfigPath);
            var outputType = mOutputTypes[mOutputTypeIndex];
            var arguments = $"{lubanDll} " +
                            $"-t all " +
                            $"-c cs{(outputType == kJson? "-simple":string.Empty)}-{outputType} " +
                            $"-d {outputType} " +
                            $"--conf {confRoot}\\luban.conf " +
                            $"-x outputDataDir={workspace}\\Assets\\StreamingAssets\\TableConfig " +
                            $"-x outputCodeDir={workspace}\\Assets\\TableConfig\\Scripts";
            
            // 创建一个新的进程信息对象
            var processInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = arguments,
                WorkingDirectory = mLubanPath,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = false 
            };
            
            var schemaFilePath = Path.Combine(confRoot, "luban.conf");
            var definesPath = Path.Combine(confRoot, "Defines");
            var definesFilePath = Path.Combine(confRoot, "Defines/builtin.xml");
            
            try
            {
                File.WriteAllText(schemaFilePath, Schema);
                EditorUtility.DisplayProgressBar("创建依赖", "luban.conf", .2f);
                if (!Directory.Exists(definesPath))
                {
                    Directory.CreateDirectory(definesPath);
                }
                File.WriteAllText(definesFilePath, kBuiltinXml);
                EditorUtility.DisplayProgressBar("创建依赖", "Defines/builtin.xml", .4f);
                
                try
                {
                    using var process = Process.Start(processInfo);

                    if (process is null)
                    {
                        LogTool.ZToolKitLogError("Luban",$"Error: process Failed");
                    }
                    else
                    {
                        string outputStr = process.StandardOutput.ReadToEnd();
                
                        EditorUtility.DisplayProgressBar("luban", "解析中", .6f);
                        
                        process.WaitForExit();
                        AssetDatabase.Refresh();
                        
                        if (outputStr.Contains("bye~"))
                        {
                            AnalysisConfigFile();
                            LogTool.ZToolKitLog("Luban", $"Analysis Succeed, OutPut Type: {outputType}");
                        }
                        else
                        {
                            LogTool.ZToolKitLogError("Luban", "Analysis Failed");
                        }
                    
                        Debug.Log(outputStr);
                    }
                }
                catch (Exception ex)
                {
                    LogTool.ZToolKitLogError("Luban",$"Error: {ex.Message}");
                }
            }
            finally
            {
                try
                {
                    if (File.Exists(schemaFilePath))
                    {
                        File.Delete(schemaFilePath);
                    }
                    
                    if (File.Exists(definesFilePath))
                    {
                        File.Delete(definesFilePath);
                    }
                    
                    if (Directory.Exists(definesPath))
                    {
                        Directory.Delete(definesPath);
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
                finally
                {
                    EditorUtility.ClearProgressBar();
                }
            }
        }

        private void AnalysisConfigFile()
        {
            try
            {
                var cfgFiles = new List<string>();
                var path = Path.Combine(Application.streamingAssetsPath, "TableConfig");
                var files = Directory.GetFiles(path);

                foreach (var file in files)
                {
                    if (file.EndsWith(".json") || file.EndsWith(".byte"))
                    {
                        var fileName = Path.GetFileNameWithoutExtension(file);
                        cfgFiles.Add(fileName);
                    }
                }
                
                File.WriteAllText(CfgTool.fileListPath, JsonConvert.SerializeObject(cfgFiles));
                AssetDatabase.Refresh();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                throw;
            }
        }
    }
} 