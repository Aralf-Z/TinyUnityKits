using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZToolKit
{
    public interface ICanLog
    {
        
    }

    public static class LogExtension
    {
        public static void Info(this ICanLog log, string message, Color color)
        {
            LogTool.Info(log.GetType().Name, message, color);
        }
        
        public static void Debug(this ICanLog log, string message, Color color)
        {
            LogTool.Debug(log.GetType().Name, message, color);
        }
        
        public static void Warning(this ICanLog log, string message)
        {
            LogTool.Warning(log.GetType().Name, message);
        }
        
        public static void Error(this ICanLog log, string message)
        {
            LogTool.Error(log.GetType().Name, message);
        }
    }
}
