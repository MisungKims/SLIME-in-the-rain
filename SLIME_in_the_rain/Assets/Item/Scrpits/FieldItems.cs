using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldItems : MonoBehaviour
{
    public Item item;
    public MeshRenderer mesh;
     

    public void SetItem(Item _item)
    {
        item.itemName = _item.itemName;
        item.itemGb = _item.itemGb;
        item.itemType = _item.itemType;

        mesh.materials[0].color = _item.itemGb.color;
    }
    public Item GetItem()
    {
        return item;
    }
    public void DestroyItem()
    {
        Destroy(gameObject);
    }
}
