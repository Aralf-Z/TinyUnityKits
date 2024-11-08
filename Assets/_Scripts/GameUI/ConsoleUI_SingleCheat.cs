using System.Collections;
using System.Collections.Generic;
using RedSaw.CommandLineInterface;
using UnityEngine;
using ZToolKit;

public class ConsoleUI_SingleCheat : UIElement
    , IObject<ConsoleUI_SingleCheat>
{
    public override void Init()
    {
        
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

    public ConsoleUI_SingleCheat SetCommand(Command command)
    {
        
        return this;
    }
    
    bool IObject<ConsoleUI_SingleCheat>.IsCollected { get; set; } 
    public void OnRecycle() { }
}
