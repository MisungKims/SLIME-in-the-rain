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
    Vector3 vec3;
    #endregion

    private void Start()
    {
        //������ �̱���
        slime = Slime.Instance;
        vec3.y = Camera.main.transform.position.y;
    }
    private void Update()
    {
        if(Town_Tower.onStay)                       //���߿� �����Ҷ� ī�޶� ��ǥ ���Խ� ���� ��ǥ�� �̵��ϴ½����� �ٲٱ�
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
            if (!ShopCanvas.gameObject.activeSelf)       //esc�� ������ �ְ� ��ư���� ������ ����  //���� ��������
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

    #region �Լ�
    Vector3 SlimePos()
    {
        //������ ��ǥ �ޱ�
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
