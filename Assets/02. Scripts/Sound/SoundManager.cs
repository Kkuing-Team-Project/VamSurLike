using UnityEngine;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public AudioSource audioSource;
    public AudioSource moveAudioSource;
    private Dictionary<string, AudioClip> soundClipDictionary = new Dictionary<string, AudioClip>();

    public Transform playerTransform; // 플레이어 또는 카메라의 위치
    public float maxVolumeDistance = 5f;
    public float minVolumeDistance = 20f;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySound(string clipName, bool isMoveSound = false, Vector3 position = default, bool loop = false)
    {
        if (!soundClipDictionary.TryGetValue(clipName, out AudioClip clip))
        {
            clip = GetSound(clipName);
        }

        if (clip != null)
        {
            AudioSource targetAudioSource = isMoveSound ? moveAudioSource : audioSource;
            targetAudioSource.clip = clip;
            targetAudioSource.loop = loop;

            if (isMoveSound)
            {
                UpdateMoveSoundPosition(position); // 이동 사운드 위치 업데이트
            }
            targetAudioSource.Play();
        }
        else
        {
            Debug.LogWarning("Sound not found: " + clipName);
        }
    }

    public void PlayOneShot(string clipName, bool isMoveSound = false, Vector3 position = default)
    {
        if (!soundClipDictionary.TryGetValue(clipName, out AudioClip clip))
        {
            clip = GetSound(clipName);
        }

        if (clip != null)
        {
            AudioSource targetAudioSource = isMoveSound ? moveAudioSource : audioSource;

            if (isMoveSound)
            {
                UpdateMoveSoundPosition(position); // 이동 사운드 위치 업데이트
            }
            targetAudioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning("Sound not found: " + clipName);
        }
    }

    private void UpdateMoveSoundPosition(Vector3 position)
    {
        // 이 메서드에서 MoveAudioSource의 볼륨을 조절
        float distance = Vector3.Distance(playerTransform.position, position);
        moveAudioSource.volume = Mathf.Clamp01(1 - (distance - maxVolumeDistance) / (minVolumeDistance - maxVolumeDistance));
    }

    public void StopBackgroundMusic()
    {
        audioSource.Stop();
    }

    public AudioClip GetSound(string key)
    {
        if (!soundClipDictionary.TryGetValue(key, out AudioClip clip))
        {
            clip = Resources.Load<AudioClip>("AudioClips/" + key);
            if (clip != null)
            {
                soundClipDictionary.Add(key, clip);
            }
            else
            {
                Debug.LogError("Failed to load sound: " + key);
            }
        }
        return clip;
    }
}