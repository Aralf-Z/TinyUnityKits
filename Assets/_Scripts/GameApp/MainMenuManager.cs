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

        this.RegisterEvent<EvtOnKillEnemy>(DebugEnemy);
        this.SendCommand(new CmdKillEnemy(1));
        this.SendCommand(new CmdKillEnemy(2));
        this.SendCommand(new CmdKillEnemy(3));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            UITool.OpenUI<ConsoleUI>(UIPanel.Tip);
        }
    }

    private void DebugEnemy(EvtOnKillEnemy killEnemy)
    {
        Debug.Log($"还剩：{killEnemy.enemyCount}敌人");
    }
    
    public IArchitecture GetArchitecture()
    {
        return GameCoreMgr.Interface;
    }
}
