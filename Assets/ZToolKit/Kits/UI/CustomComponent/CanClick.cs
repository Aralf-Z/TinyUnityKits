using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ZToolKit
{
    public class CanClick : MonoBehaviour
    ,IPointerClickHandler
    {
        public Action onClickAct;
        
        public void OnPointerClick(PointerEventData eventData)
        {
            onClickAct?.Invoke();
        }
    }
}
