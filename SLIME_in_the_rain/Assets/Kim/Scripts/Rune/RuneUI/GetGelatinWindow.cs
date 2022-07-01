using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GetGelatinWindow : MonoBehaviour
{
    #region 변수
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

    #region 함수
    public void SetUI()
    {

    }

    // 획득 버튼 클릭시
    public void GetButton()
    {

    }
    #endregion
}
