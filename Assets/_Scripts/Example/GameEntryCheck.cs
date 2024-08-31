using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZToolKit;
using cfg;
using UnityEngine.SceneManagement;

public class GameEntryCheck : MonoBehaviour
{
    private bool mAllInitialized;
    
    private void Update()
    {
        if (!mAllInitialized)
        {
            if (ZToolKitCore.Instance.Inited)
            {
                SceneManager.LoadScene("_Scenes/MainMenu");
            }

            mAllInitialized = ZToolKitCore.Instance.Inited;
        }
    }
}
