using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//���߿� �����Ҷ� ī�޶� ��ǥ ���Խ� ���� ��ǥ�� �̵��ϴ� ������ �ٲٱ�
public class ICamera : MonoBehaviour
{
    #region ����
    
    private Vector3 vec3;
    private GameObject shopCanvas;
    bool isStd = false;

    Slime slime;
    #endregion

    private void OnEnable()
    {
        slime = Slime.Instance;

        if (SceneManager.GetActiveScene().buildIndex > 2)
        {
            isStd = true;
        }
        else if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            shopCanvas = GameObject.Find("VillageCanvas").transform.Find("Shop").gameObject;
            isStd = true;
        }
        else if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            Debug.Log("not STD");
            isStd = false;
            Title();
        }
        else if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            Debug.Log("not STD");
            isStd = false;
            Result();
        }
    }

    private void Update()
    {
        if (isStd)
        {
            if (SceneManager.GetActiveScene().buildIndex != 1)
            {
                Std();
            }
            else
            {
                if(shopCanvas.activeSelf)
                {
                    StartCoroutine(ShopOpen());
                }
                else
                {
                    Std();
                }
            }
        }
    }
    IEnumerator ShopOpen()
    {
        Shop();
        slime.canMove = false;
        while (shopCanvas.activeSelf)
        {
            yield return null;
        }
        slime.canMove = true;
    }


    public void Title()
    {
        Debug.Log("Title Camera");
        vec3.x = -3f;
        vec3.y = 6f;
        vec3.z = -14f;
        Camera.main.transform.SetPositionAndRotation(vec3, Quaternion.Euler(Vector3.right * 30));
        Camera.main.orthographicSize = 3f;
    }
    public void Std()
    {
        Debug.Log("STD Camera");
        vec3.x = slime.transform.position.x;
        vec3.y = 13.0f;
        vec3.z = slime.transform.position.z - 19.0f;
        Camera.main.transform.SetPositionAndRotation(vec3, Quaternion.Euler(Vector3.right * 30));
        Camera.main.orthographicSize = 5f;
    }
    public void Shop()
    {
        Debug.Log("Shop Camera");
        slime.canMove = false;
        vec3.x = slime.transform.position.x - 7f;
        vec3.y = 9.1f;
        vec3.z = slime.transform.position.z - 10f;
        Camera.main.transform.position = vec3;
    }
    public void Result()
    {
        Debug.Log("Result Camera");
        vec3.x = 960f;
        vec3.y = 540f;
        vec3.z = -500f;
        Camera.main.transform.SetPositionAndRotation(vec3, new Quaternion());
        Camera.main.orthographicSize = 583f;

    }
}
