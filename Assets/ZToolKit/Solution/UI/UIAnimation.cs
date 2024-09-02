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
        private UIScreen mUI;
        private RectTransform mRoot;
        private CanvasGroup mCg;

        private const float kPopOutTime = .3f;

        private const float kPopMoveTime = .3f;
        
        public UIAnimation(UIScreen ui)
        {
            mUI = ui;
            mRoot = mUI.animRoot;
            mCg = mUI.TryGetComponent<CanvasGroup>(out var cg) ? cg : ui.gameObject.AddComponent<CanvasGroup>();
        }

        public void AnimOnOpen(UIAnimType type)
        {
            switch (type)
            {
                case UIAnimType.PopOut : 
                    PopOutOnOpen(); 
                    break;
                case UIAnimType.PopMoveLeft:
                case UIAnimType.PopMoveRight:
                case UIAnimType.PopMoveUp:
                case UIAnimType.PopMoveDown:
                    PopMoveOnOpen(GetDir(type));
                    break;
                default:
                    break;
            }
        }

        public void AnimOnHide(UIAnimType type)
        {
            switch (type)
            {
                case UIAnimType.PopOut: 
                    PopOutOnHide(); 
                    break;
                case UIAnimType.PopMoveLeft:
                case UIAnimType.PopMoveRight:
                case UIAnimType.PopMoveUp:
                case UIAnimType.PopMoveDown:
                    PopMoveOnHide(GetDir(type));
                    break;
                default:
                    mUI.gameObject.SetActive(false);
                    break;
            }
        }

        private void PopOutOnOpen()
        {
            mRoot.DOKill();

            mRoot.localScale = Vector3.one * .5f;
            mRoot.anchoredPosition = Vector2.zero;
            
            mRoot.DOScale(Vector3.one, kPopOutTime);
            
            CgOnOpen(kPopOutTime);
        }

        private void PopOutOnHide()
        {
            mRoot.DOKill();
            mRoot.DOScale(Vector3.one * .5f, kPopOutTime)
                .OnComplete(() => mUI.gameObject.SetActive(false));
            
            CgOnHide(kPopOutTime);
        }
        
        private void PopMoveOnOpen(Vector2 dir)
        {
            var oriPos = -dir * new Vector2(960, 540);
            var tarPos = Vector2.zero;
            
            mRoot.DOKill();
            mRoot.anchoredPosition = oriPos;
            mRoot.localScale = Vector3.one;
            
            DOTween.To(() => mRoot.anchoredPosition, x => mRoot.anchoredPosition = x, tarPos, kPopMoveTime);
            
            CgOnOpen(kPopMoveTime);
        }

        private void PopMoveOnHide(Vector2 dir)
        {
            var oriPos = Vector2.zero;
            var tarPos = dir * new Vector2(960, 540);
            
            mRoot.DOKill();
            mRoot.anchoredPosition = oriPos;
            
            DOTween.To(() => mRoot.anchoredPosition, x => mRoot.anchoredPosition = x, tarPos, kPopMoveTime)
                .OnComplete(() =>mUI.gameObject.SetActive(false));
            
            CgOnHide(kPopMoveTime);
        }
        
        private void CgOnOpen(float animTime)
        {
            mCg.DOKill();
            mCg.alpha = 0;
            DOTween.To(() => mCg.alpha, x => mCg.alpha = x, 1, animTime);
        }
        
        private void CgOnHide(float animTime)
        {
            mCg.DOKill();
            DOTween.To(() => mCg.alpha, x => mCg.alpha = x, 0, animTime);
        }

        private Vector2 GetDir(UIAnimType type)
        {
            var dirX = type == UIAnimType.PopMoveLeft ? -1 : 0
                + type == UIAnimType.PopMoveRight ? 1 : 0;
            var dirY = type == UIAnimType.PopMoveDown ? -1 : 0
                + type == UIAnimType.PopMoveUp ? 1 : 0;

            return new Vector2(dirX, dirY);
        }
    }
}