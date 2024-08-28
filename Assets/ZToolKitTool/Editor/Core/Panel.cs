/*

*/

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace ZToolKit.Editor
{
    public class PanelWindow : EditorWindow
    {
        private PanelBase[] mPanels;
        private Vector2 mScrollBtn;

        [MenuItem("Tools/ZToolKit #Z", false, 1)]
        private static void OpenSelf()
        {
            var w = GetWindow<PanelWindow>("ConfigPanel");
            w.maxSize = new Vector2(900, 900);
            w.minSize = new Vector2(630, 450);
        }

        private void OnEnable()
        {
            var panels = new List<PanelBase>();
            var ts = typeof(PanelBase).Assembly
                .GetTypes()
                .Where(t => typeof(PanelBase).IsAssignableFrom(t) && !t.IsAbstract);

            foreach (var t in ts)
            {
                if (Activator.CreateInstance(t) is not PanelBase panel) continue;
                panel.Init();
                panels.Add(panel);
            }

            mPanels = new PanelBase[panels.Count];
            mPanels = panels.OrderBy(p => p.Priority).ToArray();
        }

        private void OnGUI()
        {
            mScrollBtn = GUILayout.BeginScrollView(mScrollBtn, GUILayout.Width(position.width), GUILayout.Height(position.height));
            
            var titleFont = new GUIStyle {fontSize = 15, normal = new GUIStyleState{textColor = Color.cyan}};
            var curWinRect = position;

            foreach (var p in mPanels)
            {
                using (new GUILayout.VerticalScope("HelpBox"))
                {
                    GUILayout.Space(5);
                    GUILayout.Label(p.PanelName, titleFont);
                    p.DrawPanel(curWinRect);
                    GUILayout.Space(5);
                }
                GUILayout.Space(10);
            }
            
            GUILayout.EndScrollView();
        }
    }
}