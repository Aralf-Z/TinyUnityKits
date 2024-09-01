using System;
using UnityEngine;

namespace ZToolKit
{
    public abstract class UIScreen : MonoBehaviour
    {
        [Header("UIScreen")]
        [Tooltip("弹窗一般会点击空白处会关闭，所以空白背景可以添加canClick组件。但管理器并不会自动搜寻该组件，需要手动拖拽")]
        public CanClick[] hideCpts;

        [Tooltip("UI的打开关闭动画形式")]
        public UIAnimType uiAnimType;

        [Tooltip(@"弹窗一般有一个蒙版背景，所以动画并非整个UI, 需要一个根对象，如果为空，那么管理器会在UI中自动搜寻""Frame""作为根对象")]
        public Transform animRoot;
        
        protected virtual string SfxOnOpen => string.Empty;
        
        protected virtual string SfxOnHide => string.Empty;

        private UIAnimation mUIAnim;
        
        public UIScreen Init()
        {
            OnInit();

            if (!animRoot)
            {
                animRoot = transform.Find("Frame");
            }
            
            foreach (var cpt in hideCpts)
            {
                cpt.onClickAct += HideSelf;
            }

            mUIAnim = new UIAnimation(animRoot);
            
            return this;
        }

        public void Open(object data)
        {
            AudioTool.PlaySfx(SfxOnOpen);
            mUIAnim.AnimOnOpen(uiAnimType);
            gameObject.SetActive(true);
            OnOpen(data);
        }

        public void Hide()
        {
            AudioTool.PlaySfx(SfxOnHide);
            mUIAnim.AnimOnHide(uiAnimType, () =>gameObject.SetActive(false));
            OnHide();
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