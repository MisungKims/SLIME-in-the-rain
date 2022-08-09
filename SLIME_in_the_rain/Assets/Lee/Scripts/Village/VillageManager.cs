using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageManager : MonoBehaviour           
{

    #region ����
    
    //private
    GameObject ShopCanvas;
    GameObject TowerCanvas;

    //singletons
    Slime slime;
    ICamera _camera;
    ICanvas canvas;

    #endregion

    #region ����Ƽ �Լ�

    private void Start()
    {
        //singletons
        slime = Slime.Instance;
        _camera = ICamera.Instance;
        canvas = ICanvas.Instance;

        ShopCanvas = canvas.transform.Find("Shop").gameObject;
        TowerCanvas = canvas.transform.Find("Tower").gameObject;

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
    }
    #endregion

}
