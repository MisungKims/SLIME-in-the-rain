using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Town_Button : MonoBehaviour
{
    //��ư ���� ĵ����
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
