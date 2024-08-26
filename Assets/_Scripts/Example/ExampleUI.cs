using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZToolKit;

public class ExampleUI : UIScreen
{
    public Text testText;
    
    protected override void OnInit()
    {
        Debug.Log("Init ExampleUI");
    }

    protected override void OnOpen(object data)
    {
        var msg = (string) data;
        testText.text = msg;
    }

    protected override void OnHide()
    {
        
    }
}
