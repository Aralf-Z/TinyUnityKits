using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZToolKit;
using cfg;

public class Game : MonoBehaviour
{
    private bool open;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    

    // Update is called once per frame
    void Update()
    {
        if (ZToolKitCore.Instance.Initialized && !open)
        {
            Debug.Log("Open ExampleUI");
            open = true;
            var ui = UIManager.OpenUIScreen<ExampleUI>(UIManager.UIPanel.Normal, "Test Success\n 测试成功");
            Debug.Log("===" + ui.gameObject.activeSelf);
        }
    }
}
