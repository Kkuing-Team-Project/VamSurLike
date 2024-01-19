using UnityEngine;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public AudioSource audioSource;
    public AudioSource MoveAudioSource;
    private Dictionary<string, AudioClip> soundClipDictionary;

    public Transform playerTransform; // 플레이어 또는 카메라의 위치
    public float maxVolumeDistance = 5f;
    public float minVolumeDistance = 20f;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeSoundDictionary();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeSoundDictionary()
    {
        soundClipDictionary = new Dictionary<string, AudioClip>();
    }

    public void PlaySound(string clipName, bool isMoveSound = false, Vector3 position = default, bool loop = false)
    {
        if (!soundClipDictionary.TryGetValue(clipName, out AudioClip clip))
        {
            clip = GetSound(clipName);
        }

        if (clip != null)
        {
            if (isMoveSound)
            {
                if (MoveAudioSource != null) // MoveAudioSource의 null 여부를 체크
                {
                    MoveAudioSource.clip = clip;
                    MoveAudioSource.loop = loop;
                    MoveAudioSource.Play();
                    UpdateMoveSoundPosition(position); // 이동 사운드 위치 업데이트
                }
            }
            else
            {
                // 일반 사운드 처리
                audioSource.PlayOneShot(clip);
            }
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
        MoveAudioSource.volume = Mathf.Clamp01(1 - (distance - maxVolumeDistance) / (minVolumeDistance - maxVolumeDistance));
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