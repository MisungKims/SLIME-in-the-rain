using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ICamera : MonoBehaviour
{
    #region º¯¼ö
    #region ½Ì±ÛÅæ
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
        vec3.z = slime.transform.position.z - 12f;

        Camera.main.transform.position = vec3;
    }
    public void Focus_Town(GameObject gameObject)
    {
        vec3.x = gameObject.transform.localPosition.x + 2f;
        vec3.y = 10f;
        vec3.z = gameObject.transform.localPosition.z - 22.0f;
        Camera.main.transform.position =  vec3;
    }
    public void Focus_Obj(GameObject gameObject)
    {
        vec3.x = slime.transform.position.x;
        vec3.y = Camera.main.transform.position.y;
        vec3.z = slime.transform.position.z - 28;

        Camera.main.transform.position = vec3;
    }
}
