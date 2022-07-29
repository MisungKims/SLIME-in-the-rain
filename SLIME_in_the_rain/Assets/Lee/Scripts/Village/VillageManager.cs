using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageManager : MonoBehaviour           
{
    #region 변수
    //싱글톤들
    Slime slime;
    ICamera _camera;
    ITower _tower;

    //관리 할 UI : 샵, 건물
    public GameObject ShopCanvas;

    //[Header("--- 다음으로 넘어감.Obj ---")]
    //public GameObject nextObj;
    //[Header("--- 씬 종료.Obj ---")]
    //public List<Button> ExitObj;
    #endregion

    #region 유니티 함수

    private void Start()
    {
        //싱글톤
        slime = Slime.Instance;
        _camera = ICamera.Instance;
        _tower = ITower.Instance;
    }
    private void Update()
    {
        if(_tower.onStay)                       
        {
            if (!ShopCanvas.activeSelf)       //esc로 끌때도 있고 버튼으로 끌때가 있음  //상점 꺼졌을때
            {
                _camera.Focus_Slime();
                slime.canMove = true;
            }
            if (Input.GetKeyDown(KeyCode.G))
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

        }
        else        //평상시
        {
            _camera.Focus_Slime();
            slime.canMove = true;
        }
    }
    #endregion

    #region 함수
    void ShopOpen() //상점 관련 ui
    {
        ShopCanvas.SetActive(true);
        slime.canMove = false;
        _camera.Focus_Tower(_tower.towerObj);
    }
    void TowerOpen() //타워 관련 ui
    {

    }

    #endregion

}
