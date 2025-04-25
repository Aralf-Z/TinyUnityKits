using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZToolKit
{
    public abstract class SaveBase
    {
        /// <summary>
        /// 游戏版本号
        /// </summary>
        public string version;
        /// <summary>
        /// 档案名称
        /// </summary>
        public string archive;
        /// <summary>
        /// 存档名称
        /// </summary>
        public string name;
        /// <summary>
        /// 存档时间
        /// </summary>
        public DateTime saveTime;
        /// <summary>
        /// 已游玩时长，秒
        /// </summary>
        public uint playedTime;
    }
}
