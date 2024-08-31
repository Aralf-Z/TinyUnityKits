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
        public static string LubanConfigPath => Application.productName + "LubanConfigPath";
        public static string LubanConfigUrl => Application.productName + "LubanConfigUrl";
        public static string LubanOutputType => Application.productName + "LubanOutputType";
    }
}

#endif