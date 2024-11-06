using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZToolKit;

public class HelpUI : UIScreen
{
    [Header("HelpUI")] 
    public Button exitBtn;
    
    protected override void OnInit()
    {
        exitBtn.onClick.AddListener(HideSelf);
    }

    protected override void OnOpen(object data)
    {
       
    }

    protected override void OnHide()
    {
       
    }
}
