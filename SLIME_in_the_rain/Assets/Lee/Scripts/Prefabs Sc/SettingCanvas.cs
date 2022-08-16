using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Threading.Tasks;

public class SettingCanvas : MonoBehaviour
{

    #region 변수
    #region 싱글톤
    private static SettingCanvas instance = null;
    public static SettingCanvas Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }
    #endregion
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


    void Awake()
    {
        if (null == instance)
        {
            instance = this;

            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)     //씬 시작시 불러오기
    {
        DelayedUpdateVolume();
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    private void Start()
    {
        //슬라이더 토글 설정
        //슬라이더
        masterSlider.onValueChanged.AddListener(delegate { AudioControl("Master", masterSlider, masterToggle); });
        bgmSlider.onValueChanged.AddListener(delegate { AudioControl("BGM", bgmSlider, bgmToggle); });
        sfxSlider.onValueChanged.AddListener(delegate { AudioControl("SFX", sfxSlider, sfxToggle); });

        //토글
        masterToggle.GetComponent<Toggle>().onValueChanged.AddListener(delegate { Toggle("Master", masterSlider, masterToggle); });
        bgmToggle.GetComponent<Toggle>().onValueChanged.AddListener(delegate { Toggle("BGM", bgmSlider, bgmToggle); });
        sfxToggle.GetComponent<Toggle>().onValueChanged.AddListener(delegate { Toggle("SFX", sfxSlider, sfxToggle); });
    }
    #endregion


    #region 함수
    async void DelayedUpdateVolume()
    {
        await Task.Delay(1);
        UpdateVolume();
    }
    void UpdateVolume()     //AudioMixer의 SetFloat가 제대로 작동 안해서 딜레이 주고 실행 되게 해야함
    {
        //Load
        //슬라이더 값 불러오기
        masterSlider.value = PlayerPrefs.GetFloat("Master" + "sound");
        bgmSlider.value = PlayerPrefs.GetFloat("BGM" + "sound");
        sfxSlider.value = PlayerPrefs.GetFloat("SFX" + "sound");

        AudioControl("Master", masterSlider, masterToggle);
        AudioControl("BGM", bgmSlider, bgmToggle);
        AudioControl("SFX", sfxSlider, sfxToggle);

        //토글 값 불러오기
        masterToggle.isOn = System.Convert.ToBoolean(PlayerPrefs.GetInt("Master" + "toggle"));
        bgmToggle.isOn = System.Convert.ToBoolean(PlayerPrefs.GetInt("BGM" + "toggle"));
        sfxToggle.isOn = System.Convert.ToBoolean(PlayerPrefs.GetInt("SFX" + "toggle"));

        Toggle("Master", masterSlider, masterToggle);
        Toggle("BGM", bgmSlider, bgmToggle);
        Toggle("SFX", sfxSlider, sfxToggle);
    }


    //오디오 슬라이더 함수
    public void AudioControl(string str, Slider slider,Toggle toggle)
    {
        //슬라이더 소리 설정
        float volume = slider.value;
        
        //음소거가 아닐시에만
        if (!toggle.isOn)
        {
            if (volume == -40f)
            {
                sound.SetFloat(str, -80);     //소리 너무 크면 지지직 거려서 제한
            }
            else
            {
                sound.SetFloat(str, volume);
            }   
        }
        SliderVolumeText(slider, volume);
        PlayerPrefs.SetFloat(str + "sound", slider.value);       //Save
    }

    //슬라이더 텍스트에 값 넘기는 함수
    public void SliderVolumeText(Slider slider, float volume)
    {
        slider.transform.GetChild(2).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text
            = ((int)((volume + 40) / 40 * 100)).ToString();
    }

    //오디오 토글 함수
    public void Toggle(string str, Slider slider, Toggle toggle)
    {
        isOn = toggle.GetComponent<Toggle>().isOn;      //음소거 on
        if (isOn)
        {
            toggle.transform.GetChild(2).transform.GetComponent<Image>().enabled = true;

            if (str == "Master")
            {
                sound.FindMatchingGroups("Master")[0].audioMixer.SetFloat(str, -80f);
            }
            else if (str == "BGM")
            {
                sound.FindMatchingGroups("Master")[1].audioMixer.SetFloat(str, -80f);
            }
            else if (str == "SFX")
            {
                sound.FindMatchingGroups("Master")[2].audioMixer.SetFloat(str, -80f);
            }
        }
        else
        {
            toggle.transform.GetChild(2).transform.GetComponent<Image>().enabled = false;
            AudioControl(str, slider, toggle);
        }
        PlayerPrefs.SetInt(str + "toggle", System.Convert.ToInt32(toggle.isOn));     //Save
    }

    #endregion

}
