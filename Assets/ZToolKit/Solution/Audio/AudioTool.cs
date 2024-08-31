using System;
using System.Collections;
using System.Collections.Generic;
using cfg;
using UnityEngine;

namespace ZToolKit
{
    public static class AudioTool
    {
        public static void PlayMusic(string clipName)
        {
            if (CheckClip(clipName))
            {
                AudioMgr.PlayMusic(clipName);
            }
        }
        
        public static void PlayOneShot(string clipName)
        {
            if (CheckClip(clipName))
            {
                AudioMgr.PlayOneShot(clipName);
            }
        }
        
        public static void PlaySfx(string clipName)
        {
            if (CheckClip(clipName))
            {
                AudioMgr.PlaySfx(clipName);
            }
        }

        public static void SetMusicVol(float value)
        {
            AudioMgr.SetMusicVol(value);
        }

        public static void SetSfxVol(float value)
        {
            AudioMgr.SetSfxVol(value);
        }

        private static bool CheckClip(string clipName)
        {
#if UNITY_EDITOR
            if (ResTool.IsExist(clipName))
            {
                return true;
            }
            
            LogTool.ZToolKitLogError("AudioTool",$"资源：{clipName} 不存在");
            return false;
#else
            return true;
#endif
        }
    }
}