using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound3D : MonoBehaviour
{
    public void Play3DSound(AudioClip clip)
    {
        AudioSource source = GetComponent<AudioSource>();
        if (source != null && clip != null)
        {
            source.clip = clip;
            source.Play();
        }
        else
        {
            Debug.LogError("AudioSource or AudioClip is missing");
        }
    }
}
