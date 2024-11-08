using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using ZToolKit;

public class ConsoleUI_CheatPanel : UIElement
{
    public ConsoleUI_SingleCheat singleCheat;
    public Transform cheatContainer;
    
    private RectTransform mRectTransf;
    private MonoBehaviourPool<ConsoleUI_SingleCheat> mCheatPool;

    private ConsoleUI.CheatSystem mSystem = new ConsoleUI.CheatSystem();
    
    public override void Init()
    {
        mRectTransf = (RectTransform) transform;
        mCheatPool = new MonoBehaviourPool<ConsoleUI_SingleCheat>
            (cheatContainer, singleCheat.gameObject, x => x.Init());
        singleCheat.gameObject.SetActive(false);
    }

    public override void Open()
    {
        mRectTransf.DOKill();
        mRectTransf.anchoredPosition = new Vector2(-780, 0);
        gameObject.SetActive(true);
        mRectTransf.DOAnchorPosX(0, .5f);

        foreach (var cmd in mSystem.allCommands)
        {
            // var scr = mCheatPool.Get();
            // scr.
        }
    }

    public override void UpdateSelf()
    {
        
    }

    public override void Hide()
    {
        mRectTransf.DOKill();
        mRectTransf.anchoredPosition = new Vector2(0, 0);
        mRectTransf.DOAnchorPosX(-780, .5f)
            .OnComplete(() => gameObject.SetActive(false));
        
        mCheatPool.RecycleUsing();
    }
    
}
