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

    #region ����
    #region �̱���
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
    public AudioMixer sound;        //����� ���� �ͼ�
    bool isOn;

    //������
    public Slider masterSlider;
    public Toggle masterToggle;
    //BGM
    public Slider bgmSlider;
    public Toggle bgmToggle;
    //SFX
    public Slider sfxSlider;
    public Toggle sfxToggle;
    #endregion

    #region ����Ƽ �Լ�


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
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)     //�� ���۽� �ҷ�����
    {
        DelayedUpdateVolume();
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    private void Start()
    {
        //�����̴� ��� ����
        //�����̴�
        masterSlider.onValueChanged.AddListener(delegate { AudioControl("Master", masterSlider, masterToggle); });
        bgmSlider.onValueChanged.AddListener(delegate { AudioControl("BGM", bgmSlider, bgmToggle); });
        sfxSlider.onValueChanged.AddListener(delegate { AudioControl("SFX", sfxSlider, sfxToggle); });

        //���
        masterToggle.GetComponent<Toggle>().onValueChanged.AddListener(delegate { Toggle("Master", masterSlider, masterToggle); });
        bgmToggle.GetComponent<Toggle>().onValueChanged.AddListener(delegate { Toggle("BGM", bgmSlider, bgmToggle); });
        sfxToggle.GetComponent<Toggle>().onValueChanged.AddListener(delegate { Toggle("SFX", sfxSlider, sfxToggle); });
    }
    #endregion


    #region �Լ�
    async void DelayedUpdateVolume()
    {
        await Task.Delay(1);
        UpdateVolume();
    }
    void UpdateVolume()     //AudioMixer�� SetFloat�� ����� �۵� ���ؼ� ������ �ְ� ���� �ǰ� �ؾ���
    {
        //Load
        //�����̴� �� �ҷ�����
        masterSlider.value = PlayerPrefs.GetFloat("Master" + "sound");
        bgmSlider.value = PlayerPrefs.GetFloat("BGM" + "sound");
        sfxSlider.value = PlayerPrefs.GetFloat("SFX" + "sound");

        AudioControl("Master", masterSlider, masterToggle);
        AudioControl("BGM", bgmSlider, bgmToggle);
        AudioControl("SFX", sfxSlider, sfxToggle);

        //��� �� �ҷ�����
        masterToggle.isOn = System.Convert.ToBoolean(PlayerPrefs.GetInt("Master" + "toggle"));
        bgmToggle.isOn = System.Convert.ToBoolean(PlayerPrefs.GetInt("BGM" + "toggle"));
        sfxToggle.isOn = System.Convert.ToBoolean(PlayerPrefs.GetInt("SFX" + "toggle"));

        Toggle("Master", masterSlider, masterToggle);
        Toggle("BGM", bgmSlider, bgmToggle);
        Toggle("SFX", sfxSlider, sfxToggle);
    }


    //����� �����̴� �Լ�
    public void AudioControl(string str, Slider slider,Toggle toggle)
    {
        //�����̴� �Ҹ� ����
        float volume = slider.value;
        
        //���ҰŰ� �ƴҽÿ���
        if (!toggle.isOn)
        {
            if (volume == -40f)
            {
                sound.SetFloat(str, -80);     //�Ҹ� �ʹ� ũ�� ������ �ŷ��� ����
            }
            else
            {
                sound.SetFloat(str, volume);
            }   
        }
        SliderVolumeText(slider, volume);
        PlayerPrefs.SetFloat(str + "sound", slider.value);       //Save
    }

    //�����̴� �ؽ�Ʈ�� �� �ѱ�� �Լ�
    public void SliderVolumeText(Slider slider, float volume)
    {
        slider.transform.GetChild(2).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text
            = ((int)((volume + 40) / 40 * 100)).ToString();
    }

    //����� ��� �Լ�
    public void Toggle(string str, Slider slider, Toggle toggle)
    {
        isOn = toggle.GetComponent<Toggle>().isOn;      //���Ұ� on
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
