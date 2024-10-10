using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
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
        
        public static string GetUIStr(Transform refTransform, string key)
        {
            if(UiL10n.DataMap.TryGetValue(key, out var l10N))
            {
                return sLanguage switch
                {
                    Language.Chinese => l10N.Cn,
                    Language.English => l10N.En,
                    _ => default,
                };
            }
            
            LogTool.ZToolKitLogError("L10nTool",$"Invalid Key:{key}, path:{refTransform.GetPath()}");
            return default;
        }

        public static string GetGameStr(string key)
        {
            if(GameL10n.DataMap.TryGetValue(key, out var l10N))
            {
                return sLanguage switch
                {
                    Language.Chinese => l10N.Cn,
                    Language.English => l10N.En,
                    _ => default,
                };
            }
            
            LogTool.ZToolKitLogError("L10nTool",$"Invalid Key:{key}");
            return default;
        }
    }
}