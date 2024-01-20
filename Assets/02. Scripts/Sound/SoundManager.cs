using UnityEngine;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    
    public AudioSource audioSource; // 기존의 audioSource를 효과음용으로 사용
    public AudioSource bgmAudioSource; // BGM용 AudioSource 추가

    private Dictionary<string, AudioClip> soundClipDictionary = new Dictionary<string, AudioClip>();

    public Transform playerTransform;
    public float maxVolumeDistance = 5f;
    public float minVolumeDistance = 20f;

    private bool isMoveSoundActive = false;
    private AudioSource currentMoveAudioSource;
    private string currentLayerName;
    private float currentMaxVolume;

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

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    
        if (bgmAudioSource == null)
        {
            bgmAudioSource = gameObject.AddComponent<AudioSource>();
            bgmAudioSource.loop = true; // BGM은 일반적으로 루프되어야 함
        }
    }


    public void PlaySound(string clipName, bool isBGM = false,  bool isMoveSound = false, string LayerName = default, bool loop = false, float volume = 0.5f)
    {
        AudioClip clip;
        if (!soundClipDictionary.TryGetValue(clipName, out clip))
        {
            clip = GetSound(clipName);
        }

        if (playerTransform == null && isMoveSound)
        {

            GameObject player = FindPlayerByLayer(LayerMask.NameToLayer("PLAYER"));
            if (player != null)
            {
                playerTransform = player.transform;
            }
            else
            {
                Debug.LogError("Player object not found");
                return;
            }
        }

        if (clip == null)
        {
            Debug.LogWarning("Sound not found: " + clipName);
            return;
        }

        AudioSource targetAudioSource = isMoveSound ? FindAndPrepareMoveAudioSource(LayerName, clip, loop, volume) : (isBGM ? bgmAudioSource : audioSource);
        if (targetAudioSource == null) return;

        targetAudioSource.clip = clip;
        targetAudioSource.loop = loop || isBGM; // BGM은 항상 루프
        targetAudioSource.volume = volume;
        targetAudioSource.Play();

        if (isMoveSound)
        {
            currentLayerName = LayerName;
            currentMaxVolume = volume;
            currentMoveAudioSource = targetAudioSource;
            isMoveSoundActive = true;
        }
    }

    private AudioSource FindAndPrepareMoveAudioSource(string layerName, AudioClip clip, bool loop, float volume)
    {
        GameObject moveObject = FindPlayerByLayer(LayerMask.NameToLayer(layerName));
        if (moveObject == null)
        {
            Debug.LogWarning("Object with Layer '" + layerName + "' not found.");
            return null;
        }

        AudioSource moveAudioSource = moveObject.GetComponent<AudioSource>();
        if (moveAudioSource == null)
        {
            moveAudioSource = moveObject.AddComponent<AudioSource>();
        }

        return moveAudioSource;
    }

    private void UpdateMoveSoundPosition(string layerName, AudioSource moveAudioSource, float maxVolume)
    {
        if (playerTransform == null)
        {
            Debug.LogWarning("PlayerTransform is not set");
            return;
        }

        GameObject moveObject = FindPlayerByLayer(LayerMask.NameToLayer(layerName));
        if (moveObject == null)
        {
            Debug.LogWarning("Move object with layer '" + layerName + "' not found.");
            return;
        }

        Vector3 position = moveObject.transform.position;
        float distance = Vector3.Distance(playerTransform.position, position);
        float volume;

        if (distance <= maxVolumeDistance)
        {
            volume = maxVolume;
        }
        else if (distance >= minVolumeDistance)
        {
            volume = 0f;
        }
        else
        {
            volume = maxVolume * (1 - (distance - maxVolumeDistance) / (minVolumeDistance - maxVolumeDistance));
        }

        moveAudioSource.volume = Mathf.Clamp01(volume);
    }
    
    void Update()
    {
        if (isMoveSoundActive)
        {
            UpdateMoveSoundPosition(currentLayerName, currentMoveAudioSource, currentMaxVolume);
        }
}


    public void StopBaseAudio()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    public AudioClip GetSound(string key)
    {
        AudioClip clip;
        if (!soundClipDictionary.TryGetValue(key, out clip))
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

    private GameObject FindPlayerByLayer(int layer)
    {
        GameObject[] gameObjects = FindObjectsOfType<GameObject>();
        foreach (var obj in gameObjects)
        {
            if (obj.layer == layer)
            {
                return obj;
            }
        }
        return null;
    }
}
