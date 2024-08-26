/*

*/

using UnityEngine;

#if UNITY_EDITOR

namespace ZToolKit
{
    public static class EditorPrefsKeys
    {
        //saveConfig
        public static string CanSaveInEditor => Application.productName + "CanSaveInEditor";
        public static string LoadSaveInEditor => Application.productName + "LoadSaveInEditor";
        
        //lubanConfig
        public static string LubanPath => Application.productName + "LubanPath";
        public static string LubanDataPath => Application.productName + "LubanDataPath";
    }
}

#endif