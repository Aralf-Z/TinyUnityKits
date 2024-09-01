using System;
using UnityEngine;

namespace ZToolKit
{
    public enum UIAnim
    {
        [Tooltip("无")]
        None,
        [Tooltip("弹窗,弹出形式为从中间弹出放大, 隐藏形式为往中间缩小")]
        PopOut,
        [Tooltip("弹窗,弹出形式为从左向右, 隐藏形式为从右向左")]
        PopMoveLeft,
        [Tooltip("弹窗,弹出形式为从右向左, 隐藏表现为从左向右")]
        PopMoveRight,
        [Tooltip("弹窗,弹出形式为从下向上, 隐藏表现为从上向下")]
        PopMoveUp,
        [Tooltip("弹窗,弹出形式为从上向下, 隐藏表现为从下向上")]
        PopMoveDown,
    }

    public abstract class UIScreen : MonoBehaviour
    {
        [Header("UIScreen")]
        [Tooltip("弹窗一般会点击空白处会关闭，所以空白背景可以添加canClick组件。但管理器并不会自动搜寻该组件，需要手动拖拽")]
        public CanClick[] hideCpts;

        [Tooltip("UI的打开关闭动画形式")]
        public UIAnim uiAnim;

        [Tooltip(@"弹窗一般有一个蒙版背景，所以动画并非整个UI, 需要一个根对象，如果为空，那么管理器会在UI中自动搜寻""Frame""作为根对象")]
        public Transform animRoot;
        
        protected virtual string SfxOnOpen => string.Empty;
        
        protected virtual string SfxOnHide => string.Empty;

        public UIScreen Init()
        {
            OnInit();

            foreach (var cpt in hideCpts)
            {
                cpt.onClickAct += HideSelf;
            }

            return this;
        }

        public void Open(object data)
        {
            AudioTool.PlaySfx(SfxOnOpen);
            gameObject.SetActive(true);
            OnOpen(data);
        }

        public void Hide()
        {
            AudioTool.PlaySfx(SfxOnHide);
            OnHide();
            gameObject.SetActive(false);
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