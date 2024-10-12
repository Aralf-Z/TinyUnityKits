using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ZToolKit
{
    [DisallowMultipleComponent]
    public abstract class LocalizationBase : MonoBehaviour
    {
        public Graphic target;
        
        public string key;

        protected RectTransform mRectTransform;
        
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

        protected abstract void OnLanguageChange();

#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            target = transform.GetComponent<Graphic>();
            mRectTransform = (RectTransform) transform;
        }
#endif
    }
}
