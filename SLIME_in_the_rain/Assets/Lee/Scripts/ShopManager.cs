using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public GameObject shopInven;
    
    public GameObject mainCamera;
    public GameObject shopCamera;
    
    //상점
    void Shop()
    {
        Collision ObjCol = this.GetComponent<Collision>();
        OnCollisionStay(ObjCol);
    }
    private void OnCollisionStay(Collision collision)
    {

        Debug.Log("onStay");
        if (Input.GetKey(KeyCode.G))
        {
            ShopOpen();
            Debug.Log("GGGGG");
        }
    }
    void ShopOpen()
    {
        shopInven.SetActive(true);
        shopCamera.SetActive(true);
        mainCamera.SetActive(false);

        this.GetComponent<Slime>().enabled = false; //움직임 막기
        
    }
    void ShopClose()
    {
        this.GetComponent<Slime>().enabled = true;
    }



}
