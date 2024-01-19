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

    // 배경음악
    public void LobbySound() => PlaySound("Sound_BG_Title"); // 1. -> 로비 배경 음악 : 게임 실행 이후 타이틀 화면에 출력 (loop)
    public void StageSound()=> PlaySound("Sound_BG_Stage_Choice"); // 2. -> 스테이지 선택 화면 배경 음악 : 스테이지 선택 화면 진입 시 출력 (loop)
    public void InGameSound()=> PlaySound("Sound_BG_Battle01"); // 3. -> 인게임 배경음악 : 인게임 진입 시 출력 (01 종료 이후 02 출력, 이후 loop)
    public void InGameSound2()=> PlaySound("Sound_BG_Battle02"); // 4. -> 인게임 배경음악 : 인게임 진입 시 출력 ((01 종료 이후 02 출력, 이후 loop)


    // 캐릭터 효과음
    public void PlaySpawnSound() => PlaySound("Sound_EF_CH_Spawn"); // 5. -> 인게임 스타트 사운드 : 인게임 진입 시 1회 출력
    public void CrashSound() => PlaySound("Sound_EF_CH_Shield"); // 6. -> 캐릭터 충돌 사운드 : 캐릭터가 적과 충돌하거나 적의 공격에 적중했을 때 1회 출력
    public void PlayHitSound() => PlaySound("Sound_EF_CH_Hit"); // 7. -> 캐릭터 일반 공격 사운드 : 캐릭터가 적에게 공격을 실행했을 때 1회 출력
    public void PlayDeathSound() => PlaySound("Sound_EF_CH_Death"); // 8. -> 게임 오버 사운드 : 게임 오버 조건 달성 시 1회 출력 (캐릭터, 영혼 HP =< 0 or 붕괴존 안정화 실패)
    public void EFSound() => PlaySound("Sound_EF_CH_EXP02"); // 9. -> 경험치 획득 사운드 : 캐릭터가 경험치 획득 시 1회 출력
    public void PlayFireSkillSound() => PlaySound("Sound_EF_CH_Skill_Fire"); // 10. -> 불 스킬 사운드 : 불 속성 캐릭터가 스킬 사용 시 1회 출력
    public void PlayIceSkillSound() => PlaySound("Sound_EF_CH_Skill_Ice"); // 11. -> 얼음 스킬 사운드 : 얼음 속성 캐릭터가 스킬 사용 시 1회 출력
    public void SouPlayIceSkillSoundnd() => PlaySound("Sound_EF_CH_Skill_Ice"); // 12. -> 바람 스킬 사운드 : 바람 속성 캐릭터가 스킬 사용 시 1회 출력


    // 영혼 효과음
    public void PlaySoulSound() => PlaySound("Sound_EF_SP"); // 13. -> 3D사운드 : 영혼에게 고정되어 loop 출력


    // 증강 효과음
    public void PlayMeteorSound() => PlaySound("Sound_EF_UP_Meteor"); // 14. -> 3D사운드 : 소형 메테오 공격 범위에 1회 출력 (1초의 딜레이가 필요할 수 있음)
    public void PlayNukeSound() => PlaySound("Sound_EF_UP_Nuke"); // 15. -> 소멸 마법 : 소멸 마법 시행 시 1회 출력
    public void PlayPoisonSound() => PlaySound("Sound_EF_UP_Poison"); // 16. -> 3D사운드 : 독 장판 내에 적이 존재할 시 loop 출력

    
    // UI 효과음
    public void LevelSound() => PlaySound("Sound_UI_LevelUP"); // 17. -> 레벨업 UI : 레벨업 이후 증강 선택 UI가 출력됨과 동시에 1회 출력
    public void SelectSound() => PlaySound("Sound_UI_UP_Select"); // 18. -> 증강 선택 UI : 증강을 선택(터치)할 시 1회 출력
}