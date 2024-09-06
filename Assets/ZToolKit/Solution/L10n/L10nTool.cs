using System;
using System.Collections;
using System.Collections.Generic;
using cfg;
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
        
        public static TblL10nUI UiL10n => CfgTool.Tables.TblL10nUI;
        
        public static event Action Event_OnChangeLanguage;
        
        private static Language sLanguage;

        private static LanguageHandlerBase sLanguageHandler;
        
        private static Dictionary<Language, LanguageHandlerBase> sLanguageHandlers = new Dictionary<Language, LanguageHandlerBase>()
        {
            [Language.English] = new EnglishHandler(),
            [Language.Chinese] = new ChineseHandler(),
        };

        static L10nTool()
        {
            sLanguageHandler = sLanguageHandlers[sLanguage];
        }

        private static void Set(Language language)
        {
            sLanguage = language;
            sLanguageHandler = sLanguageHandlers[sLanguage];
            Event_OnChangeLanguage?.Invoke();
        }

        public static string GetStr(string key)
        {
            return sLanguageHandler.GetStr(key);
        }
        
        private class ChineseHandler:LanguageHandlerBase
        {
            public override string GetStr(string key)
            {
                return UiL10n.GetByL10nKey(key).Cn;
            }
        }
        
        private class EnglishHandler:LanguageHandlerBase
        {
            public override string GetStr(string key)
            {
                return UiL10n.GetByL10nKey(key).En;
            }
        }
        
        private abstract class LanguageHandlerBase
        {
            public abstract string GetStr(string key);
        }
    }
}