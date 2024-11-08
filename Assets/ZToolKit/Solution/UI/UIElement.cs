using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZToolKit
{
    public abstract class UIElement: MonoBehaviour
    {
        public bool isOpen => gameObject.activeSelf;
        public abstract void Init();
        public abstract void Open();
        public abstract void UpdateSelf();
        public abstract void Hide();
       
    }
}
