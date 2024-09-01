using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ZToolKit
{
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
