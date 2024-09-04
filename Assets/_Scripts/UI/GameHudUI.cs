using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZToolKit;

public class GameHudUI :UIScreen
{
    [Header("GameHudUI")] 
    public Button settingBtn;
    public Button helpBtn;
    public Button backMainBtn;
    
    protected override void OnInit()
    {
        settingBtn.onClick.AddListener(() => UITool.OpenUI<SettingUI>(UIPanel.Tip));
        helpBtn.onClick.AddListener(() => UITool.OpenUI<HelpUI>(UIPanel.Tip));
        backMainBtn.onClick.AddListener(() =>
        {
            HideSelf();
            UITool.OpenUI<MainMenuUI>(UIPanel.Normal);
        });
    }

    protected override void OnOpen(object data)
    {
        AudTool.PlayMusic(CfgTool.Audio.GameBgm);
    }

    protected override void OnHide()
    {
        
    }
}
