using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class SoundManager : MonoBehaviour
{

    #region ����
    public AudioMixer sound;        //����� ���� �ͼ�
    bool isOn;
    //������
    public Slider masterAudioSlider;   
    public Toggle masterToggle;
    #endregion

    #region ����Ƽ �Լ�(Start)
    private void Start()
    {
        //�����̴� �ʱ⼳��
        masterAudioSlider.GetComponent<Slider>().onValueChanged.AddListener(delegate { MasterAudioControl(); });

        //��� �ʱ⼳��
        masterToggle.GetComponent<Toggle>().onValueChanged.AddListener(delegate { Toggle(); });
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
    public void MasterAudioControl()
    {
        float volume = masterAudioSlider.value;
        if (volume == -40f) sound.SetFloat("Master", -80);     //�Ҹ� �ʹ� ũ�� ������ �ŷ��� ����
        else sound.SetFloat("Master", volume);
        SliderVolumeText(masterAudioSlider, volume);
    }

    //����� ��� �Լ�
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
