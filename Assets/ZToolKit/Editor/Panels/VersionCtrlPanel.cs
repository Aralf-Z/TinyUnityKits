using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;

namespace ZToolKit.Editor
{
    public class VersionCtrlPanel : PanelBase
    {
        public override int Priority => 5;
        public override string PanelName => "[编辑器] 版本控制";

        public override void Init()
        {
            
        }

        public override void DrawPanel(Rect windowRect)
        {
            using (var h = new GUILayout.HorizontalScope())
            {
                GUILayout.FlexibleSpace();
                GUI.backgroundColor = Color.green;

                if (GUILayout.Button("打开SourceTree", EditorStyles.miniButton, GUILayout.Width(150)))
                {
                    Process.Start("C:\\Users\\Ein\\AppData\\Local\\SourceTree\\SourceTree.exe");
                }

                GUI.backgroundColor = Color.white;
            }
        }
    }
}
