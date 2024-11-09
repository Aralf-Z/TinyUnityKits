using System.Collections;
using System.Collections.Generic;
using QFramework;
using RedSaw.CommandLineInterface;
using UnityEngine;

public static class DefaultCmd
{
    private class ConsoleController:
        IController
    {
        public IArchitecture GetArchitecture()
        {
            return GameCoreMgr.Interface;
        }
    }

    private static ConsoleController cc = new ConsoleController();
    
    [Command(desc: "打印")]
    private static void printf(string str)
    {
        Debug.Log(str);
    }
}