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

    ////마스터
    //public Slider masterAudioSlider;   
    //public Toggle masterToggle;
    ////BGM
    //public Slider bgmAudioSlider;
    //public Toggle bgmToggle;
    ////SFX
    //public Slider sfxAudioSlider;
    //public Toggle sfxToggle;
    #endregion

    #region 유니티 함수(Start)
    private void Start()
    {
        Transform master = this.transform.Find("Master");
        Slider masterSlider = master.GetChild(1).GetComponent<Slider>();
        Toggle masterToggle = master.GetChild(2).GetComponent<Toggle>();

        Transform bgm = this.transform.Find("BGM");
        Slider bgmSlider = bgm.GetChild(1).GetComponent<Slider>();
        Toggle bgmToggle = bgm.GetChild(2).GetComponent<Toggle>();

        Transform sfx = this.transform.Find("SFX");
        Slider sfxSlider = sfx.GetChild(1).GetComponent<Slider>();
        Toggle sfxToggle = sfx.GetChild(2).GetComponent<Toggle>();


        //슬라이더 초기설정
        masterSlider.onValueChanged.AddListener(delegate { AudioControl(masterSlider, "Master"); });
        bgmSlider.onValueChanged.AddListener(delegate { AudioControl(bgmSlider, "BGM"); });
        sfxSlider.onValueChanged.AddListener(delegate { AudioControl(sfxSlider, "SFX"); });

        //토글 초기설정
        masterToggle.GetComponent<Toggle>().onValueChanged.AddListener(delegate { Toggle(masterToggle); });
        bgmToggle.GetComponent<Toggle>().onValueChanged.AddListener(delegate { Toggle(bgmToggle); });
        sfxToggle.GetComponent<Toggle>().onValueChanged.AddListener(delegate { Toggle(sfxToggle); });

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
    public void AudioControl(Slider slider,string str)
    {
        float volume = slider.value;
        if (volume == -40f) sound.SetFloat(str, -80);     //소리 너무 크면 지지직 거려서 제한
        else sound.SetFloat(str, volume);
        SliderVolumeText(slider, volume);
    }

    //오디오 토글 함수
    public void Toggle(Toggle toggle)
    {
        isOn = toggle.GetComponent<Toggle>().isOn;
        if (isOn)
        {
            toggle.transform.GetChild(1).gameObject.SetActive(true);
            toggle.transform.GetChild(2).gameObject.SetActive(false);
            AudioListener.volume = 1;
        }
        else
        {
            toggle.transform.GetChild(1).gameObject.SetActive(false);
            toggle.transform.GetChild(2).gameObject.SetActive(true);
            AudioListener.volume = 0;
        }
    }

    #endregion

}
