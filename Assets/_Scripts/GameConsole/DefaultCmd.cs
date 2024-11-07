using System.Collections;
using System.Collections.Generic;
using QFramework;
using RedSaw.CommandLineInterface;
using UnityEngine;

public static class DefaultCmd
{
    [Command(desc: "杀死一个敌人")]
    private static void KillEnemy(int num)
    {
        ConsoleCore.Instance.SendCommand(new CmdKillEnemy(num));
    }
}
