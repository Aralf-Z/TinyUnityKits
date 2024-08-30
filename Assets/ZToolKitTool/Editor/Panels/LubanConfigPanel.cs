/*

*/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Path = System.IO.Path;

namespace ZToolKit.Editor
{   
    public class LubanConfigPanel: PanelBase
    {
        public override int Priority => 101;
        public override string PanelName => "[编辑器] Luban设置";

        private string mLubanPath;
        private string mLubanDataPath;
        
        public override void Init()
        {
            mLubanPath = EditorPrefs.GetString(EditorPrefsKeys.LubanPath);
            mLubanDataPath = EditorPrefs.GetString(EditorPrefsKeys.LubanDataPath);
            
            if (mLubanPath == string.Empty)
            {
                //默认位置
                mLubanPath = Path.Combine(Directory.GetParent(Application.dataPath).FullName, "Config");
            }
            
            if (mLubanDataPath == string.Empty)
            {
                //默认位置
                mLubanDataPath = Path.Combine(Directory.GetParent(Application.dataPath).FullName, "Config");
            }
        }

        public override void DrawPanel(Rect windowRect)
        {
            using (var h = new GUILayout.HorizontalScope())
            {
                
            }
            
            GUILayout.Space(5);
            
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
                GUILayout.TextField(mLubanDataPath, GUILayout.Width(windowRect.width * 5 / 9));

                GUILayout.Space(5);

                if (GUILayout.Button("选择本地数据表", EditorStyles.miniButton, GUILayout.Width(100)))
                {
                    var path = EditorUtility.OpenFilePanel("路径", mLubanDataPath, "xlsx");

                    if (path != string.Empty)
                    {
                        mLubanDataPath = Path.Combine(path);
                        EditorPrefs.SetString(EditorPrefsKeys.LubanDataPath, mLubanDataPath);
                        
                        //todo 生成schema
                    }
                }

                GUILayout.Space(5);
                GUILayout.FlexibleSpace();
                GUI.backgroundColor = Color.green;

                if (GUILayout.Button("打开本地数据表", EditorStyles.miniButton, GUILayout.Width(100)))
                {
                    try
                    {
                        Process.Start(mLubanDataPath);
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
                if (GUILayout.Button("打开Luban文档", GUILayout.Width(120)))
                {
                    Application.OpenURL("https://luban.doc.code-philosophy.com/docs/manual/excel");
                }
                
                GUILayout.Space(5);
                
                if (GUILayout.Button(new GUIContent("下载安装.Net8(?)", "Luban需要安装.Net8才能运行") , GUILayout.Width(120)))
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
                
                GUILayout.Space(5);
                
                GUILayout.FlexibleSpace();
                GUI.backgroundColor = Color.green;
                
                if (GUILayout.Button("执行Luban", EditorStyles.miniButton, GUILayout.Width(100)))
                {
                    ExecuteBatFile();
                }

                GUI.backgroundColor = Color.white;
            }
        }
        
        private void ExecuteBatFile()
        {
            var workspace = "..";
            var lubanDll = @".\Luban\Luban.dll";
            var confRoot = Path.GetDirectoryName(mLubanDataPath);
            
            var arguments = $"{lubanDll} -t all -c cs-bin -d bin --conf {confRoot}\\luban.conf " +
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
            
            string fileContent = @"
            {
                ""groups"": [
                {""names"":[""c""], ""default"":true},
                {""names"":[""s""], ""default"":true},
                {""names"":[""e""], ""default"":true}
                ],
                ""schemaFiles"": [
                {""fileName"":""Defines"", ""type"":""""},
                {""fileName"":""tables@TableConfig.xlsx"", ""type"":""table""},
                {""fileName"":""beans@TableConfig.xlsx"", ""type"":""bean""},
                {""fileName"":""enums@TableConfig.xlsx"", ""type"":""enum""}
                ],
                ""dataDir"": """",
                ""targets"": [
                {""name"":""server"", ""manager"":""Tables"", ""groups"":[""s""], ""topModule"":""cfg""},
                {""name"":""client"", ""manager"":""Tables"", ""groups"":[""c""], ""topModule"":""cfg""},
                {""name"":""all"", ""manager"":""Tables"", ""groups"":[""c"",""s"",""e""], ""topModule"":""cfg""}
                ]
            }";
            
            var tempFilePath = Path.Combine(confRoot, "luban.conf");
            
            try
            {
                // 将内容写入临时文件
                File.WriteAllText(tempFilePath, fileContent);

                EditorUtility.DisplayProgressBar("创建依赖", "luban.conf", .5f);

                try
                {
                    using var process = Process.Start(processInfo);

                    if (process is null)
                    {
                        LogTool.ZToolKitLogError("Luban",$"Error: process Failed");
                    }
                    else
                    {
                        string output = process.StandardOutput.ReadToEnd();
                
                        EditorUtility.DisplayProgressBar("luban", "解析中", .8f);
                        process.WaitForExit();
                
                        AssetDatabase.Refresh();
                    
                        if (output.Contains("bye~"))
                        {
                            LogTool.ZToolKitLog("Luban", "Analysis Succeed");
                        }
                        else
                        {
                            LogTool.ZToolKitLogError("Luban", "Analysis Failed");
                        }
                    
                        Debug.Log(output);
                    }
                }
                catch (Exception ex)
                {
                    LogTool.ZToolKitLogError("Luban",$"Error: {ex.Message}");
                }
            }
            finally
            {
                if (File.Exists(tempFilePath))
                {
                    File.Delete(tempFilePath);
                }
                
                EditorUtility.ClearProgressBar();
            }
            
            
        }
    }
} 