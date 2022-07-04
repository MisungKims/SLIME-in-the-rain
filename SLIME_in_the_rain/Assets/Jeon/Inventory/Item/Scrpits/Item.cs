using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    weapon,
    gelatin
}

[System.Serializable]
public class Item
{
    public ItemType itemType;
    public string itemName;
    public Sprite itemIcon;
    public GameObject itemGB;

    public Material itemMaterial;


    public bool Use()
    {
        return false;
    }
}
