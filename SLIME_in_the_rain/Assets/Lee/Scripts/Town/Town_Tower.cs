using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Town_Tower : MonoBehaviour
{
    #region 변수
    public GameObject uiCanvas;       //각 오브젝트의 UI
    public GameObject townManager;      //타운 매니저에 있는 카메라 관련 가져오려고

    private bool uiUseable = false;     //캐릭터가 ui 사용가능한 위치까지 왔는지 확인하는 bool
    private bool uiOpenning = false;    //ui 열려있는지 확인용 bool
    Collision collision_P = new Collision();    //현재 충돌한 콜리전 확인용(플레이어인지)
    Vector3 vec3 = new Vector3();

    #endregion

    //플레이어가 안움직이니까 CollisionStay가 꺼짐
    private void OnCollisionStay(Collision collision)
    {
        uiUseable = true;
        collision_P = collision;
        Debug.Log("onStaySlime");

    }
    private void OnCollisionExit(Collision collision)
    {
        uiUseable = false;
        collision_P = new Collision();
        Debug.Log("ExitSlime");
    }


    void ShopOpen()
    {
        if (collision_P.collider.gameObject.CompareTag("Player"))
        {
            collision_P.gameObject.GetComponent<Slime>().enabled = false;
        }
        uiOpenning = true;

        //ui 켜짐
        uiCanvas.gameObject.SetActive(true);

        //캐릭카메라 on & 메인카메라 off
        vec3.x = this.transform.position.x;
        vec3.y = this.transform.position.z + 20;
        Camera.main.transform.position = vec3;


        //플레이어 콜라이더 따기

    }
    void ShopClose()
    {
        //Ui꺼짐
        uiCanvas.SetActive(false);


        //플레이어 콜라이더 따기
        if (collision_P.collider.gameObject.CompareTag("Player"))
        {
            collision_P.gameObject.GetComponent<Slime>().enabled = true;
        }

        uiOpenning = false;
    }

    private void Start()
    {
        vec3.z = Camera.main.transform.position.z;
    }

    private void Update()
    {
        if (uiUseable && Input.GetKey(KeyCode.G))
        {
            Debug.Log("GGGGG");
            //움직임 막기
            ShopOpen();
        }
        if (uiOpenning && Input.GetKey(KeyCode.Escape))
        {
            Debug.Log("Shopopenning");
            ShopClose();
        }

    }

}
