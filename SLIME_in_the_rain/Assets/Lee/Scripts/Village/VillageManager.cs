using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class VillageManager : MapManager           
{
    #region ����
    
    public GameObject ShopCanvas;
    public GameObject TowerCanvas;



    //singletons
    Slime slime;
    ICamera _camera;
    SceneDesign sceneDesign;
    JellyManager jellyManager;


    #endregion

    #region ����Ƽ ����������Ŭ
    private void Start()
    {
        //singletons
        sceneDesign = SceneDesign.Instance;
        slime = Slime.Instance;
        _camera = ICamera.Instance;
        jellyManager = JellyManager.Instance;



        StartCoroutine(Clear());
    }
    
    private void Update()
    {
        #region ī�޶� ���� ���ǹ�
        if (ShopCanvas.activeSelf)       //esc�� ������ �ְ� ��ư���� ������ ����  //���� ��������
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
    IEnumerator Clear()
    {
        while(!slime.currentWeapon)
        {
            yield return null;
        }
        ClearMap();
    }

    private void OnDisable()
    {
        if(sceneDesign)
        {
            
            sceneDesign.ResetScene();
            Debug.Log("Execution Reset");
        }
        else
        {
            //Debug.Log("Null SceneDesign instance");
        }

        if(jellyManager)
        {
            sceneDesign.jellyInit = jellyManager.JellyCount;
            Debug.Log("Execution sceneDesign.jellyInit");
        }
        else
        {
            //Debug.Log("Null JellyManager instance");
        }
    }
    #endregion

}
