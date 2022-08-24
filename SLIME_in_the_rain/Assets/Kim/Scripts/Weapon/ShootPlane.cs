using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootPlane : MonoBehaviour
{
    private Weapon weapon;

    private void Awake()
    {
        weapon = this.transform.parent.GetComponent<Weapon>();

    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
