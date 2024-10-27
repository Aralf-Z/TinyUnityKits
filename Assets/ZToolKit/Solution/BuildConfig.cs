using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using YooAsset;

namespace ZToolKit
{
    public enum ResMode
    {
        ResourcesLoad,
        YooAsset,
    }
    
    [CreateAssetMenu(fileName = "BuildConfig", menuName = "ZTool-GameConfig/BuildConfig", order = 0)]
    public class BuildConfig : ScriptableObject
    {
        public bool isConsoleActive = true;
        
        public ResMode resMode = ResMode.ResourcesLoad;

        public EPlayMode playMode = EPlayMode.EditorSimulateMode;

        //todo 本地化设置：1字体，2是否自适应---通配设置
        
        public string Version = "ver. 1.0.0";

        private int version_X_0_0;
        
        private int version_0_Y_0;
        
        private int version_0_0_Z;
    }
    
    public static class GameConfig
    {
        public static bool isConsoleActive { get; }
        
        public static ResMode ResMode { get; }

        public static EPlayMode PlayMode { get; }
        
        static GameConfig()
        {
            var buildConfig = Resources.Load<BuildConfig>("Config/BuildConfig");

            isConsoleActive = buildConfig.isConsoleActive;
            ResMode = buildConfig.resMode;
            PlayMode = buildConfig.playMode;
        }
    }

#if UNITY_EDITOR
    
    [CustomEditor(typeof(BuildConfig))]
    public class BuildConfigEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var cfg = (BuildConfig) target;
            var titleFont = new GUIStyle {fontSize = 15, normal = new GUIStyleState{textColor = Color.cyan}};
            
            //m_Script
            var propertyScr = serializedObject.FindProperty("m_Script");
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.PropertyField(propertyScr, false);
            EditorGUI.EndDisabledGroup();
            
            //console
            EditorGUILayout.Space(5);
            EditorGUILayout.LabelField("Console", titleFont);
            var consoleProperty= serializedObject.FindProperty("isConsoleActive");
            EditorGUILayout.PropertyField(consoleProperty, false);
            
            //resTool
            EditorGUILayout.Space(5);
            EditorGUILayout.LabelField("ResTool", titleFont);
            var resProperty = serializedObject.FindProperty("resMode");
            EditorGUILayout.PropertyField(resProperty, false);
            
            if(cfg.resMode == ResMode.YooAsset)
            {
                var playProperty = serializedObject.FindProperty("playMode");
                EditorGUILayout.PropertyField(playProperty , false);
            }
            
            serializedObject.ApplyModifiedProperties();
        }
    }
    
#endif
}
