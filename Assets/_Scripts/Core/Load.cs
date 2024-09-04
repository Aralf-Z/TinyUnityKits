using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using ZToolKit;

public class Load : MonoBehaviour
{
    public Text loadingTxt;
    private float mLoadingTimer;

    private async void Start()
    {
        await ToolKit.Init();

        SceneManager.LoadScene("_Scenes/MainMenu");
    }

    private void Update()
    {
        mLoadingTimer += Time.deltaTime;
        
        if (mLoadingTimer < .3f)
        {
            loadingTxt.text = "Loading.";
        }
        else if (mLoadingTimer < .6f)
        {
            loadingTxt.text = "Loading..";
        }
        else if (mLoadingTimer < .9f)
        {
            loadingTxt.text = "Loading...";
        }
        else
        {
            mLoadingTimer = 0;
        }
    }
}
