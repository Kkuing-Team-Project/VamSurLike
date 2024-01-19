using UnityEngine;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public AudioSource audioSource;
    private Dictionary<string, AudioClip> soundClipDictionary;

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

    private void PlaySound(string clipName)
    {
        if (!soundClipDictionary.TryGetValue(clipName, out AudioClip clip))
        {
            clip = GetSound(clipName);
        }

        if (clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning("Sound not found: " + clipName);
        }
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

    public void PlayFireAttackSound() => PlaySound("Sound_EF_CH_Atk_Fire"); // FirePlayer가 일반 공격을 실행 시 출력되는 사운드입니다.
    public void PlayIceAttackSound() => PlaySound("Sound_EF_CH_Atk_Ice"); // IcePlayer가 일반 공격을 실행 시 출력되는 사운드입니다.
    public void PlayWindAttackSound() => PlaySound("Sound_EF_CH_Atk_Wind"); // WindPlayer가 일반 공격을 실행 시 출력되는 사운드입니다.
    public void PlayDeathSound() => PlaySound("Sound_EF_CH_Death"); // 게임 오버 시 출력되는 사운드입니다.
    public void PlayExpSound() => PlaySound("Sound_EF_CH_EXP"); // 경험치 획득 시 출력되는 사운드입니다.
    public void PlayHitSound() => PlaySound("Sound_EF_CH_Hit"); // 캐릭터가 적과 충돌하거나 공격을 받을 시 출력되는 사운드입니다.
    public void PlayFireSkillSound() => PlaySound("Sound_EF_CH_Skill_Fire"); // FirePlayer가 스킬을 사용할 시 출력되는 사운드입니다.
    public void PlayIceSkillSound() => PlaySound("Sound_EF_CH_Skill_Ice"); // IcePlayer가 스킬을 사용할 시 출력되는 사운드입니다.
    public void PlayWindSkillSound() => PlaySound("Sound_EF_CH_Skill_Wind"); // WindPlayer가 스킬을 사용할 시 출력되는 사운드입니다.
    public void PlaySpawnSound() => PlaySound("Sound_EF_CH_Spawn"); // * 게임 시작 직후, 캐릭터가 소환됨을 알리면서 출력되는 사운드입니다.
    public void PlayEnemyHitSound() => PlaySound("Sound_EF_Enemy_Hit02"); // * 적 몬스터가 캐릭터의 공격에 피격되었을 때 출력되는 사운드입니다.
    public void PlaySoulSound() => PlaySound("Sound_EF_SP"); // * 게임 내에 출현하는 영혼 오브젝트에서 출력되는 사운드입니다. 3D
    public void PlayMeteorSound() => PlaySound("Sound_EF_UP_Meteor"); // 소형 메테오에서 출력되는 사운드입니다. 메테오가 적을 공격했을 때(피격 판정) 출력)
    public void PlayNukeSound() => PlaySound("Sound_EF_UP_Nuke"); // 소멸 마법에서 출력되는 사운드입니다.
    public void PlayPoisonSound() => PlaySound("Sound_EF_UP_Poison"); // 3D 독 장판에서 출력되는 사운드입니다. (장판이 소멸할 때까지 loop됩니다.)
    public void PlayShieldSound() => PlaySound("Sound_EF_UP_Shield"); // 쉴드에서 출력되는 사운드입니다. + 쉴드에서 출력되는 사운드입니다.
    public void PlayZoneStartSound() => PlaySound("Sound_EF_Zone_Start"); // 맵 내에 붕괴존이 생성되었을 때 출력되는 사운드입니다.
    public void PlayZoneEndSound() => PlaySound("Sound_EF_Zone_End"); // 붕괴존이 소멸되었을 때 출력되는 사운드입니다.
}
