using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;

namespace WargameSystem.MenuSystem
{
    public class SettingsUIManager : MonoBehaviour
    {
        public AudioMixer masterMixer; // 오디오 믹서 객체를 저장하는 변수
        public float masterLvl; // 마스터 레벨 변수

        public void SetFullScreen(bool isFullScreen)
        {
            Screen.fullScreen = isFullScreen; // 전체 화면 여부를 설정하는 함수
        }

        public void SetQuality(int qualityIndex)
        {
            QualitySettings.SetQualityLevel(qualityIndex); // 화질 설정을 변경하는 함수
        }

        public void SetVolume(float volume)
        {
            masterMixer.SetFloat("Volume", volume); // 마스터 믹서의 볼륨 파라미터를 조절하는 함수
        }
    }
}
