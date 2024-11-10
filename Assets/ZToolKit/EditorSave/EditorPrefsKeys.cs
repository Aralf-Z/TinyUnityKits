/*

*/

using UnityEngine;

#if UNITY_EDITOR

namespace ZToolKit
{
    public static class EditorPrefsKeys
    {
        //lubanConfig
        public static string LubanPath => Application.productName + "LubanPath";
        public static string LubanConfigPath => Application.productName + "LubanConfigPath";
        public static string LubanConfigUrl => Application.productName + "LubanConfigUrl";
        public static string LubanOutputType => Application.productName + "LubanOutputType";
    }
}

#endif