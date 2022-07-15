using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwap : MonoBehaviour
{
    public ItemWeaponSwap swap;
    public EWeaponType e;


    private void Start()
    {
       e= swap.eWeaponType;
    }
}
