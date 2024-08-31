

using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace ZToolKit.Editor
{
    // public class GameSaveConfigPanel : PanelBase
    // {
    //     public override int Priority => 100;
    //     public override string PanelName => "[编辑器] 存档设置";
    //
    //     private bool mCanSaveInEditor;
    //     private bool mLoadSaveInEditor;
    //     private string mSaveFolder;
    //
    //     private static Dictionary<SaveFolder, string> SaveFolderDic = new()
    //     {
    //         [SaveFolder.Device] = "存储在设备",
    //         [SaveFolder.Game] = "存储在文件夹",
    //     };
    //     
    //     public override void Init()
    //     {
    //         mCanSaveInEditor = EditorPrefs.GetBool(EditorPrefsKeys.CanSaveInEditor);
    //         mLoadSaveInEditor = EditorPrefs.GetBool(EditorPrefsKeys.LoadSaveInEditor);
    //         //mSaveFolder = GameSave.SaveFolderDic[ResTool.Load<GameConfig>(nameof(GameConfig)).saveFolder];
    //     }
    //
    //     public override void DrawPanel(Rect windowRect)
    //     {
    //         mCanSaveInEditor = EditorGUILayout.Toggle("编辑器中保留存档", mCanSaveInEditor);
    //         EditorPrefs.SetBool(EditorPrefsKeys.CanSaveInEditor, mCanSaveInEditor);
    //         mLoadSaveInEditor = EditorGUILayout.Toggle("编辑器中读取存档", mLoadSaveInEditor);
    //         EditorPrefs.SetBool(EditorPrefsKeys.LoadSaveInEditor, mLoadSaveInEditor);
    //
    //         if (GUILayout.Button("打开存档位置", EditorStyles.miniButton, GUILayout.Width(200)))
    //         {
    //             if (!File.Exists(mSaveFolder))
    //                 Directory.CreateDirectory(mSaveFolder);
    //             Process.Start(mSaveFolder);
    //         }
    //     }
    // }
}
