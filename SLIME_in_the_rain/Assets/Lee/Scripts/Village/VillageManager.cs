using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VillageManager : MonoBehaviour           
{

    #region ����
    public GameObject ShopCanvas;
    public GameObject TowerCanvas;

    //singletons
    Slime slime;
    ICamera _camera;
    SceneDesign sceneDesign;
    #endregion

    #region ����Ƽ �Լ�
    private void Awake()
    {
        sceneDesign = SceneDesign.Instance;
    }

    private void Start()
    {
        //singletons
        slime = Slime.Instance;
        _camera = ICamera.Instance;

        //������ �ʱ� ��ġ
        Vector3 startPos = Vector3.zero;
        startPos.y = 2f;
        slime.transform.position = startPos;
        
        
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
