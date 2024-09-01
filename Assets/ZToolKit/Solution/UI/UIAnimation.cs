using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace ZToolKit
{
    public enum UIAnimType
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
    
    public class UIAnimation
    {
        private Transform mRoot;
        private CanvasGroup mCg;

        public UIAnimation(Transform root)
        {
            mRoot = root;
            mCg = root.TryGetComponent<CanvasGroup>(out var cg) ? cg : root.gameObject.AddComponent<CanvasGroup>();
        }

        public void AnimOnOpen(UIAnimType type)
        {
            if(type == UIAnimType.None)
                return;
            
            mRoot.DOKill();
            mCg.DOKill();

            mRoot.localScale = Vector3.one * .5f;
            mCg.alpha = 0;

            mRoot.DOScale(Vector3.one, .3f);
            DOTween.To(() => mCg.alpha, x => mCg.alpha = x, 1, .3f);
        }

        public void AnimOnHide(UIAnimType type, Action onCompleteAct)
        {
            if(type == UIAnimType.None)
                return;
            
            mRoot.DOKill();
            mCg.DOKill();
            
            mRoot.DOScale(Vector3.one * .5f, .3f);
            DOTween.To(() => mCg.alpha, x => mCg.alpha = x, 0, .3f)
                .OnComplete(() => onCompleteAct?.Invoke());
        }

        private void PopMove(Vector2 direction)
        {
            
        }
    }
}