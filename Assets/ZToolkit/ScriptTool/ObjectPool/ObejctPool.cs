/*

*/

using System.Collections.Generic;

namespace ZToolKit
{
    public class ObjectPool<T> : IObjectPool<T> where T : IObject<T>, new()
    {
        private readonly Queue<T> mPool = new ();
        private readonly HashSet<T> mShowing = new ();
        
        public T Get()
        {
            if (mPool.Count <= 0)
                mPool.Enqueue(new T());
            var obj = mPool.Dequeue();
            obj.IsCollected = false;
            mShowing.Add(obj);
            
            return obj;
        }

        public void Recycle(T obj)
        {
            if (obj.IsCollected) return;
            obj.IsCollected = true;
            obj.OnRecycle();
            mPool.Enqueue(obj);
            mShowing.Remove(obj);
        }

        public void Recycle(IEnumerable<T> objs)
        {
            foreach (var obj in objs) Recycle(obj);
        }
        
        public void ClearCache()
        {
            mPool.Clear();
            mShowing.Clear();
        }
    }
}