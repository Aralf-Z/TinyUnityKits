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
        
        public static TbL10nUI UiL10n => CfgTool.Tables.TbL10nUI;
        public static TbL10nGame GameL10n => CfgTool.Tables.TbL10nGame;
        
        public static event Action Event_OnChangeLanguage;
        
        private static Language sLanguage;

        static L10nTool()
        {
            Language = Language.Chinese;
        }

        private static void Set(Language language)
        {
            sLanguage = language;
            Event_OnChangeLanguage?.Invoke();
        }
        
        public static string GetUIStr(string key) => sLanguage switch
        {
            Language.Chinese => UiL10n.GetByL10nKey(key).Cn,
            Language.English => UiL10n.GetByL10nKey(key).En,
        };

        public static string GetGameStr(string key) => sLanguage switch
        {
            Language.Chinese => GameL10n.GetByL10nKey(key).Cn,
            Language.English => GameL10n.GetByL10nKey(key).En,
        };
    }
}