using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageManager : MonoBehaviour           
{
    #region ����
    //�̱����
    Slime slime;
    ICamera _camera;
    ITower _tower;

    //���� �� UI : ��, �ǹ�
    public GameObject ShopCanvas;

    //[Header("--- �������� �Ѿ.Obj ---")]
    //public GameObject nextObj;
    //[Header("--- �� ����.Obj ---")]
    //public List<Button> ExitObj;
    #endregion

    #region ����Ƽ �Լ�

    private void Start()
    {
        //�̱���
        slime = Slime.Instance;
        _camera = ICamera.Instance;
        _tower = ITower.Instance;
    }
    private void Update()
    {
        if(_tower.onStay)                       
        {
            if (!ShopCanvas.activeSelf)       //esc�� ������ �ְ� ��ư���� ������ ����  //���� ��������
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
        else        //����
        {
            _camera.Focus_Slime();
            slime.canMove = true;
        }
    }
    #endregion

    #region �Լ�
    void ShopOpen() //���� ���� ui
    {
        ShopCanvas.SetActive(true);
        slime.canMove = false;
        _camera.Focus_Tower(_tower.towerObj);
    }
    void TowerOpen() //Ÿ�� ���� ui
    {

    }

    #endregion

}
