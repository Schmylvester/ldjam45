using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class AudioManager : MonoBehaviour
    {
        [System.Serializable]
        public struct AudioBGM
        {
            public string id;
            public AudioClip intro;
            public AudioClip loop;
        }

        [SerializeField] private AudioBGM[] m_tracks;
        [SerializeField] private AudioSource m_audioSource;
        int m_activeTrackIdx = -1;

        private void Start()
        {
            playMusic(Random.Range(0,2));
        }

        public void playMusic(int idx)
        {
            m_activeTrackIdx = idx;
            float introLength = 0;
            if (m_tracks[idx].intro)
            {
                m_audioSource.clip = m_tracks[idx].intro;
                introLength = m_tracks[idx].intro.length;
                m_audioSource.Play();
            }
            StartCoroutine(introEnd(introLength));
        }

        public void playMusic(string id)
        {
            for(int i = 0; i < m_tracks.Length; ++i)
            {
                if(m_tracks[i].id == id)
                {
                    playMusic(i);
                }
            }
        }

        private IEnumerator introEnd(float time)
        {
            yield return new WaitForSeconds(time);
            m_audioSource.clip = m_tracks[m_activeTrackIdx].loop;
            m_audioSource.loop = true;
            m_audioSource.Play();
        }
    }
}