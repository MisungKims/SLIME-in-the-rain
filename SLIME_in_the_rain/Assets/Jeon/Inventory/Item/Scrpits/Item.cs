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
    public Item(ItemType _itemType, string _itemName, string _itemExplain) { itemType = _itemType; itemName = _itemName; itemExplain = _itemExplain;}
    public ItemType itemType;
    public string itemName, itemExplain;

    public Sprite itemIcon;
    public GameObject itemGB;
    public List<ItemEffect> efts = new List<ItemEffect>();


    public bool Use(int _slotNum)
    {
        bool isUsed = false;
        if (efts.Count > 0)
        {

            foreach (ItemEffect eft in efts)
            {
               
                    isUsed = eft.ExecuteRole(_slotNum);

            }
            return isUsed;
        }
        else
        {
            return false;
        }
    }
}
