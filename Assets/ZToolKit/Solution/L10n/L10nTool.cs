using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZToolKit
{
    public enum Language
    {
        English,
        Chinese,
    }
    
    public static class L10nTool
    {
        public static Language Language
        {
            get => sLanguage;
            set => Set(value);
        }

        private static Language sLanguage;
        
        private static void Set(Language language)
        {
            sLanguage = language;
            Debug.Log("->" + language);
        }
    }
}