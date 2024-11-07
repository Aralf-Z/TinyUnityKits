using System;
using System.Collections;
using System.Collections.Generic;
using QFramework;
using ZToolKit;


    public class ConsoleCore : SingletonDontDestroy<ConsoleCore>
    ,IController
    {
        protected override void OnAwake()
        {
            
        }

        protected override void OnStart()
        {
            
        }

        public IArchitecture GetArchitecture()
        {
            return GameCoreMgr.Interface;
        }
    }
