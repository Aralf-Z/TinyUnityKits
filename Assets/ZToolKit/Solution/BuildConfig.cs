using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using YooAsset;

namespace ZToolKit
{
    [CreateAssetMenu(fileName = "BuildConfig", menuName = "ZTool-GameConfig/BuildConfig", order = 0)]
    public class BuildConfig : ScriptableObject
    {
        public bool isConsoleActive = true;
        
        public ResMode resMode = ResMode.ResourcesLoad;

        public EPlayMode playMode = EPlayMode.EditorSimulateMode;

        //todo 本地化设置：1字体，2是否自适应---通配设置

        public int SavesCountInAnArchive = 5;
        
        public SaveLocation saveLocation = SaveLocation.Assets;

        public SaveType saveType = SaveType.Json;
    }
    
    public static class GameConfig
    {
        public static bool isConsoleActive { get; }
        
        public static ResMode ResMode { get; }

        public static EPlayMode PlayMode { get; }
        
        public static SaveLocation SaveLocation { get; }
        
        public static SaveType SaveType { get; }
        
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
                if (cfg.playMode == EPlayMode.WebPlayMode)
                {
                    EditorGUILayout.HelpBox("YooAsset的Web模式只支持异步加载，目前并未为此做同步适配。" +
                                            "以下模块在YooAsset的web模式下无法使用，web端使用以下模块推荐使用ResourcesLoad:\n" +
                                            "LocalizationImage, UITool, AudTool, SingletonDontDestroy", MessageType.Warning);
                }
            }
            
            //SaveTool
            EditorGUILayout.Space(5);
            EditorGUILayout.LabelField("Save", titleFont);
            var countProperty = serializedObject.FindProperty("SavesCountInAnArchive");
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
