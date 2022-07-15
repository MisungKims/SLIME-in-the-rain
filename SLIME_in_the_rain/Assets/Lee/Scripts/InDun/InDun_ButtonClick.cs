using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InDun_ButtonClick : MonoBehaviour
{
    //버튼 관련 캔버스
    public GameObject settingCanvas;
    public void SettingButton()
    {
        settingCanvas.gameObject.SetActive(true);
    }

    public void CloseButton()
    {
        settingCanvas.gameObject.SetActive(false);
    }

}
