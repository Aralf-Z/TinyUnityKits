using System;
using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;
using ZToolKit;

public class MainMenuManager : MonoBehaviour
,IController
{
    private void Start()
    {
        UITool.OpenUI<MainMenuUI>(UIPanel.Normal);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            UITool.OpenUI<ConsoleUI>(UIPanel.Tip);
        }
    }

    public IArchitecture GetArchitecture()
    {
        return GameCoreMgr.Interface;
    }
}
 