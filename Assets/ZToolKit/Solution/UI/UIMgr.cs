using System.Collections.Generic;
using UnityEngine;

namespace ZToolKit
{
    public enum UIPanel
    {
        /// <summary>
        /// 背景界面
        /// </summary>
        Background = 0,

        /// <summary>
        /// 一般界面
        /// </summary>
        Normal = 10,

        /// <summary>
        /// 弹窗
        /// </summary>
        Tip = 20
    }
    
    internal class UIMgr : SingletonDontDestroy<UIMgr> 
    {
        private readonly Dictionary<UIPanel, Transform> mUIPanels = new();

        protected override void OnAwake()
        {
            mUIPanels.Add(UIPanel.Background, transform.Find("Background"));
            mUIPanels.Add(UIPanel.Normal, transform.Find("Normal"));
            mUIPanels.Add(UIPanel.Tip, transform.Find("Tip"));
        }

        protected override void OnStart()
        {
            
        }

        #region UIScreen
        
        private readonly Dictionary<string, UIScreen> mUIScreens = new();
        private readonly Dictionary<string, UIScreen> mShowingUIScreens = new();
        
        public static T OpenUIScreen<T>(UIPanel uiPanel, object data = default) where T : UIScreen
        {
            string uiName = typeof(T).Name;
            var uiScreen = (T)GetUIScreen(uiName, uiPanel);
            
            uiScreen.Open(data);
            if (!Instance.mShowingUIScreens.ContainsKey(uiName))
            {
                Instance.mShowingUIScreens.Add(uiName, uiScreen);
            }

            return uiScreen;
        }

        public static T TryGetUIOnOpen<T>() where T : UIScreen
        {
            string uiName = typeof(T).Name;
            if (Instance.mShowingUIScreens.TryGetValue(uiName, out var ui))
            {
                return (T) ui;
            }
            else
            {
                Debug.LogError("UI Is Not Open");
                return null;
            }
        }
        
        private static UIScreen GetUIScreen(string uiName, UIPanel uiPanel)
        {
            if (!Instance.mUIScreens.ContainsKey(uiName))
            {
                var uiPrefab = ResTool.Load<GameObject>(uiName);
                var uiGo = Instantiate(uiPrefab);
                var ui = uiGo.GetComponent<UIScreen>().Init();
                
                Instance.mUIScreens.Add(uiName, ui);
            }
            var screen = Instance.mUIScreens[uiName];
            screen.transform.SetParent(Instance.mUIPanels[uiPanel], false);
            screen.transform.SetAsLastSibling();
            
            return screen;
        }

        public static void HideUIScreen(UIScreen uiScreen)
        {
            HideUIScreen(uiScreen.GetType().Name);
        }
        
        public static void HideUIScreen<T>() where T : UIScreen
        {
            HideUIScreen(typeof(T).Name);
        }

        private static void HideUIScreen(string uiName)
        {
            if (!Instance.mShowingUIScreens.ContainsKey(uiName))
            {
                return;
            }
            var uiScreen = Instance.mShowingUIScreens[uiName];
            uiScreen.Hide();
            Instance.mShowingUIScreens.Remove(uiName);
        }

        public static void HideAllUIScreens()
        {
            foreach (var element in Instance.mShowingUIScreens.Values)
            {
                element.Hide();
            }
            Instance.mShowingUIScreens.Clear();
        }

        public static bool IsOpen<T>() where T : UIScreen
        {
            return IsOpen(typeof(T).Name);
        }

        private static bool IsOpen(string uiName)
        {
            return Instance.mShowingUIScreens.ContainsKey(uiName);
        }

        #endregion
    }
}