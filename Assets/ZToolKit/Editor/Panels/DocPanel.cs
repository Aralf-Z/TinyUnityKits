using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZToolKit.Editor
{
    public class DocPanel : PanelBase
    {
        public override int Priority => 2;
        public override string PanelName => "[编辑器] 文档";
        
        public override void Init()
        {
            
        }

        public override void DrawPanel(Rect windowRect)
        {
            using (var h = new GUILayout.HorizontalScope())
            {
                if (GUILayout.Button("2021.3 Doc", GUILayout.Width(100)))
                {
                    Application.OpenURL("https://docs.unity.cn/2021.3/Documentation/ScriptReference/index.html");
                }
                
                GUILayout.Space(5);
                
                if (GUILayout.Button("DOTween Doc", GUILayout.Width(100)))
                {
                    Application.OpenURL("http://dotween.demigiant.com/documentation.php#creatingTweener");
                }
                
                GUILayout.Space(5);
                
                if (GUILayout.Button("Luban Doc", GUILayout.Width(100)))
                {
                    Application.OpenURL("https://luban.doc.code-philosophy.com/docs/manual/excel");
                }
                
                GUILayout.Space(5);
                
                if (GUILayout.Button("UniTask Doc", GUILayout.Width(100)))
                {
                    Application.OpenURL("https://github.com/Cysharp/UniTask?tab=readme-ov-file#getting-started");
                }

                GUILayout.Space(5);
            }
        }
    }
}
