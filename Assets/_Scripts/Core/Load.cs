using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using ZToolKit;

public class Load : MonoBehaviour
{
    private bool Inited;

    private async void Update()
    {
        if (!Inited)
        {
            Inited = true;
            await ToolKit.Init();
            
            SceneManager.LoadScene("_Scenes/MainMenu");
        }
    }
}
