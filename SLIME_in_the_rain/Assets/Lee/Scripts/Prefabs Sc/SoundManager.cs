using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class SoundManager : MonoBehaviour
{

    #region 변수
    public AudioMixer sound;        //오디오 관리 믹서
    bool isOn;
    //마스터
    public Slider masterAudioSlider;   
    public Toggle masterToggle;
    #endregion

    #region 유니티 함수(Start)
    private void Start()
    {
        //슬라이더 초기설정
        masterAudioSlider.GetComponent<Slider>().onValueChanged.AddListener(delegate { MasterAudioControl(); });

        //토글 초기설정
        masterToggle.GetComponent<Toggle>().onValueChanged.AddListener(delegate { Toggle(); });
    }
    #endregion


    #region 함수
    //슬라이더 텍스트에 값 넘기는 함수
    public void SliderVolumeText(Slider slider, float sound)
    {
        slider.transform.GetChild(2).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text
            = ((int)((sound + 40) / 40 * 100)).ToString();
    }

    //마스터 볼륨 관련 함수
    public void MasterAudioControl()
    {
        float volume = masterAudioSlider.value;
        if (volume == -40f) sound.SetFloat("Master", -80);     //소리 너무 크면 지지직 거려서 제한
        else sound.SetFloat("Master", volume);
        SliderVolumeText(masterAudioSlider, volume);
    }

    //오디오 토글 함수
    public void Toggle()
    {
        isOn = masterToggle.GetComponent<Toggle>().isOn;
        if (isOn)
        {
            masterToggle.transform.GetChild(1).gameObject.SetActive(true);
            masterToggle.transform.GetChild(2).gameObject.SetActive(false);
            AudioListener.volume = 1;
        }
        else
        {
            masterToggle.transform.GetChild(1).gameObject.SetActive(false);
            masterToggle.transform.GetChild(2).gameObject.SetActive(true);
            AudioListener.volume = 0;
        }
    }
    #endregion

}
