/*

*/

using System.Collections.Generic;

namespace ZToolKit
{
    /// <summary>
    /// 对象接口
    /// </summary>
    public interface IObject<T>
    {
        bool IsCollected { get; set; }
        
        void OnRecycle();
    }

    /// <summary>
    /// 对象池接口
    /// </summary>
    /// <typeparam name="T">对象</typeparam>
    public interface IObjectPool<T> where T : IObject<T>
    {
        T Get();
    
        void Recycle(T obj);
        
        void Recycle(IEnumerable<T> objs);
        
        void ClearCache();
    }
}