using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldItems : MonoBehaviour
{
    public Item item;
   
  
     

    public void SetItem(Item _item)
    {
        item.itemName = _item.itemName;
        item.itemType = _item.itemType;
        item.itemIcon = _item.itemIcon;


    }
    public Item GetItem()
    {
        return item;
    }
    public void DestroyItem()
    {
        Debug.Log("a");
        Destroy(this.gameObject);
    }
}
