/*

*/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
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
                mLubanDataPath = Path.Combine(Directory.GetParent(Application.dataPath).FullName, "Config", "Data");
            }
        }

        public override void DrawPanel(Rect windowRect)
        {
            using (var h = new GUILayout.HorizontalScope())
            {
                GUILayout.TextField(mLubanPath, GUILayout.Width(windowRect.width * 5 / 9));
                
                GUILayout.Space(5);
                
                GUILayout.FlexibleSpace();
                
                if (GUILayout.Button("选择Luban位置", EditorStyles.miniButton, GUILayout.Width(100)))
                {
                    var path = EditorUtility.OpenFolderPanel("路径", mLubanPath, "");
                    mLubanPath = path == string.Empty ? mLubanPath : path;
                    EditorPrefs.SetString(EditorPrefsKeys.LubanPath, mLubanPath);
                }
                
                GUILayout.Space(5);
                
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
            }

            using (var h = new GUILayout.HorizontalScope())
            {
                GUILayout.TextField(mLubanDataPath, GUILayout.Width(windowRect.width * 5 / 9));
                
                GUILayout.Space(5);
                
                GUILayout.FlexibleSpace();
                
                if (GUILayout.Button("选择数据表位置", EditorStyles.miniButton, GUILayout.Width(100)))
                {
                    var path = EditorUtility.OpenFolderPanel("路径", mLubanDataPath, "");
                    mLubanDataPath = path == string.Empty ? mLubanDataPath : path;
                    EditorPrefs.SetString(EditorPrefsKeys.LubanDataPath, mLubanDataPath);
                }
                
                GUILayout.Space(5);
                
                if (GUILayout.Button("打开数据表位置", EditorStyles.miniButton, GUILayout.Width(100)))
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
            }
            
            // if (GUILayout.Button("执行Luban", EditorStyles.miniButton))
            // {
            //     ExecuteBatFile(Path.Combine(mLubanPath, "gen.bat"));
            // } 
        }
        
        private void ExecuteBatFile(string filePath)
        {
            try
            {
                var process = new Process
                {
                    StartInfo =
                    {
                        FileName = filePath, 
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false, 
                        CreateNoWindow = true,
                    }
                };
                
                var outputStr = new List<string>(10);
                process.OutputDataReceived += (sender, args) => outputStr.Add(args.Data);
                process.ErrorDataReceived += (sender, args) => outputStr.Add(args.Data);
                
                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                process.WaitForExit();
                
                LogTool.ZToolKitLog("Luban","Batch file executed successfully.");
                
                foreach (var str in outputStr.Where(str => str != string.Empty))
                {
                    LogTool.ZToolKitLog("Luban", str);
                }
            }
            catch (System.Exception e)
            {
                LogTool.EditorLogError( "Failed to execute batch file: " + e.Message);
            }
        }
    }
} 