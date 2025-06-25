/*

*/

using System;
using UnityEngine;

namespace ZToolKit
{
    public class AnimatorEvent : MonoBehaviour
    {
        private Action OnPlayCallBack;
        private Action OnEndCallBack;
        private Action OnCallBack1;
        private Action OnCallBack2;

        public void OnAnimatorPlayCallBack() => OnPlayCallBack?.Invoke();

        public void OnAnimatorEndCallBack() => OnEndCallBack?.Invoke();

        public void OnAnimatorCallBack1() => OnCallBack1?.Invoke();

        public void OnAnimatorCallBack2() => OnCallBack2?.Invoke();

        public void SetOnPlayCallBack(Action onPlayCallBack) => OnPlayCallBack = onPlayCallBack;

        public void SetOnEndCallBack(Action onEndCallBack) => OnEndCallBack = onEndCallBack;

        public void SetOnCallBack1(Action onCallBack1) => OnCallBack1 = onCallBack1;

        public void SetOnCallBack2(Action onCallBack2) => OnCallBack2 = onCallBack2;
    }
}