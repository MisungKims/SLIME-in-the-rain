using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ITower : MonoBehaviour
{
    #region ����
    #region �̱���
    private static ITower instance = null;
    public static ITower Instance
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

    public bool onStay = false;
    [Header("//�� �־��൵ �˴ϴ�")]
    public GameObject towerObj;
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
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("onStay");
        if(collision.transform.tag == "Slime")
        {
            onStay = true;
            towerObj = this.gameObject;
            this.transform.GetComponent<Outline>().enabled = true;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.tag == "Slime")
        {
            onStay = false;
            towerObj = new GameObject();
            this.transform.GetComponent<Outline>().enabled = false;
        }
    }
}
