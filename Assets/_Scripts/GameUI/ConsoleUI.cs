using System;
using System.Collections;
using System.Collections.Generic;
using RedSaw.CommandLineInterface;
using UnityEngine;
using UnityEngine.UI;
using ZToolKit;

public class ConsoleUI : UIScreen
{
    [Header("CheatUI")]
    public ConsoleUI_CheatPanel cheatPanel;

    public Button cheatBtn;
    public Image cheatBtnImg;

    public bool isOpenCheatOnInit;

    protected override void OnInit()
    {
        cheatPanel.SetActiveOnInit(isOpenCheatOnInit);
        cheatBtnImg.transform.localScale = new Vector3(isOpenCheatOnInit ? 1 : -1, 1, 1);
        cheatBtn.onClick.AddListener(OnClickCheatBtn);
    }

    protected override void OnOpen(object data)
    {
        
    }

    protected override void OnHide()
    {
        
    }

    private void OnClickCheatBtn()
    {
        if (cheatPanel.GoActive)
        {
            cheatBtnImg.transform.localScale = new Vector3(-1,1,1);
            cheatPanel.Hide();
        }
        else
        {
            cheatBtnImg.transform.localScale = new Vector3(1,1,1);
            cheatPanel.Open();
        }
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            HideSelf();
        }
    }
}
