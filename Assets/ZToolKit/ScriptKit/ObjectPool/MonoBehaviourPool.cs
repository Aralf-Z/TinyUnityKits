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
        private readonly HashSet<T> mUsing = new ();
        private readonly Transform mContainer;
        private readonly GameObject mTemplate;
        private readonly Action<T> mInitAct;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="container">对象容器</param>
        /// <param name="template">对象gameObject模板</param>
        /// <param name="initAct">脚本初始化Act</param>
        public MonoBehaviourPool(Transform container,GameObject template, Action<T> initAct = null)
        {
            mTemplate = template;
            mContainer = container;
            template.GetComponent<T>();
            mInitAct = initAct;
        }

        /// <summary>
        /// 获得一个脚本对象
        /// </summary>
        /// <returns></returns>
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
            mUsing.Add(obj);
        
            return obj;
        }
        
        public void Recycle(T obj)
        {
            if (obj.IsCollected) return;
            obj.IsCollected = true;
            obj.OnRecycle();
            obj.gameObject.SetActive(false);
            mPool.Enqueue(obj);
            mUsing.Remove(obj);
        }

        public void Recycle(IEnumerable<T> objs)
        {
            foreach (var obj in objs)
            {Recycle(obj);}
        }

        /// <summary>
        /// 回收使用中的，注意引用中的对象很可能会被回收
        /// </summary>
        public void RecycleUsing()
        {
            Recycle(mUsing.ToArray());
        }
        
        public void ClearCache()
        {
            while (mPool.Count>0)
            {
                var obj = mPool.Dequeue();
                Object.Destroy(obj.gameObject);
            }

            var tempShowing = mUsing.ToList();
            var index = 0;
            
            while (index++ < tempShowing.Count)
                Object.Destroy(tempShowing[index].gameObject);

            mPool.Clear();
            mUsing.Clear();
        }
    }
}