using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace ZToolKit
{
    //[Flags]
    public enum UIAnimType
    {
        [Tooltip("无")]
        None          = 0b0,//0
        [Tooltip("弹窗,弹出形式为从中间弹出放大, 隐藏形式为往中间缩小")]
        PopOut        = 0b1,//1
        [Tooltip("弹窗,弹出形式为从左向右, 隐藏形式为从右向左")]
        PopMoveLeft   = 0b10,//2
        [Tooltip("弹窗,弹出形式为从右向左, 隐藏表现为从左向右")]
        PopMoveRight  = 0b100,//4
        [Tooltip("弹窗,弹出形式为从下向上, 隐藏表现为从上向下")]
        PopMoveUp     = 0b1000,//8
        [Tooltip("弹窗,弹出形式为从上向下, 隐藏表现为从下向上")]
        PopMoveDown   = 0b10000,//16
    }
    
    public class UIAnimation
    {
        private UIScreen mUI;
        private CanvasGroup mCg;

        private const float kPopOutTime = .3f;

        private const float kPopMoveTime = .3f;
        
        public UIAnimation(UIScreen ui)
        {
            mUI = ui;
            mCg = mUI.TryGetComponent<CanvasGroup>(out var cg) ? cg : ui.gameObject.AddComponent<CanvasGroup>();
        }

        public void AnimOnOpen()
        {
            var type = mUI.animOnOpen;
            
            mUI.gameObject.SetActive(true);
            mUI.animRoot.localScale = Vector3.one;
            mUI.animRoot.anchoredPosition = Vector2.zero;
            mCg.alpha = 1;

            switch (type)
            {
                case UIAnimType.PopOut : 
                    PopOutOnOpen(); 
                    break;
                case UIAnimType.PopMoveLeft:
                case UIAnimType.PopMoveRight:
                case UIAnimType.PopMoveUp:
                case UIAnimType.PopMoveDown:
                // case UIAnimType.PopMoveLeft | UIAnimType.PopMoveUp:
                // case UIAnimType.PopMoveLeft | UIAnimType.PopMoveDown:
                // case UIAnimType.PopMoveRight | UIAnimType.PopMoveUp:
                // case UIAnimType.PopMoveRight | UIAnimType.PopMoveDown:
                    PopMoveOnOpen(GetDir(type));
                    break;
                default:
                    break;
            }
        }

        public void AnimOnHide()
        {
            var type = mUI.animOnHide;
            
            switch (type)
            {
                case UIAnimType.PopOut: 
                    PopOutOnHide(); 
                    break;
                case UIAnimType.PopMoveLeft:
                case UIAnimType.PopMoveRight:
                case UIAnimType.PopMoveUp:
                case UIAnimType.PopMoveDown:
                // case UIAnimType.PopMoveLeft | UIAnimType.PopMoveUp:
                // case UIAnimType.PopMoveLeft | UIAnimType.PopMoveDown:
                // case UIAnimType.PopMoveRight | UIAnimType.PopMoveUp:
                // case UIAnimType.PopMoveRight | UIAnimType.PopMoveDown:
                PopMoveOnHide(GetDir(type));
                    break;
                default:
                    mUI.gameObject.SetActive(false);
                    break;
            }
        }

        private void PopOutOnOpen()
        {
            mUI.animRoot.DOKill();

            mUI.animRoot.localScale = Vector3.one * .5f;

            mUI.animRoot.DOScale(Vector3.one, kPopOutTime);
            
            CgOnOpen(kPopOutTime);
        }

        private void PopOutOnHide()
        {
            mUI.animRoot.DOKill();
            mUI.animRoot.DOScale(Vector3.one * .5f, kPopOutTime)
                .OnComplete(() => mUI.gameObject.SetActive(false));
            
            CgOnHide(kPopOutTime);
        }
        
        private void PopMoveOnOpen(Vector2 dir)
        {
            var oriPos = -dir * new Vector2(960, 540);
            var tarPos = Vector2.zero;
            
            mUI.animRoot.DOKill();
            mUI.animRoot.anchoredPosition = oriPos;

            DOTween.To(() => mUI.animRoot.anchoredPosition, x => mUI.animRoot.anchoredPosition = x, tarPos, kPopMoveTime);
            
            CgOnOpen(kPopMoveTime);
        }

        private void PopMoveOnHide(Vector2 dir)
        {
            var oriPos = Vector2.zero;
            var tarPos = dir * new Vector2(960, 540);
            
            mUI.animRoot.DOKill();
            mUI.animRoot.anchoredPosition = oriPos;
            
            DOTween.To(() => mUI.animRoot.anchoredPosition, x => mUI.animRoot.anchoredPosition = x, tarPos, kPopMoveTime)
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
            var dirX = type == UIAnimType.PopMoveLeft? -1 : 0
                + type  == UIAnimType.PopMoveRight ? 1 : 0;
            var dirY = type == UIAnimType.PopMoveDown ? -1 : 0
                + type  == UIAnimType.PopMoveUp ? 1 : 0;
        
            return new Vector2(dirX, dirY);
        }
        
        // private Vector2 GetDir(UIAnimType type)
        // {
        //     var dirX = (type & UIAnimType.PopMoveLeft) == UIAnimType.PopMoveLeft? -1 : 0
        //         + (type & UIAnimType.PopMoveRight) == UIAnimType.PopMoveRight ? 1 : 0;
        //     var dirY = (type & UIAnimType.PopMoveDown) == UIAnimType.PopMoveDown ? -1 : 0
        //         + (type & UIAnimType.PopMoveUp) == UIAnimType.PopMoveUp ? 1 : 0;
        //
        //     return new Vector2(dirX, dirY);
        // }
    }
}