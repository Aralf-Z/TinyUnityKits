using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ZToolKit
{
    public class CustomToggle : Toggle
    {
        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            
            AudioTool.PlaySfx(CfgTool.Audio.ClickTgl);
        }
    }
}
