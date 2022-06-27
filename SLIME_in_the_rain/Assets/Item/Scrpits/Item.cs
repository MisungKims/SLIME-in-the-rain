using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum itemType
{
    weapon,
    gelatin
}

[System.Serializable]
public class Item
{
    public itemType itemType;
    public string itemName;
    
    public Material itemGb;
    public bool Use()
    {
        return false;
    }
}
