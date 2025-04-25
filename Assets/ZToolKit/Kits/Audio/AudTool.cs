using System;
using System.Collections;
using System.Collections.Generic;
using cfg;
using UnityEngine;

namespace ZToolKit
{
    public static class AudTool
    {
        public static bool IsActive => AudMgr.Instance.IsActive;
        public static float MusicVol => AudMgr.Instance.musicSource.volume; 
        public static float SfxVol =>AudMgr.Instance.sfxSource.volume;

        public static void PlayMusic(string clipName)
        {
            AudMgr.PlayMusic(clipName);
        }
        
        public static void PlaySfx(string clipName)
        {
            AudMgr.PlaySfx(clipName);
        }

        public static void PlayTest(string clipName, float value)
        {
            AudMgr.PlayTestAudio(clipName, value);
        }

        public static void SetActive(bool active)
        {
            AudMgr.Instance.IsActive = active;
        }
        
        public static void SetMusicVol(float value)
        {
            AudMgr.SetMusicVol(value);
        }

        public static void SetSfxVol(float value)
        {
            AudMgr.SetSfxVol(value);
        }
    }
}