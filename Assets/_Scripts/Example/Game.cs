using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZToolKit;

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
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("Input A");
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log("Input W");
            var da = Instantiate(Resources.Load<GameObject>("dagger"));
            da.transform.localScale = Vector3.one * 5;
        }

        if (ZToolKitCore.Instance.Initialized && !open)
        {
            open = true;
            UIManager.OpenUIScreen<ExampleUI>(UIManager.UIPanel.Normal, "测试成功");
        }
    }
}
