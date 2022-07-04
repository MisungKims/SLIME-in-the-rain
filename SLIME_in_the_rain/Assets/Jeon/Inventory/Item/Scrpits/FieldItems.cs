using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldItems : PickUp
{
    #region 변수
    public Item item;
    public MeshRenderer gb;
    Inventory inventory;
    Slime slime;
    #endregion

    #region 유니티 함수
    protected override void Start()
    {
        inventory = Inventory.Instance;
        slime = Slime.Instance;
        //jellyManager = JellyManager.Instance;
        //objectPoolingManager = ObjectPoolingManager.Instance;

        base.Start();
    }

#endregion

    #region 함수
    /// <summary>
    /// 아이템 획득
    /// </summary>
    public override void Get()
    {
        if (inventory.items.Count < inventory.SlotCount)
        {
            inventory.items.Add(item);
            if (inventory.onChangedItem != null)
            {

                inventory.onChangedItem.Invoke();
            }
            this.gameObject.SetActive(false);
        }
        else
        {
            velocity = -velocity;
            targetPos = Vector3.zero;
            targetPos.x = transform.position.x + (dir.x * velocity);
            targetPos.y = transform.position.y;
            targetPos.z = transform.position.z + (dir.z * velocity);

            transform.position = targetPos;
            Debug.Log("아이템 공간 없음");
        }
    }

    public void SetItem(Item _item) //아이템 셋팅
    {
        item.itemName = _item.itemName;
        item.itemType = _item.itemType;
        item.itemIcon = _item.itemIcon;
        item.itemGB = _item.itemGB;
        item.itemMaterial = _item.itemMaterial;
        gb.material = item.itemMaterial;
    }

    #endregion
}
