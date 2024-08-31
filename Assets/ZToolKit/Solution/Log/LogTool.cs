/*

这是一个核心层类
*/

using System;
using UnityEngine;

namespace ZToolKit
{   
    public static class LogTool
    {
        public static void EditorLog(string headStr, string messageStr, Color color = default)
        {
#if UNITY_EDITOR
            color = color == default ? Color.white : color;
            Debug.Log($"<color={color.ToHex()}>[{headStr}]: {messageStr}</color>");
#endif
        }

        public static void EditorLogError(string messageStr)
        {
#if UNITY_EDITOR
            Debug.LogError(messageStr);
#endif
        }
        
        public static void EditorLogError(Exception messageStr)
        {
#if UNITY_EDITOR
            Debug.LogError(messageStr);
#endif
        }
        
        public static void ZToolKitLog(string headStr, string messageStr)
        {
#if UNITY_EDITOR
            Debug.Log($"<color=#bd868dff>[ZTool-{headStr}]: {messageStr}</color>");
#endif
        }
        
        public static void ZToolKitLogError(string headStr, string messageStr)
        {
#if UNITY_EDITOR
            Debug.Log($"<color=#CA463Dff>[ZTool-{headStr}]: {messageStr}</color>");
#endif
        }
    }
} 