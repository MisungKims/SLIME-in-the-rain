using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Town_Tower : MonoBehaviour
{
    #region ����
    public GameObject uiCanvas;       //�� ������Ʈ�� UI
    public GameObject townManager;      //Ÿ�� �Ŵ����� �ִ� ī�޶� ���� ����������

    private bool uiUseable = false;     //ĳ���Ͱ� ui ��밡���� ��ġ���� �Դ��� Ȯ���ϴ� bool
    private bool uiOpenning = false;    //ui �����ִ��� Ȯ�ο� bool
    Collision collision_P = new Collision();    //���� �浹�� �ݸ��� Ȯ�ο�(�÷��̾�����)
    Vector3 vec3 = new Vector3();

    #endregion

    //�÷��̾ �ȿ����̴ϱ� CollisionStay�� ����
    private void OnCollisionStay(Collision collision)
    {
        uiUseable = true;
        collision_P = collision;
        Debug.Log("onStaySlime");

    }
    private void OnCollisionExit(Collision collision)
    {
        uiUseable = false;
        collision_P = new Collision();
        Debug.Log("ExitSlime");
    }


    void ShopOpen()
    {
        if (collision_P.collider.gameObject.CompareTag("Player"))
        {
            collision_P.gameObject.GetComponent<Slime>().enabled = false;
        }
        uiOpenning = true;

        //ui ����
        uiCanvas.gameObject.SetActive(true);

        //ĳ��ī�޶� on & ����ī�޶� off
        vec3.x = this.transform.position.x;
        vec3.y = this.transform.position.z + 20;
        Camera.main.transform.position = vec3;


        //�÷��̾� �ݶ��̴� ����

    }
    void ShopClose()
    {
        //Ui����
        uiCanvas.SetActive(false);


        //�÷��̾� �ݶ��̴� ����
        if (collision_P.collider.gameObject.CompareTag("Player"))
        {
            collision_P.gameObject.GetComponent<Slime>().enabled = true;
        }

        uiOpenning = false;
    }

    private void Start()
    {
        vec3.z = Camera.main.transform.position.z;
    }

    private void Update()
    {
        if (uiUseable && Input.GetKey(KeyCode.G))
        {
            Debug.Log("GGGGG");
            //������ ����
            ShopOpen();
        }
        if (uiOpenning && Input.GetKey(KeyCode.Escape))
        {
            Debug.Log("Shopopenning");
            ShopClose();
        }

    }

}
