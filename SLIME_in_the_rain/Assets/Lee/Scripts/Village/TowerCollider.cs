using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerCollider : MonoBehaviour
{
    public static bool onStay;
    public static GameObject thisObject;

    #region �ݶ��̴� �Լ�
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Slime")
        {
            
            onStay = true;
            thisObject = this.gameObject;
            this.transform.GetComponent<Outline>().enabled = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.tag == "Slime")
        {
            onStay = false;
            thisObject = null;
            this.transform.GetComponent<Outline>().enabled = false;
        }
    }
    #endregion
}
