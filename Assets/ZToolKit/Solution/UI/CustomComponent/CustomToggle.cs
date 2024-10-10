using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ZToolKit
{
    public class CustomToggle : Toggle
    {
        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            
            AudTool.PlaySfx(CfgTool.Audio.EnterTgl);
        }
        
        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            
            AudTool.PlaySfx(CfgTool.Audio.ClickTgl);
        }
    }
}
