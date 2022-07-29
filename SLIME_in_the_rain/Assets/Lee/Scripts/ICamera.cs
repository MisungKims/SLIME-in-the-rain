using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//���߿� �����Ҷ� ī�޶� ��ǥ ���Խ� ���� ��ǥ�� �̵��ϴ� ������ �ٲٱ�
public class ICamera : MonoBehaviour
{
    #region ����
    #region �̱���
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
    // Start is called before the first frame update
    private void Start()
    {
        slime = Slime.Instance;
    }

    public void Focus_Slime()
    {
        vec3.x = slime.transform.position.x;
        vec3.y = 10f;
        vec3.z = slime.transform.position.z - 10f;

        Camera.main.transform.position = vec3;
    }
    public void Focus_Tower(GameObject gameObject)
    {
        vec3.x = gameObject.transform.position.x - 6f;
        vec3.y = 10f;
        vec3.z = gameObject.transform.position.z - 8f;
        Camera.main.transform.position =  vec3;
    }

}
