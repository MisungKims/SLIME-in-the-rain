using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InDun_UI_Jelly : MonoBehaviour
{
    #region ����
    //���� ���� ����
    private JellyManager jellyManager;

    //���� ���� ui ������Ʈ
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
        //���� ī����
        jellyText.text = jellyManager.JellyCount.ToString();        
    }
}
