using UnityEngine;
using UnityEngine.UI;
using ZToolKit;


public class SettingUI : UIScreen
{
    public Button exitBtn;

    public Button fullScreenBtn;
    public Button windowedBtn;

    public Image fullScreenTick;
    public Image windowedTick;

    public Scrollbar musicScroll;
    public Scrollbar sfxScroll;
    
    protected override void OnInit()
    {
        // exitBtn.onClick.AddListener(HideSelf);
        //
        // fullScreenBtn.onClick.AddListener(() => SetFullScreen(true));
        // windowedBtn.onClick.AddListener(() => SetFullScreen(false));
        //
        // musicScroll.onValueChanged.AddListener(AudioTool.SetMusicVol);
        // sfxScroll.onValueChanged.AddListener(AudioTool.SetSfxVol);
    }

    protected override void OnOpen(object data)
    {
        SetFullScreen(Screen.fullScreen);
    }

    protected override void OnHide()
    {
        
    }

    private void SetFullScreen(bool active)
    {
        // Screen.fullScreen = active;
        // fullScreenTick.enabled = active;
        // windowedTick.enabled = !active;
    }
}
