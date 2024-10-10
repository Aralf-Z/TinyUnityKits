using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ZToolKit
{
    [DisallowMultipleComponent]
    public class Localization : MonoBehaviour
    {
        [Tooltip("文本组件")]
        public Text text;

        [Tooltip("语言key")]
        public string key;

        private void Awake()
        {
            OnLanguageChange();
            L10nTool.Event_OnChangeLanguage += OnLanguageChange;
        }

        private void OnEnable()
        {
            OnLanguageChange();
            L10nTool.Event_OnChangeLanguage += OnLanguageChange;
        }

        private void OnDisable()
        {
            L10nTool.Event_OnChangeLanguage -= OnLanguageChange;
        }

        private void OnDestroy()
        {
            L10nTool.Event_OnChangeLanguage -= OnLanguageChange;
        }

        private void OnLanguageChange()
        {
            text.text = L10nTool.GetUIStr(transform, key);
        }

#if UNITY_EDITOR
        
        private void OnValidate()
        {
            text = transform.GetComponent<Text>();
        }
        
#endif
    }
}
