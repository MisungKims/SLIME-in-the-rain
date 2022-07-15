using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

public class FieldItems : PickUp
{
    #region 변수
    public Item item;
    public GameObject gb;
    bool isFind = false;
  
   
    Slime slime;
    Inventory inventory;
    InventoryUI inventoryUI;
    [SerializeField]
    public FadeOutText warningText;

    public TextMeshProUGUI wT;
    #endregion

    #region 유니티 함수
    protected override void Start()
    {
        inventory = Inventory.Instance;
        slime = Slime.Instance;
        inventoryUI = InventoryUI.Instance;
        base.Start();
    }

    #endregion

    #region 함수
    /// <summary>
    /// 아이템 획득
    /// </summary>
    public override void Get()
    {
        if (inventory.items.Count < inventory.SlotCount) //인벤토리 공간 있을때
        {
            switch (item.itemType) //아이템 타입에 따라
            {
                case ItemType.weapon:
                    if (slime.currentWeapon == null)
                    {
                        wT.text = "무기가 아직 없습니다.";
                        fullBag();
                    }
                    else
                    {
                        addItem();
                    }
                    break;
                case ItemType.gelatin:
                    isFind = false;
                    for (int i = 0; i < inventory.items.Count; i++)
                    {
                        if (inventory.items[i].itemName == item.itemName)
                        {
                            findSame();
                        }
                    }
                    if (!isFind)
                    {
                        addItem();
                    }
                    break;
                default:
                    break;
            }
        }
        else if (inventory.items.Count == inventory.SlotCount && item.itemType == ItemType.gelatin)
        {
            isFind = false;
            for (int i = 0; i < inventory.items.Count; i++)
            {
                if (inventory.items[i].itemName == item.itemName)
                {
                    findSame();
                    break;
                }
            }
            wT.text = "인벤토리가 가득 찼습니다.";
            fullBag();
        }
        else
        {
            wT.text = "인벤토리가 가득 찼습니다.";
            fullBag();
        }

        inventoryUI.RedrawSlotUI();
    }


    void addItem()
    {
        inventory.items.Add(item);
        inventoryUI.slots[inventoryUI.index].itemCount = 1;

        if (inventory.onChangedItem != null)
        {
            inventory.onChangedItem.Invoke();
        }
        inventoryUI.index++;
        this.gameObject.SetActive(false);
    }

    void fullBag()
    {
        velocity = -velocity;
        targetPos = Vector3.zero;
        targetPos.x = transform.position.x + (dir.x * velocity);
        targetPos.y = transform.position.y;
        targetPos.z = transform.position.z + (dir.z * velocity);
        transform.position = targetPos;
       
        if (warningText.gameObject.activeSelf) warningText.isAgain = true;
        else warningText.gameObject.SetActive(true);
    }

    void findSame()
    {
        for (int i = 0; i < inventory.items.Count; i++)
        {
            if (inventory.items[i].itemName == item.itemName)
            {
                inventoryUI.slots[i].SetSlotCount();
                isFind = true;
                break;
            }
        }
        this.gameObject.SetActive(false);

    }

    public void SetItem(Item _item) //아이템 셋팅
    {
        item.itemName = _item.itemName;
        item.itemType = _item.itemType;
        item.itemExplain = _item.itemExplain;

        item.itemGB = _item.itemGB;
        item.itemIcon = _item.itemIcon;

        item.efts = _item.efts;

        GameObject.Instantiate(item.itemGB, this.transform.position, Quaternion.identity).transform.parent = transform;
    }

    #endregion
}
