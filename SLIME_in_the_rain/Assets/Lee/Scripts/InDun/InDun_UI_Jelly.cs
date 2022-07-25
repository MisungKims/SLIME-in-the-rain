using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InDun_UI_Jelly : MonoBehaviour
{
    #region 변수
    //젤리 변수 참조
    private JellyManager jellyManager;

    //젤리 관련 ui 오브젝트
    //main
    public TextMeshProUGUI jellyText;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        jellyManager = JellyManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        //젤리 카운팅
        jellyText.text = jellyManager.JellyCount.ToString();        
    }
}
