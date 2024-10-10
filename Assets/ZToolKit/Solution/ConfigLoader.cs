using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZToolKit
{
    public static class ConfigLoader
    {
        public static void Init()
        {
            var buildConfig = ResTool.Load<BuildConfig>("BuildConfig");

            LogTool.logZToolKit = buildConfig.logToolKit;
        }
    }
}
