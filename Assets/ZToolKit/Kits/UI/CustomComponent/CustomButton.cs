using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using ZToolKit;

public class CustomButton : Button
{
    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        
        AudTool.PlaySfx(CfgTool.Audio.EnterBtn);
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        
        AudTool.PlaySfx(CfgTool.Audio.ClickBtn);
    }
}
