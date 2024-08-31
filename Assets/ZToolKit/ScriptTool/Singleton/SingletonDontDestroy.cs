using System;
using UnityEngine;

namespace ZToolKit
{
    public abstract class SingletonDontDestroy<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T sInstance;

        public static T Instance
        {
            get
            {
                if (sInstance)
                {
                    return sInstance;
                }

                var resName = typeof(T).Name;
                if (ResTool.IsExist(resName))
                {
                    var prefab = ResTool.Load<GameObject>(resName);
                    sInstance = Instantiate(prefab).GetComponent<T>();
                }
                else
                {
                    var go = new GameObject(resName);
                    sInstance = go.AddComponent<T>();
                }
                
                return sInstance;
            }
        }

        private void Awake()
        {
            if (sInstance)
            {
                Debug.LogError($"There are two or more {typeof(T).Name}s in scene");
                return;
            }

            sInstance = transform.GetComponent<T>();
            DontDestroyOnLoad(gameObject);
            OnAwake();
        }

        private void Start()
        {
            OnStart();
        }

        protected abstract void OnAwake();
        
        protected abstract void OnStart();
    }
}