using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InDun_UI_Jelly : MonoBehaviour
{
    #region ����
    //���� ���� ����
    private JellyManager jellyManager;
    private Slime slime;
    //���� ���� ui ������Ʈ
    //main
    public TextMeshProUGUI jellyText;
    public GameObject jelly_3d_main;

    #endregion

    //3d���� ȸ��
    Vector3 vec3 = new Vector3();
    float speed = 1f;

    // Start is called before the first frame update
    void Start()
    {
        jellyManager = JellyManager.Instance;
        vec3.y = 1;
    }

    // Update is called once per frame
    void Update()
    {
        //���� ī����
        jellyText.text = jellyManager.JellyCount.ToString();

        //ui_ ������� ȸ��
        vec3.y += speed;
        jelly_3d_main.transform.localRotation = Quaternion.Euler(vec3);
        
    }
}
