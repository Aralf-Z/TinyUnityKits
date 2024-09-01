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
    
    protected override void OnInit()
    {
        DisplayInit();
        LanguageInit();
        AudioInit();
    }

    protected override void OnOpen(object data)
    {
        
    }

    protected override void OnHide()
    {
        
    }

    private void DisplayInit()
    {
        fullScreenTgl.isOn = Screen.fullScreen;
        
        fullScreenTgl.onValueChanged.AddListener(isOn =>
        {
            if (Screen.fullScreen != isOn)
            {
                Screen.fullScreen = isOn;
            }
        });
    }

    private void LanguageInit()
    {
        englishTgl.isOn = L10nTool.Language == Language.English;
        
        englishTgl.onValueChanged.AddListener(isOn =>
        {
            if (L10nTool.Language == Language.English)
            {
                return;
            }

            if (isOn)
            {
                L10nTool.Language = Language.English;
            }
        });
        
        chineseTgl.onValueChanged.AddListener(isOn =>
        {
            if (L10nTool.Language == Language.Chinese)
            {
                return;
            }

            if (isOn)
            {
                L10nTool.Language = Language.Chinese;
            }
        });
    }

    private void AudioInit()
    {
        audioTgl.isOn = AudioTool.IsActive;
        musicScroll.value = AudioTool.MusicVol;
        sfxScroll.value = AudioTool.SfxVol;
            
        audioTgl.onValueChanged.AddListener(isOn =>
        {
            if (isOn != AudioTool.IsActive)
            {
                AudioTool.SetActive(isOn);
            }
        });
        
        musicScroll.onValueChanged.AddListener(AudioTool.SetMusicVol);
        sfxScroll.onValueChanged.AddListener(AudioTool.SetSfxVol);
    }
}
