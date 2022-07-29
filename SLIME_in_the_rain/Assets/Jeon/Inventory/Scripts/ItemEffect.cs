using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemEffect : ScriptableObject
{
    public int slotN;
    public EWeaponType eWeaponType;

    public ItemType it;
    public EGelatinType eGelatinType;
    public GelatinManager gelatin = new GelatinManager();
    public Weapon weapon = new Weapon();
    public abstract bool ExecuteRole(int slotNum);
}
