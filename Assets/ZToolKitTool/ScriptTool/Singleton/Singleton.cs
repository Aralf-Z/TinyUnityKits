
namespace PFramework
{
    /// <summary>
    /// 普通单例类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Singleton<T> where T : new()
    {
        private static T sInstance;
        public static T Instance => sInstance ??= new T();
        protected Singleton() { }
    }
}