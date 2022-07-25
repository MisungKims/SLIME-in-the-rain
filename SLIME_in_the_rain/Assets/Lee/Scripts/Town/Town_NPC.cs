using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Town_NPC : MonoBehaviour       //※타운_타워 소스랑 엮여있음
                                                //타워 UI관리 (카메라 제어 포함) 
{
    #region 변수
    //열어야 할 UI : 샵, 건물
    public GameObject ShopCanvas;
    
    
    Slime slime;
    Vector3 vec3;
    #endregion

    private void Start()
    {
        //슬라임 싱글톤
        slime = Slime.Instance;
        vec3.y = Camera.main.transform.position.y;
    }
    private void Update()
    {
        if(Town_Tower.onStay)                       //나중에 수정할때 카메라 좌표 대입식 말고 좌표를 이동하는식으로 바꾸기
        {
           if(Input.GetKeyDown(KeyCode.G))
            {
                //Debug.Log("G");
                Camera.main.transform.position = TowerPos(Town_Tower.towerObj);
                switch (Town_Tower.towerObj.name)
                {
                    case "Shop":
                        ShopOpen();
                        break;

                    default:
                        break;
                }
            }
            if (!ShopCanvas.gameObject.activeSelf)       //esc로 끌때도 있고 버튼으로 끌때가 있음  //상점 꺼졌을때
            {
                Camera.main.transform.position = SlimePos();
                slime.canMove = true;
            }
        }
        else
        {
            Camera.main.transform.position = SlimePos();
        }
    }

    #region 함수
    Vector3 SlimePos()
    {
        //슬라임 좌표 받기
        vec3.x = slime.transform.position.x;

        vec3.z = slime.transform.position.z - 28;

        return vec3;
    }
    Vector3 TowerPos(GameObject tower)
    {
        vec3.x = gameObject.transform.localPosition.x + 2f;
        vec3.z = gameObject.transform.localPosition.z - 22.0f;
        return vec3;
    }


    void ShopOpen() //상점 관련 ui
    {
        ShopCanvas.gameObject.SetActive(true);
        slime.canMove = false;
    }
    void TowerOpen() //타워 관련 ui
    {

    }

    #endregion




}
