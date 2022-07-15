using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InDun_UI_Jelly : MonoBehaviour
{
    #region 변수
    //젤리 변수 참조
    private JellyManager jellyManager;
    private Slime slime;
    //젤리 관련 ui 오브젝트
    //main
    public TextMeshProUGUI jellyText;
    public GameObject jelly_3d_main;

    #endregion

    //3d젤리 회전
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
        //젤리 카운팅
        jellyText.text = jellyManager.JellyCount.ToString();

        //ui_ 젤리모양 회전
        vec3.y += speed;
        jelly_3d_main.transform.localRotation = Quaternion.Euler(vec3);
        
    }
}
