using UnityEngine;
using UnityEngine.UI;

namespace ZToolKit
{
    [RequireComponent(typeof(Text))]
    public class LocalizationText : LocalizationBase
    {
        protected override void OnLanguageChange()
        {
            ((Text)target).text = L10nTool.GetUIStr((RectTransform)transform, key);
        }
    }
}
