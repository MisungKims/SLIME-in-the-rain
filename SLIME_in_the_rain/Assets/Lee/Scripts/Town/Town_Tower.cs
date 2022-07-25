using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Town_Tower : MonoBehaviour
{
    public static bool onStay =false;
    public static GameObject towerObj;
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
