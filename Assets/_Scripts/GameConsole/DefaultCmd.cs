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
    
    [Command(name: "杀死敌人", desc: "杀死x个敌人")]
    private static void KillEnemy(int num)
    {
        cc.SendCommand(new CmdKillEnemy(num));
    }
}