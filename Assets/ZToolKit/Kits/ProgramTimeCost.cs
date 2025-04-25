using System;
using UnityEngine;

public static class ProgramTimeCost
{
    private static float startTime;

    public static void StartCount()
    {
        startTime = Time.realtimeSinceStartup;
    }

    public static float EndCount()
    {
        return Time.realtimeSinceStartup - startTime;
    }

    public static void LogMethodTimeCost(Action act)
    {
        var timeStart = Time.realtimeSinceStartup;
        act.Invoke();
        var timeEnd = Time.realtimeSinceStartup;
        Debug.Log($"[{act.Method.Name}] Cost Time: {timeEnd - timeStart}");
    }
}
