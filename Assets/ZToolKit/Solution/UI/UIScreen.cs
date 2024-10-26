using System;
using UnityEngine;

namespace ZToolKit
{
    public abstract class UIScreen : MonoBehaviour
    {
        [Header("UIScreen")]
        [Tooltip("弹窗一般会点击空白处会关闭，所以空白背景可以添加canClick组件。")]
        public CanClick[] hideCpts;
        [Tooltip("UI的打开动画形式")]
        public UIAnimType animOnOpen = UIAnimType.None;
        [Tooltip("UI的关闭动画形式")]
        public UIAnimType animOnHide = UIAnimType.None;
        [Tooltip("动画根对象")]
        public RectTransform animRoot;

        protected virtual string SfxOnOpen => string.Empty;
        
        protected virtual string SfxOnHide => string.Empty;

        private UIAnimation mUIAnim;
        
        public UIScreen Init()
        {
            OnInit();

            foreach (var cpt in hideCpts)
            {
                cpt.onClickAct += HideSelf;
            }

            mUIAnim = new UIAnimation(this);
            
            return this;
        }

        public void Open(object data)
        {
            if (SfxOnOpen != string.Empty)
            {
                AudTool.PlaySfx(SfxOnOpen);
            }
            OnOpen(data);
            mUIAnim.AnimOnOpen();
        }

        public void Hide()
        {
            if (SfxOnOpen != string.Empty)
            {
                AudTool.PlaySfx(SfxOnHide);
            }
            OnHide();
            mUIAnim.AnimOnHide();
        }
        
        protected void HideSelf()
        {
            UIMgr.HideUIScreen(this);
        }
        
        /// <summary>
        /// 在Awake之后，Start之前调用
        /// </summary>
        protected abstract void OnInit();
        protected abstract void OnOpen(object data);
        protected abstract void OnHide();
    }
}