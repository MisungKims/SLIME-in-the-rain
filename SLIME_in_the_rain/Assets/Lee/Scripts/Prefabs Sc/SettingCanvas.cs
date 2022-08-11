using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class SettingCanvas : MonoBehaviour
{

    #region 변수
    public AudioMixer sound;        //오디오 관리 믹서
    bool isOn;

    //마스터
    public Slider masterSlider;
    public Toggle masterToggle;
    //BGM
    public Slider bgmSlider;
    public Toggle bgmToggle;
    //SFX
    public Slider sfxSlider;
    public Toggle sfxToggle;
    #endregion

    #region 유니티 함수

    private void Start()
    {
        //슬라이더 초기설정
        masterSlider.onValueChanged.AddListener(delegate { AudioControl(masterSlider, "Master"); });
        bgmSlider.onValueChanged.AddListener(delegate { AudioControl(bgmSlider, "BGM"); });
        sfxSlider.onValueChanged.AddListener(delegate { AudioControl(sfxSlider, "SFX"); });

        //토글 초기설정
        masterToggle.GetComponent<Toggle>().onValueChanged.AddListener(delegate { Toggle(masterToggle, "Master"); });
        bgmToggle.GetComponent<Toggle>().onValueChanged.AddListener(delegate { Toggle(bgmToggle, "BGM"); });
        sfxToggle.GetComponent<Toggle>().onValueChanged.AddListener(delegate { Toggle(sfxToggle, "SFX"); });

    }
    private void OnEnable()
    {

        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)     //씬 시작시 불러오기
    {
        //슬라이더 값 불러오기
        masterSlider.value = PlayerPrefs.GetFloat("Master" + "sound");
        AudioControl(masterSlider, "Master");
        bgmSlider.value = PlayerPrefs.GetFloat("BGM" + "sound");
        AudioControl(masterSlider, "BGM");
        sfxSlider.value = PlayerPrefs.GetFloat("SFX" + "sound");
        AudioControl(masterSlider, "SFX");

        //토글 값 불러오기
        masterToggle.isOn = System.Convert.ToBoolean(PlayerPrefs.GetInt("Master"));
        masterToggle.isOn = System.Convert.ToBoolean(PlayerPrefs.GetInt("BGM"));
        masterToggle.isOn = System.Convert.ToBoolean(PlayerPrefs.GetInt("SFX"));
    }
    private void OnDisable()        //씬 종료시 저장
    {
        //슬라이더 값 저장
        PlayerPrefs.SetFloat("Master" + "sound", masterSlider.value);
        PlayerPrefs.SetFloat("BGM" + "sound", bgmSlider.value);
        PlayerPrefs.SetFloat("SFX" + "sound", sfxSlider.value);

        //토글 값 저장
        PlayerPrefs.SetInt("Master" + "toggle", System.Convert.ToInt32(masterToggle.isOn));
        PlayerPrefs.SetInt("BGM" + "toggle", System.Convert.ToInt32(bgmToggle.isOn));
        PlayerPrefs.SetInt("SFX" + "toggle", System.Convert.ToInt32(sfxToggle.isOn));

        SceneManager.sceneLoaded -= OnSceneLoaded;
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
    public void Toggle(Toggle toggle,string str)
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
