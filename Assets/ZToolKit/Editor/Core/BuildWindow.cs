using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ZToolKit.Editor
{
    public class BuildWindow : EditorWindow
    {
        private BuildConfig mBuildConfig;
        private UnityEditor.Editor mConfigEditor;
        
        [MenuItem("Tools/BuildWindow #B", false, 1)]
        private static void OpenSelf()
        {
            var w = GetWindow<BuildWindow>("BuildWindow", true, WindowsDefine.DockedWindowTypes);
            w.maxSize = new Vector2(900, 900);
            w.minSize = new Vector2(630, 450);
        }
        
        private void OnEnable()
        {
            mBuildConfig = AssetDatabase.LoadAssetAtPath<BuildConfig>("Assets/Resources/Config/BuildConfig.asset");
            mConfigEditor = UnityEditor.Editor.CreateEditor(mBuildConfig);
        }
        
        private void OnGUI()
        {
            var titleFont = new GUIStyle {fontSize = 15, normal = new GUIStyleState{textColor = Color.cyan}};
            
            if (mBuildConfig)
            {
                GUILayout.Label("[BuildConfig]", titleFont);
                mConfigEditor.OnInspectorGUI();
            }
        }
    }
}
