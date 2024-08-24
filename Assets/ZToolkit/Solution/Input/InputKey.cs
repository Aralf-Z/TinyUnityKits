/*

*/

using System;
using UnityEngine;

namespace ZToolKit
{
    /// <summary>
    /// 单个输入键位
    /// </summary>
    /// <typeparam name="T"> 命令 </typeparam>
    public class InputKey<T> where T: Enum
    {
        public enum InputType
        {
            /// <summary> 按下 </summary>
            Down = 1 << 0,//1
            /// <summary> 按住 </summary>
            Hold = 1 << 1,//2
            /// <summary> 抬起 </summary>
            Up = 1 << 2,//4
        }
        
        public T Command => mCommand;
        public KeyCode PlayerKey => mPlayerKey;
        public KeyCode PlayerKey2 => mPlayerKey2;
        public InputType KeyInputType => mInputType;
    
        private T mCommand;
        private KeyCode mDefaultKey;
        private KeyCode mPlayerKey;
        private KeyCode mPlayerKey2;
        private InputType mInputType;
        private Action mCallBack;
    
        public InputKey(T command, KeyCode defaultKey, InputType inputType = default)
        {
            mCommand = command;
            mPlayerKey = mDefaultKey = defaultKey;
            mPlayerKey2 = KeyCode.None;
            mInputType = inputType;
        }
    
        public InputKey<T> SetPlayerKey(KeyCode keyCode)
        {
            mPlayerKey = keyCode;
            return this;
        }
    
        public InputKey<T> SetPlayerKey2(KeyCode keyCode)
        {
            mPlayerKey2 = keyCode;
            return this;
        }
        
        public InputKey<T> SetDefaultKey()
        {
            mPlayerKey = mDefaultKey;
            return this;
        }
    
        public InputKey<T> SetInputType(InputType inputType)
        {
            mInputType = inputType;
            return this;
        }
    
        public InputKey<T> SetCallBack(Action callBack)
        {
            mCallBack = callBack;
            return this;
        }
        
        /// <summary>
        /// 获得玩家是否输入
        /// </summary>
        /// <returns></returns>
        public bool GetKeyState()
        {
            var input = mInputType switch
            {
                InputType.Down => Input.GetKeyDown(mPlayerKey) || Input.GetKeyDown(mPlayerKey2),
                InputType.Hold => Input.GetKey(mPlayerKey) || Input.GetKey(mPlayerKey2),
                InputType.Up => Input.GetKeyUp(mPlayerKey) || Input.GetKeyUp(mPlayerKey2),
                _ => false
            };

            if(input)
                mCallBack?.Invoke();

            return input;
        }
    }

}