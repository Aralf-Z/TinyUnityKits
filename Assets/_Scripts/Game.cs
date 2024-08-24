using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZToolKit;

public class Game : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            UIManager.OpenUIScreen<ExampleUI>(UIManager.UIPanel.Normal, "测试成功");
        }
    }
}
