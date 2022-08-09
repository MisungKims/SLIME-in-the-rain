using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageManager : MonoBehaviour           
{

    #region 변수
    
    //private
    GameObject ShopCanvas;
    GameObject TowerCanvas;

    //singletons
    Slime slime;
    ICamera _camera;
    ICanvas canvas;

    #endregion

    #region 유니티 함수

    private void Start()
    {
        //singletons
        slime = Slime.Instance;
        _camera = ICamera.Instance;
        canvas = ICanvas.Instance;

        ShopCanvas = canvas.transform.Find("Shop").gameObject;
        TowerCanvas = canvas.transform.Find("Tower").gameObject;

        //슬라임 초기 위치
        Vector3 startPos = Vector3.zero;
        startPos.y = 2f;
        slime.transform.position = startPos;
    }
    private void Update()
    {
        #region 카메라 관련 조건문
        if (ShopCanvas.activeSelf)       //esc로 끌때도 있고 버튼으로 끌때가 있음  //상점 꺼졌을때
        {

            slime.canMove = false;
            _camera.Focus_Shop(this.gameObject);

        }
        else if (TowerCanvas.activeSelf)
        {
            slime.canMove = false;
            _camera.Focus_Slime();
        }
        else
        {
            slime.canMove = true;
            _camera.Focus_Slime();
        }
        #endregion
    }
    #endregion

}
