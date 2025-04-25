using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ZToolKit
{
    public class Toggles
    {
        public Toggle CurToggle => mCurToggle;
        
        private Toggle mCurToggle;

        public Toggles(Toggle defaultToggle,  params Toggle[] toggles)
        {
            mCurToggle = defaultToggle;
            
            foreach (var toggle in toggles)
            {
                toggle.isOn = toggle == mCurToggle;
            }
            
            foreach (var toggle in toggles)
            {
                toggle.onValueChanged.AddListener(isOn => OnToggleChange(toggle, isOn));
            }
        }

        private void OnToggleChange(Toggle toggle, bool isOn)
        {
            if (isOn)
            {
                mCurToggle.isOn = false;
                mCurToggle = toggle;
            }
        }
    }
}
