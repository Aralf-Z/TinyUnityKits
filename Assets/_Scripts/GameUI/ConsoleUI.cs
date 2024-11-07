using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZToolKit;

public class ConsoleUI : UIScreen
{
    protected override void OnInit()
    {
        
    }

    protected override void OnOpen(object data)
    {
        
    }

    protected override void OnHide()
    {
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            HideSelf();
        }
    }
}
