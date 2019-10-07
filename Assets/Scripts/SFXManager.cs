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

    public void PlaySFX(string fileName, float volumeScale = 0.001f)
    {
        if (!clips.ContainsKey(fileName))
        {
            AudioClip clip = (AudioClip)Resources.Load("Audio/" + fileName);
            if (!clip)
            {
                Debug.Log("Could not find Audio/" + fileName);
                return;
            }
            clips.Add(fileName, clip);
        }

        AudioSource src = null;
        foreach (AudioSource source in audioSources)
        {
            if (source && !source.isPlaying)
            {
                src = source;
                break;
            }
        }

        if (src == null)
        {
            src = gameObject.AddComponent<AudioSource>();
            audioSources.Add(src);
        }

        src.PlayOneShot(clips[fileName], volumeScale);
    }
}
