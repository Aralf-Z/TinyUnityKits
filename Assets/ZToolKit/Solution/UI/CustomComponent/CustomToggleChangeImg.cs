using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ZToolKit
{
    public class CustomToggleChangeImg : CustomToggle
    {
        private Image mSelfImg;

        protected override void Awake()
        {
            base.Awake();

            mSelfImg = transform.GetComponent<Image>();
            
            onValueChanged.AddListener(on =>
            {
                mSelfImg.color = new Color(1, 1, 1, on ? 0 : 1);
            });
        }
    }
}
