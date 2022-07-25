using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class SettingButton : MonoBehaviour
{
    //사운드 컨트롤러 가져오기
    public AudioMixer sound;

    private Toggle masterToggle;


    #region 캔버스 on/off 함수
    void onCanvas()
    {  
        this.transform.GetChild(0).gameObject.SetActive(true);
    }

    void offCanvas()
    {
        this.transform.GetChild(0).gameObject.SetActive(false);
    }
    #endregion


    #region 오디오 함수
    //볼륨 비율 조정 (볼륨(0~40) : 표기(0~100))
    public void getValumInt(Slider slider, float sound)
    {
        slider.transform.GetChild(2).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text
            = ((int)((sound + 40) / 40 * 100)).ToString();
    }

    //마스터 슬라이더 관련 함수
    //슬라이더
    public void MasterAudioControl()
    {
        Slider masterAudioSlider = this.transform.GetChild(0).GetChild(3).GetChild(2).GetComponent<Slider>();
        float masterSound = masterAudioSlider.value;
        if (masterSound == -40f) sound.SetFloat("Master", -80);
        else sound.SetFloat("Master", masterSound);
        getValumInt(masterAudioSlider, masterSound);
    }
    //토글
    public void MasterToggle(bool isOn)
    {
        if (isOn)
        {
            masterToggle.transform.GetChild(0).gameObject.SetActive(true);
            masterToggle.transform.GetChild(1).gameObject.SetActive(false);
            AudioListener.volume = 1;
        }
        else
        {
            masterToggle.transform.GetChild(0).gameObject.SetActive(false);
            masterToggle.transform.GetChild(1).gameObject.SetActive(true);
            AudioListener.volume = 0;
        }
    }
    #endregion

    #region 유니티함수
    void Start()
    {
        masterToggle = this.transform.GetChild(0).GetChild(3).GetChild(1).GetComponent<Toggle>();
        masterToggle.onValueChanged.AddListener(MasterToggle);

        this.transform.GetChild(0).GetChild(3).GetChild(1).GetComponent<Toggle>().onValueChanged.AddListener(MasterToggle);
        this.GetComponent<Button>().onClick.AddListener(onCanvas);
        this.transform.GetChild(0).GetChild(1).GetComponent<Button>().onClick.AddListener(offCanvas);       //close 버튼
    }
    #endregion
}
