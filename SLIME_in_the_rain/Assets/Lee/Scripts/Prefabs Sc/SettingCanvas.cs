using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class SettingCanvas : MonoBehaviour
{

    #region ����
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

    private void Start()
    {
        //�����̴� �ʱ⼳��
        masterSlider.onValueChanged.AddListener(delegate { AudioControl(masterSlider, "Master"); });
        bgmSlider.onValueChanged.AddListener(delegate { AudioControl(bgmSlider, "BGM"); });
        sfxSlider.onValueChanged.AddListener(delegate { AudioControl(sfxSlider, "SFX"); });

        //��� �ʱ⼳��
        masterToggle.GetComponent<Toggle>().onValueChanged.AddListener(delegate { Toggle(masterToggle, "Master"); });
        bgmToggle.GetComponent<Toggle>().onValueChanged.AddListener(delegate { Toggle(bgmToggle, "BGM"); });
        sfxToggle.GetComponent<Toggle>().onValueChanged.AddListener(delegate { Toggle(sfxToggle, "SFX"); });

    }
    private void OnEnable()
    {

        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)     //�� ���۽� �ҷ�����
    {
        //�����̴� �� �ҷ�����
        masterSlider.value = PlayerPrefs.GetFloat("Master" + "sound");
        AudioControl(masterSlider, "Master");
        bgmSlider.value = PlayerPrefs.GetFloat("BGM" + "sound");
        AudioControl(masterSlider, "BGM");
        sfxSlider.value = PlayerPrefs.GetFloat("SFX" + "sound");
        AudioControl(masterSlider, "SFX");

        //��� �� �ҷ�����
        masterToggle.isOn = System.Convert.ToBoolean(PlayerPrefs.GetInt("Master"));
        masterToggle.isOn = System.Convert.ToBoolean(PlayerPrefs.GetInt("BGM"));
        masterToggle.isOn = System.Convert.ToBoolean(PlayerPrefs.GetInt("SFX"));
    }
    private void OnDisable()        //�� ����� ����
    {
        //�����̴� �� ����
        PlayerPrefs.SetFloat("Master" + "sound", masterSlider.value);
        PlayerPrefs.SetFloat("BGM" + "sound", bgmSlider.value);
        PlayerPrefs.SetFloat("SFX" + "sound", sfxSlider.value);

        //��� �� ����
        PlayerPrefs.SetInt("Master" + "toggle", System.Convert.ToInt32(masterToggle.isOn));
        PlayerPrefs.SetInt("BGM" + "toggle", System.Convert.ToInt32(bgmToggle.isOn));
        PlayerPrefs.SetInt("SFX" + "toggle", System.Convert.ToInt32(sfxToggle.isOn));

        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    #endregion


    #region �Լ�
    //�����̴� �ؽ�Ʈ�� �� �ѱ�� �Լ�
    public void SliderVolumeText(Slider slider, float sound)
    {
        slider.transform.GetChild(2).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text
            = ((int)((sound + 40) / 40 * 100)).ToString();
    }

    //������ ���� ���� �Լ�
    public void AudioControl(Slider slider,string str)
    {
        float volume = slider.value;
        if (volume == -40f) sound.SetFloat(str, -80);     //�Ҹ� �ʹ� ũ�� ������ �ŷ��� ����
        else sound.SetFloat(str, volume);
        SliderVolumeText(slider, volume);
    }

    //����� ��� �Լ�
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
