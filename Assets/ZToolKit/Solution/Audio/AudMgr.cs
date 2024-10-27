using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace ZToolKit
{
    internal class AudMgr : SingletonDontDestroy<AudMgr>
    {
        public AudioSource musicSource;
        public AudioSource sfxSource;
        public AudioSource testSource;

        public float MusicVol => musicSource.volume;
        public float SfxVol => sfxSource.volume;

        public bool IsActive
        {
            get => mIsActive;

            set
            {
                mIsActive = value;
                musicSource.Stop();
            }
        }

        private bool mIsActive = true;

        protected override void OnAwake()
        {
            musicSource.loop = true;
            sfxSource.loop = false;
            testSource.loop = false;
        }

        protected override void OnStart()
        {

        }

        public static void PlayMusic(string clipName)
        {
            if (Instance.mIsActive)
            {
                var vol = Instance.musicSource.volume;
                Instance.musicSource.volume = 0;

                var clip = ResTool.Load<AudioClip>(clipName);

                if (clip)
                {
                    Instance.musicSource.clip = clip;
                    Instance.musicSource.Play();

                    DOTween.To(() => Instance.musicSource.volume, x => Instance.musicSource.volume = x, vol, vol * 2);
                }
                else
                {
                    LogTool.Error("Audio", @$"""{clipName}"" Load Failed");
                }
            }
        }

        public static void PlaySfx(string clipName)
        {
            if (Instance.mIsActive)
            {
                var clip = ResTool.Load<AudioClip>(clipName);

                if (clip)
                {
                    Instance.sfxSource.PlayOneShot(clip);
                }
                else
                {
                    LogTool.Error("Audio", @$"""{clipName}"" Load Failed");
                }
            }
        }

        public static void PlayTestAudio(string clipName, float value)
        {
            if (Instance.mIsActive)
            {
                Instance.testSource.Stop();
                Instance.testSource.volume = value;
                Instance.testSource.clip =ResTool.Load<AudioClip>(clipName);
                Instance.testSource.Play();
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
