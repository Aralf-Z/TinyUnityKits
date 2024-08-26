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
                
                var prefab = ResTool.Load<GameObject>(typeof(T).Name);
                if (prefab)
                {
                    sInstance = Instantiate(prefab).GetComponent<T>();
                }
                else
                {
                    Debug.LogError($"There is no {typeof(T).Name} in the scene, or any prefab in Resources folder.");
                }
                
                return sInstance;
            }
        }

        private void Awake()
        {
            if (sInstance)
            {
                return;
            }
            
            var instance = FindObjectsOfType<T>();

            if (instance.Length > 1)
            {
                Debug.LogError($"More than one {typeof(T).Name} in this scene");
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