using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ZToolKit
{
    [RequireComponent(typeof(Image))]
    public class CustomToggleChangeImg : CustomToggle
    {
        public Sprite onSprite;
        public Sprite offSprite;

        private Image mTargetImg;
        
        protected override void Awake()
        {
            base.Awake();

            mTargetImg = transform.GetComponent<Image>();
            mTargetImg.sprite = isOn ? onSprite : offSprite;
            
            onValueChanged.AddListener(on =>
            {
                mTargetImg.sprite = on ? onSprite : offSprite;
            });
        }
    }
    
#if UNITY_EDITOR

    [CanEditMultipleObjects]
    [CustomEditor(typeof(CustomToggleChangeImg))]
    public class CustomToggleChangeImgEditor : UnityEditor.UI.ToggleEditor
    {
        public override void OnInspectorGUI()
        {
            var tgl = (CustomToggleChangeImg) target;
            tgl.onSprite = (Sprite)EditorGUILayout.ObjectField("OnSprite", tgl.onSprite, typeof(Sprite), allowSceneObjects: false);
            tgl.offSprite = (Sprite)EditorGUILayout.ObjectField("OffSprite", tgl.offSprite, typeof(Sprite), allowSceneObjects: false);
            
            base.OnInspectorGUI();
        }
    }

#endif
}
