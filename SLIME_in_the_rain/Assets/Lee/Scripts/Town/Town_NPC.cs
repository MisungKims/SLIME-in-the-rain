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
    ICamera _camera;
    ITower _tower;
    #endregion

    private void Start()
    {
        //슬라임 싱글톤
        slime = Slime.Instance;
        _camera = ICamera.Instance;
        _tower = ITower.Instance;
    }
    private void Update()
    {
        if(_tower.onStay)                       //나중에 수정할때 카메라 좌표 대입식 말고 좌표를 이동하는식으로 바꾸기
        {
           if(Input.GetKeyDown(KeyCode.G))
            {
                //Debug.Log("G");
                switch (_tower.towerObj.name)
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
                _camera.Focus_Slime();
                slime.canMove = true;
            }
        }
        else
        {
            _camera.Focus_Slime();
        }
    }

    #region 함수
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
