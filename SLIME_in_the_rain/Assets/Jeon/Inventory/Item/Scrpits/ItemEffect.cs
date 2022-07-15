using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemEffect : ScriptableObject
{
    public Weapon weapon = new Weapon();
    public int slotN;

    public EWeaponType eWeaponType;
    protected GameObject Dissolution;



    public abstract bool ExecuteRole(int slotNum);
}
