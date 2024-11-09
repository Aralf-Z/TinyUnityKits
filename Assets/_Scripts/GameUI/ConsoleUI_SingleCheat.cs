using System;
using System.Collections;
using System.Collections.Generic;
using RedSaw.CommandLineInterface;
using UnityEngine;
using UnityEngine.UI;
using ZToolKit;

public class ConsoleUI_SingleCheat : UIElement<ConsoleUI_SingleCheat>
    , IObject<ConsoleUI_SingleCheat>
{
    public Text inputTxt;
    public Button submitBtn;
    public Text descriptionTxt;

    private Command mCommand;
    
    public override ConsoleUI_SingleCheat Init()
    {
        return this;
    }

    public void SetSubmitAct(Action<Command, string> submitAct)
    {
        submitBtn.onClick.AddListener(() => submitAct?.Invoke(mCommand, inputTxt.text));
    }
    
    public override void Open()
    {
        
    }

    public override void UpdateSelf()
    {
        
    }

    public override void Hide()
    {
        
    }

    public void SetCommand(Command command)
    {
        mCommand = command;
        descriptionTxt.text = $"控制台命令：{command.name};\n描述：{command.description}";
    }
    
    bool IObject<ConsoleUI_SingleCheat>.IsCollected { get; set; } 
    public void OnRecycle() { }
}
