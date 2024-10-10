using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ZToolKit
{
    /// <summary>
    /// 同一个对象下的Image为底背景，scrollArea会跟随value变化改变
    /// 挂在ScrollBar组件的父对象上
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Image))]
    public class CustomScrollBar : MonoBehaviour
    {
        public Image scrollAreaImg;
        
        private void Awake()
        {
            var scrollbar = transform.GetComponentInChildren<Scrollbar>();
            
            scrollAreaImg.type = Image.Type.Filled;
            scrollAreaImg.fillMethod = Image.FillMethod.Horizontal;

            scrollbar.onValueChanged.AddListener(x =>
            {
                scrollAreaImg.fillAmount = x;
            });
        }
    }
}
