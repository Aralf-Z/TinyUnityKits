/*

*/

using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ZToolKit
{
    public enum Date
    {
        Year, Month, Day, Hour, Minute, Second
    }
    
    public static class Extension
    {
        /// <summary>
        /// 判断指针合法-左键或者单指
        /// </summary>
        /// <param name="eventData"></param>
        /// <returns></returns>
        public static bool IsPointerLegal(this PointerEventData eventData)
        {
            
#if UNITY_WEBGL && !UNITY_EDITOR
            return Input.touchCount == 1;
#endif
            //使用鼠标时，pointerId 会返回 -1、-2 或 -3。这些值分别对应鼠标左键、右键和中键。
            //在移动版本（如 iPad、iPhone 或 Android）上使用触摸屏时，多点触摸的范围是从 0 到设备支持的多点触摸数量
            return eventData.pointerId == 0 || eventData.pointerId == -1;
        }
        
        public static bool IsMouseLeft(this PointerEventData eventData)
        {
            return eventData.button == PointerEventData.InputButton.Left;
        }
        
        public static bool IsMouseRight(this PointerEventData eventData)
        {
            return eventData.button == PointerEventData.InputButton.Right;
        }

        public static bool IsMouseMiddle(this PointerEventData eventData)
        {
            return eventData.button == PointerEventData.InputButton.Middle;
        }

        public static DateTime String2DatDateTime(this string time)
        {
            if (DateTime.TryParseExact(time, "yyyy-MM-dd-HH-mm-ss", null, System.Globalization.DateTimeStyles.None, out var parsedTime1))
                return parsedTime1;
            if (DateTime.TryParseExact(time, "yyyy-MM-dd-HH-mm", null, System.Globalization.DateTimeStyles.None, out var parsedTime2))
                return parsedTime2;
            if (DateTime.TryParseExact(time, "yyyy-MM-dd-HH", null, System.Globalization.DateTimeStyles.None, out var parsedTime3))
                return parsedTime3;
            if (DateTime.TryParseExact(time, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out var parsedTime4))
                return parsedTime4;
            if (DateTime.TryParseExact(time, "yyyy-MM", null, System.Globalization.DateTimeStyles.None, out var parsedTime5))
                return parsedTime5;
            if (DateTime.TryParseExact(time, "yyyy", null, System.Globalization.DateTimeStyles.None, out var parsedTime6))
                return parsedTime6;
            
            Debug.LogError("Wrong TimeString, Failed to parse DateTime from string");
            return default;
        }

        public static string ToHex(this Color color)
        {
            byte r = (byte)(color.r * 255f);
            byte g = (byte)(color.g * 255f);
            byte b = (byte)(color.b * 255f);
            byte a = (byte)(color.a * 255f);

            return $"#{r:X2}{g:X2}{b:X2}{a:X2}";
        }

        /// <summary>
        /// 十六进制转换成Color，格式#A6B422FF或者A6B422FF
        /// </summary>
        /// <param name="hexStr"></param>
        /// <returns></returns>
        public static Color Hex2Color(this string hexStr)
        {
            hexStr = hexStr.Replace("#", "");

            byte r = byte.Parse(hexStr.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            byte g = byte.Parse(hexStr.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            byte b = byte.Parse(hexStr.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);

            if(hexStr.Length == 8)
            {
                byte a = byte.Parse(hexStr.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
                return new Color32(r, g, b, a);
            }
            else
            {
                return new Color32(r, g, b, 255);
            }
        }
    }
}