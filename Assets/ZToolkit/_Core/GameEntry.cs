using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZToolKit
{
    public class GameEntry : SingletonDontDestroy<GameEntry>
    {
        protected override void OnAwake()
        {
            ResTool.Init();
        }
    }
}
