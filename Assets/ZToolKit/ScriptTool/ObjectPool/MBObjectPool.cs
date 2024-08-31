/*

*/

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ZToolKit
{
    /// <summary>
    /// MonoBehaviour脚本对象池
    /// </summary>
    public class MonoBehaviourPool<T> : IObjectPool<T> where T : MonoBehaviour, IObject<T>
    {
        private readonly Queue<T> mPool = new ();
        private readonly HashSet<T> mShowing = new ();
        private readonly Transform mContainer;
        private readonly GameObject mTemplate;
        private readonly Action<T> mInitAct;

        public MonoBehaviourPool(Transform container,GameObject template, Action<T> initAct = null)
        {
            mTemplate = template;
            mContainer = container;
            template.GetComponent<T>();
            template.SetActive(false);
            mInitAct = initAct;
        }

        public T Get()
        {
            if (mPool.Count <= 0)
            {
                var t = Object.Instantiate(mTemplate, mContainer).transform.GetComponent<T>();
                mInitAct?.Invoke(t);
                mPool.Enqueue(t);
            }
            var obj = mPool.Dequeue();
            obj.gameObject.SetActive(true);
            obj.IsCollected = false;
            mShowing.Add(obj);
        
            return obj;
        }

        public void Recycle(T obj)
        {
            if (obj.IsCollected) return;
            obj.IsCollected = true;
            obj.OnRecycle();
            obj.gameObject.SetActive(false);
            mPool.Enqueue(obj);
            mShowing.Remove(obj);
        }

        public void Recycle(IEnumerable<T> objs)
        {
            foreach (var obj in objs) Recycle(obj);
        }
        
        public void ClearCache()
        {
            while (mPool.Count>0)
            {
                var obj = mPool.Dequeue();
                Object.Destroy(obj.gameObject);
            }

            var tempShowing = mShowing.ToList();
            var index = 0;
            
            while (index++ < tempShowing.Count)
                Object.Destroy(tempShowing[index].gameObject);

            mPool.Clear();
            mShowing.Clear();
        }
    }
}