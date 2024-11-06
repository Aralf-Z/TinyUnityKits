using System;
using UnityEngine;
using UnityEngine.UI;
using ZToolKit;

public class SettingUI : UIScreen
{
    protected override string SfxOnOpen => CfgTool.Audio.PopOut;

    protected override string SfxOnHide => CfgTool.Audio.PopHide;

    [Header("SettingUI")]
    public Toggle fullScreenTgl;
    public Toggle windowedTgl;
    
    public Toggle englishTgl;
    public Toggle chineseTgl;

    public Toggle audioTgl;
    public Scrollbar musicScroll;
    public Scrollbar sfxScroll;

    public Button exitBtn;

    private Toggles mDisplayTgls;
    private Toggles mLanguageTgls;
    
    protected override void OnInit()
    {
        DisplayInit();
        LanguageInit();
        AudioInit();
        
        exitBtn.onClick.AddListener(HideSelf);
    }

    protected override void OnOpen(object data)
    {
        
    }

    protected override void OnHide()
    {
        
    }

    private void DisplayInit()
    {
        mDisplayTgls = new Toggles(Screen.fullScreen ? fullScreenTgl : windowedTgl, windowedTgl, fullScreenTgl);

        fullScreenTgl.onValueChanged.AddListener(isOn =>
        {
            Screen.fullScreen = isOn;
        });
    }

    private void LanguageInit()
    {
        mLanguageTgls = new Toggles(L10nTool.Language switch
        {
            Language.English => englishTgl,
            Language.Chinese => chineseTgl,
            _ => throw new ArgumentOutOfRangeException()
        }, englishTgl, chineseTgl);

        englishTgl.onValueChanged.AddListener(isOn =>
        {
            if (isOn)
            {
                L10nTool.Language = Language.English;
            }
        });
        
        chineseTgl.onValueChanged.AddListener(isOn =>
        {
            if (isOn)
            {
                L10nTool.Language = Language.Chinese;
            }
        });
    }

    private void AudioInit()
    {
        audioTgl.isOn = AudTool.IsActive;
        musicScroll.value = AudTool.MusicVol;
        sfxScroll.value = AudTool.SfxVol;
            
        audioTgl.onValueChanged.AddListener(isOn =>
        {
            if (isOn != AudTool.IsActive)
            {
                AudTool.SetActive(isOn);
                if (isOn)
                {
                    //todo 播放缓存音乐
                }
            }
        });
        
        musicScroll.onValueChanged.AddListener(value =>
        {
            AudTool.SetMusicVol(value);
            //AudTool.PlayTest(CfgTool.Audio.DragBarMusic, value);
        });
        sfxScroll.onValueChanged.AddListener(value =>
        {
            AudTool.SetSfxVol(value);
            //AudTool.PlayTest(CfgTool.Audio.DragBarSfx, value);
        });
    }
}
