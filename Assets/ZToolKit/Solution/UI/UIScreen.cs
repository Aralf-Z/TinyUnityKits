using System;
using UnityEngine;

namespace ZToolKit
{
    public abstract class UIScreen : MonoBehaviour
    {
        protected virtual string SfxOnOpen => string.Empty;
        
        protected virtual string SfxOnHide => string.Empty;
        
        private void Awake()
        {
            Init();
        }

        private void Init()
        {
            OnInit();
        }

        public void Open(object data)
        {
            gameObject.SetActive(true);
            OnOpen(data);
        }

        public void Hide()
        {
            OnHide();
            gameObject.SetActive(false);
        }
        
        protected void HideSelf()
        {
            UIMgr.HideUIScreen(this);
        }
        
        protected abstract void OnInit();
        protected abstract void OnOpen(object data);
        protected abstract void OnHide();
    }
}