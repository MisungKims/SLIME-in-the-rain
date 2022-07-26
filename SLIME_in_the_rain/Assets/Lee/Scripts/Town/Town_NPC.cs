using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Town_NPC : MonoBehaviour       //��Ÿ��_Ÿ�� �ҽ��� ��������
                                                //Ÿ�� UI���� (ī�޶� ���� ����) 
{
    #region ����
    //����� �� UI : ��, �ǹ�
    public GameObject ShopCanvas;
    
    
    Slime slime;
    ICamera _camera;
    ITower _tower;
    #endregion

    private void Start()
    {
        //������ �̱���
        slime = Slime.Instance;
        _camera = ICamera.Instance;
        _tower = ITower.Instance;
    }
    private void Update()
    {
        if(_tower.onStay)                       //���߿� �����Ҷ� ī�޶� ��ǥ ���Խ� ���� ��ǥ�� �̵��ϴ½����� �ٲٱ�
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
            if (!ShopCanvas.gameObject.activeSelf)       //esc�� ������ �ְ� ��ư���� ������ ����  //���� ��������
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

    #region �Լ�
    void ShopOpen() //���� ���� ui
    {
        ShopCanvas.gameObject.SetActive(true);
        slime.canMove = false;
    }
    void TowerOpen() //Ÿ�� ���� ui
    {

    }

    #endregion




}
