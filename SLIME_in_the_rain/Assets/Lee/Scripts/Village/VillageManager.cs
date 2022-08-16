using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VillageManager : MonoBehaviour           
{

    #region 변수
    public GameObject ShopCanvas;
    public GameObject TowerCanvas;

    //singletons
    Slime slime;
    ICamera _camera;
    SceneDesign sceneDesign;
    #endregion

    #region 유니티 함수
    private void Awake()
    {
        sceneDesign = SceneDesign.Instance;
    }

    private void Start()
    {
        //singletons
        slime = Slime.Instance;
        _camera = ICamera.Instance;

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
        if (slime.currentWeapon != null)
        {
            sceneDesign.mapClear = true;
        }
    }
    private void OnDisable()
    {
        sceneDesign.ResetScene();
    }
    #endregion

}
