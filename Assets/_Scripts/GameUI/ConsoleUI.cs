using System;
using System.Collections;
using System.Collections.Generic;
using RedSaw.CommandLineInterface;
using UnityEngine;
using UnityEngine.UI;
using ZToolKit;

public class ConsoleUI : UIScreen
{
    [Space(5f)]
    [Header("CheatUI")]
    public ConsoleUI_CheatPanel cheatPanel;

    public Button cheatBtn;
    public Image cheatBtnImg;

    protected override void OnInit()
    {
        cheatPanel.Init();
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
        if (cheatPanel.isOpen)
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

    #region CheatPanel

    public class CheatSystem
    {
        private ICommandCreator mCommandCreator = new CommandCreator();
        private VirtualMachine mVm = new VirtualMachine();

        public List<Command> allCommands = new List<Command>();
        
        public CheatSystem()
        {
            foreach (var command in mCommandCreator.CollectCommands<CommandAttribute>())
            {
                allCommands.Add(command);
                mVm.RegisterCallable(command);
            }
        }
    }

    #endregion
}
