using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//나중에 수정할때 카메라 좌표 대입식 말고 좌표를 이동하는 식으로 바꾸기
public class ICamera : MonoBehaviour
{
    #region 변수
    #region 싱글톤
    private static ICamera instance = null;
    public static ICamera Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }
    #endregion

    private Vector3 vec3;
    private GameObject shopCanvas;
    bool isStd = false;

    Slime slime;
    #endregion

    private void Awake()
    {
        if (null == instance)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

    }
    private void Start()
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
            isStd = false;
            Title();
        }
        else if (SceneManager.GetActiveScene().buildIndex == 2)
        {
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
                if (!shopCanvas.activeSelf)
                {
                    slime.canMove = true;
                    Std();
                }
                else
                {
                    slime.canMove = false;
                    Shop();
                }

            }
        }
    }

    public void Title()
    {
        vec3.x = -3f;
        vec3.y = 6f;
        vec3.z = -14f;
        Camera.main.transform.position = vec3;
        Camera.main.transform.rotation = Quaternion.Euler(Vector3.right * 30);
        Camera.main.orthographicSize = 3f;
    }
    public void Std()
    {

        vec3.x = slime.transform.position.x;
        vec3.y = 8f;
        vec3.z = slime.transform.position.z - 10f;

        Camera.main.transform.position = vec3;
        Camera.main.transform.rotation = Quaternion.Euler(Vector3.right * 30);
        Camera.main.orthographicSize = 5f;
    }
    public void Shop()
    {
        slime.canMove = false;
        vec3.x = slime.transform.position.x - 7f;
        vec3.y = 8f;
        vec3.z = slime.transform.position.z - 10f;
        Camera.main.transform.position = vec3;
    }
    IEnumerator Result()
    {
        while (true)
        {
            vec3.x = 960f;
            vec3.y = 540f;
            vec3.z = -500f;
            Camera.main.transform.position = vec3;
            Camera.main.transform.rotation = new Quaternion();
            Camera.main.orthographicSize = 583f;
        }

    }
}
