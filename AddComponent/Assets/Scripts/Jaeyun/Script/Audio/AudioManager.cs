using System;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace UnityTemplateProjects.Jaeyun.Script.Audio
{
    public class AudioManager : MonoBehaviour
    {
        public int bgmCount;
        public int sfxCount;

        private List<AudioSource> bgms = new List<AudioSource>();
        private List<AudioSource> sfxs = new List<AudioSource>();

        private void Awake()
        {
            for (int i = 0; i < bgmCount; i++)
            {
                bgms.Add(MakeAudioSource(true));
            }

            for (int i = 0; i < sfxCount; i++)
            {
                sfxs.Add(MakeAudioSource(false));   
            }
        }

        public void PlayBgm(AudioClip clip)
        {
            var audioSource = GetPlayableAudio(bgms);
            audioSource.clip = clip;
            audioSource.Play();
        }
        
        public void PlaySfx(AudioClip clip)
        {
            var audioSource = GetPlayableAudio(sfxs);
            audioSource.clip = clip;
            audioSource.Play();
        }

        public void StopAllBgm()
        {
            foreach (var audioSource in bgms)
            {
                audioSource.Stop();
            }
        }


        private AudioSource MakeAudioSource(bool isLoop)
        {
            var result = gameObject.AddComponent<AudioSource>();
            result.loop = isLoop;
            
            return result;
        }

        private AudioSource GetPlayableAudio(List<AudioSource> audioSources)
        {
            foreach (var audioSource in audioSources)
            {
                if (!audioSource.isPlaying)
                    return audioSource;
            }

            return audioSources[0];
        }
    }
}