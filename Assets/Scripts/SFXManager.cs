using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    static public SFXManager instance;
    public Dictionary<string, AudioClip> clips = new Dictionary<string, AudioClip>();
    public List<AudioSource> audioSources;

    private void Awake()
    {
        if (instance)
        {
            Destroy(instance.gameObject);
        }

        instance = this;
    }

    public void PlaySFX(string fileName)
    {
        if (!clips.ContainsKey(fileName))
        {
            AudioClip clip = (AudioClip)Resources.Load("Audio/Break.ogg");
            if (!clip)
            {
                Debug.Log("Could not find Audio/" + fileName);
                return;
            }
            clips.Add(fileName, clip);
        }

        bool found = false;
        foreach (AudioSource source in audioSources)
        {
            if (source && !source.isPlaying)
            {
                found = true;
                source.PlayOneShot(clips[fileName]);
                break;
            }
        }

        if (!found)
        {
            AudioSource source = new AudioSource();
            source.clip = clips[fileName];
            source.PlayOneShot(clips[fileName]);
            audioSources.Add(source);
        }
    }
}
