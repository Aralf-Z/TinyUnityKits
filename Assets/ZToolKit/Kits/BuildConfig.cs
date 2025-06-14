using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace ZToolKit
{
    [CreateAssetMenu(fileName = "BuildConfig", menuName = "ZTool-GameConfig/BuildConfig", order = 0)]
    public class BuildConfig : ScriptableObject
    {
        public bool isConsoleActive = true;

        //todo 本地化设置：1字体，2是否自适应---通配设置

        public int saveCountInAnArchive = 25;
        
        public SaveLocation saveLocation = SaveLocation.Assets;

        public SaveType saveType = SaveType.Json;
    }
    
    public static class GameConfig
    {
        public static bool IsConsoleActive { get; }
        
        public static int SaveCountInAnArchive { get; }
        
        public static SaveLocation SaveLocation { get; }
        
        public static SaveType SaveType { get; }
        
        static GameConfig()
        {
            var buildConfig = Resources.Load<BuildConfig>("Config/BuildConfig");

            IsConsoleActive = buildConfig.isConsoleActive;
            SaveCountInAnArchive = buildConfig.saveCountInAnArchive;
            SaveLocation = buildConfig.saveLocation;
            SaveType = buildConfig.saveType;
        }
    }

#if UNITY_EDITOR
    
    [CustomEditor(typeof(BuildConfig))]
    public class BuildConfigEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var cfg = (BuildConfig) target;
            var titleFont = new GUIStyle {fontSize = 13, normal = new GUIStyleState{textColor = Color.white}};

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
            
            //SaveTool
            EditorGUILayout.Space(5);
            EditorGUILayout.LabelField("Save", titleFont);
            var countProperty = serializedObject.FindProperty("saveCountInAnArchive");
            EditorGUILayout.PropertyField(countProperty, false); 
            var locationProperty = serializedObject.FindProperty("saveLocation");
            EditorGUILayout.PropertyField(locationProperty, false);
            var typeProperty = serializedObject.FindProperty("saveType");
            EditorGUILayout.PropertyField(typeProperty, false);
            if (cfg.saveLocation == SaveLocation.Assets)
            {
                EditorGUILayout.HelpBox("仅支持windows便携包，用以分发测试使用", MessageType.Warning);
            }
            if (cfg.saveLocation is SaveLocation.Assets or SaveLocation.Persistent)
            {
                //todo 
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
    
#endif
}
