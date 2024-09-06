using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ZToolKit.Editor
{
    [InitializeOnLoad]
    public static class L10nEditor
    {
        private static HashSet<string> sL10nKey = new HashSet<string>();
        
        static L10nEditor()
        {
            
        }

        [InitializeOnLoadMethod]
        private static void LoadL10nKey()
        {
            //Debug.Log("LoadL10nKey");
        }
    }
}