using System;
using UnityEngine;

namespace ZToolKit
{
    public abstract class UIScreen : MonoBehaviour
    {
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
            UIManager.HideUIScreen(this);
        }
        
        protected abstract void OnInit();
        protected abstract void OnOpen(object data);
        protected abstract void OnHide();
    }
}