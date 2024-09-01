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
        
        AudioTool.PlaySfx(CfgTool.Audio.EnterBtn);
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        
        AudioTool.PlaySfx(CfgTool.Audio.ClickBtn);
    }
}
