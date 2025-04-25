using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZToolKit
{
    public abstract class UIElement<T>: MonoBehaviour where T: MonoBehaviour 
    {
        /// <summary> 游戏对象激活状态 </summary>
        public bool GoActive => gameObject.activeSelf;
        public abstract T Init();
        public abstract void Open();
        public abstract void UpdateSelf();
        public abstract void Hide();
    }
}
