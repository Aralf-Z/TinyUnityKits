using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZToolKit
{
    internal class AudMgr : SingletonDontDestroy<AudMgr>
    {
        public AudioSource musicSource;
        public AudioSource sfxSource;

        public float MusicVol => musicSource.volume; 
        public float SfxVol => sfxSource.volume; 
        
        public bool IsActive
        {
            get => mIsActive;

            set => mIsActive = value;
        }
        
        private bool mIsActive = true;
        
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
            if (Instance.mIsActive)
            {
                Instance.musicSource.clip = ResTool.Load<AudioClip>(clipName);
                Instance.musicSource.Play();
            }
        }

        public static void PlaySfx(string clipName)
        {
            if (Instance.mIsActive)
            {
                Instance.sfxSource.PlayOneShot(ResTool.Load<AudioClip>(clipName));
            }
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
