using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using RedSaw.CommandLineInterface;
using UnityEngine;
using ZToolKit;

public class ConsoleUI_CheatPanel : UIElement<ConsoleUI_CheatPanel>
{
    public ConsoleUI_SingleCheat singleCheat;
    public Transform cheatContainer;
    
    private RectTransform mRectTransf;
    private MonoBehaviourPool<ConsoleUI_SingleCheat> mCheatPool;

    private ConsoleController<LogType> mConsole;
    
    public override ConsoleUI_CheatPanel Init()
    {
        mRectTransf = (RectTransform) transform;
        mCheatPool = new MonoBehaviourPool<ConsoleUI_SingleCheat>
            (cheatContainer, singleCheat.gameObject, x => x.Init().SetSubmitAct(OnClickSubmit));
        singleCheat.gameObject.SetActive(false);
        
        return this;
    }

    public void SetActiveOnInit(bool active)
    {
        gameObject.SetActive(active);
    }

    public void SetConsole(ConsoleController<LogType> console)
    {
        mConsole = console;
        RegisterCommands();
    }
    
    public override void Open()
    {
        mRectTransf.DOKill();
        mRectTransf.anchoredPosition = new Vector2(810, 0);
        gameObject.SetActive(true);
        mRectTransf.DOAnchorPosX(1600, .5f);
    }

    public override void UpdateSelf()
    {
        
    }

    public override void Hide()
    {
        mRectTransf.DOKill();
        mRectTransf.anchoredPosition = new Vector2(1600, 0);
        mRectTransf.DOAnchorPosX(810, .5f)
            .OnComplete(() => gameObject.SetActive(false));
    }


    #region CheatLogic

    private void RegisterCommands()
    {
        foreach (var callable in mConsole.CommandSystem.Vm.AllCallables)
        {
            if (callable is Command cmd)
            {
                var scr = mCheatPool.Get();
                scr.SetCommand(cmd);
            }
        }
    }
    
    private void OnClickSubmit(Command command, string paras)
    {
        var commandStr = $"{command.name} {paras}";
        mConsole.OnSubmit(commandStr);
    }

    #endregion
}
