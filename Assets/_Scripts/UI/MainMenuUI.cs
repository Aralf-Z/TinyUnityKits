using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZToolKit;

public class MainMenuUI : UIScreen
{
    public Button startBtn;
    public Button settingBtn;
    public Button exitBtn;
    
    protected override void OnInit()
    {
        startBtn.onClick.AddListener(OnClickStartBtn);
        settingBtn.onClick.AddListener(OnClickSettingBtn);
        exitBtn.onClick.AddListener(OnClickExitBtn);
    }

    protected override void OnOpen(object data)
    {

    }

    protected override void OnHide()
    {
       
    }
    
    private void OnClickStartBtn()
    {
        Debug.Log("开始游戏");
    }

    private void OnClickSettingBtn()
    {
        UITool.OpenUI<SettingUI>(UIPanel.Tip);
    }
    
    private void OnClickExitBtn()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
