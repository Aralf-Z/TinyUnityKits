/*

*/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace ZToolKit
{
    public class InputKeys<T> where T: Enum
    {
        private Dictionary<T, InputKey<T>> mInputKeyDic;

        public InputKeys(int keysNum)
        {
            mInputKeyDic = new Dictionary<T, InputKey<T>>(keysNum);
        }

        /// <summary>
        /// 命令、键位、输入类型
        /// </summary>
        /// <param name="keys"></param>
        public InputKeys(Tuple<T, KeyCode, InputKey<T>.InputType>[] keys)
        {
            mInputKeyDic = new(keys.Length);
            foreach ((T item1, KeyCode item2, InputKey<T>.InputType item3) in keys) 
                mInputKeyDic.Add(item1, new InputKey<T>(item1, item2, item3));
        }

        public InputKey<T> AddInputKey(T command, KeyCode keycode, InputKey<T>.InputType type = default)
        {
            var newInputKey = new InputKey<T>(command, keycode, type);
            mInputKeyDic.Add(command, newInputKey);

            return newInputKey;
        }
        
        public void SetInputType(T command, InputKey<T>.InputType inputType)
        {
            mInputKeyDic[command].SetInputType(inputType);
        }
    
        public void SetPlayerKey(T command, KeyCode newKeyCode)
        {
            mInputKeyDic[command].SetPlayerKey(newKeyCode);
        }

        
        public void SetPlayerKey2(T command, KeyCode newKeyCode2)
        {
            mInputKeyDic[command].SetPlayerKey2(newKeyCode2);
        }
        
        public void SetAllKeysDefault()
        {
            foreach (var element in mInputKeyDic.Values) 
                element.SetDefaultKey();
        }
        
        public void UpdateKeysStatus()
        {
            foreach (var key in mInputKeyDic.Values)
                key.GetKeyState();
        }
    }
}