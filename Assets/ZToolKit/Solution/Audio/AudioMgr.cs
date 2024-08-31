using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZToolKit
{
    internal class AudioMgr : SingletonDontDestroy<AudioMgr>
    {
        public AudioSource musicSource;
        public AudioSource sfxSource;

        protected override void OnAwake()
        {
            musicSource.loop = true;
            sfxSource.loop = false;
        }

        protected override void OnStart()
        {
            
        }
        
        public static void PlayMusic(string clipName)
        {
            Instance.musicSource.clip = ResTool.Load<AudioClip>(clipName);
            Instance.musicSource.Play();
        }
        
        public static void PlayOneShot(string clipName)
        {
            Instance.sfxSource.PlayOneShot(ResTool.Load<AudioClip>(clipName));
        }
        
        public static void PlaySfx(string clipName)
        {
            Instance.sfxSource.clip = ResTool.Load<AudioClip>(clipName);
            Instance.sfxSource.Play();
        }

        public static void SetMusicVol(float value)
        {
            Instance.musicSource.volume = value;
        }

        public static void SetSfxVol(float value)
        {
            Instance.sfxSource.volume = value;
        }
    }
}
