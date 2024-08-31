using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZToolKit;

public class MainMenuManager : MonoBehaviour
{
    private void Start()
    {
        UITool.OpenUI<MainMenuUI>(UIPanel.Normal);
    }
}
