using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GetGelatinWindow : MonoBehaviour
{
    #region ����
    [SerializeField]
    private TextMeshProUGUI gelatinNameTxt;
    private string gelatinName;
    public string GelatinName
    {
        set
        {
            gelatinName = value;
            gelatinNameTxt.text = gelatinName;
        }
    }
    #endregion

    #region �Լ�
    public void SetUI()
    {

    }

    // ȹ�� ��ư Ŭ����
    public void GetButton()
    {

    }
    #endregion
}
