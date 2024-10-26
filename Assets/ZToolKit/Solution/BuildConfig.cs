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
        public ResMode resMode = ResMode.ResourcesLoad;

        public EPlayMode playMode = EPlayMode.EditorSimulateMode;
    }
    
    public static class GameConfig
    {
        public static ResMode ResMode { get; }

        public static EPlayMode PlayMode { get; }
        
        static GameConfig()
        {
            var buildConfig = Resources.Load<BuildConfig>("Config/BuildConfig");
            
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

            if (cfg.resMode == ResMode.ResourcesLoad)
            {
                var tar = new SerializedObject(target);
                var propertyScr = tar.FindProperty("m_Script");
                var property = tar.FindProperty("resMode");
                
                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.PropertyField(propertyScr, false);
                EditorGUI.EndDisabledGroup();
                EditorGUILayout.PropertyField(property, false);

                tar.ApplyModifiedProperties();
            }
            else if(cfg.resMode == ResMode.YooAsset)
            {
                base.OnInspectorGUI();
            }
        }
    }
    
#endif
}
