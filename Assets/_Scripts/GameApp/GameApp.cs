using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZToolKit;

public class GameApp : SingletonDontDestroy<GameApp>
{
    private Action<float> mUpdateAct;
    
    protected override void OnAwake()
    {
        
    }

    protected override void OnStart()
    {
        
    }

    public void RunApp()
    {
        if (GameConfig.isConsoleActive)
        {
            mUpdateAct += dt =>
            {
                if (Input.GetKeyDown(KeyCode.BackQuote))
                {
                    UITool.OpenUI<ConsoleUI>();
                }
            };
        }
    }
    
    private void Update()
    {
        mUpdateAct?.Invoke(Time.deltaTime);
    }
}
