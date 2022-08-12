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
  
    Inventory inventory;
    InventoryUI inventoryUI;
    [SerializeField]
    public FadeOutText warningText;

    public TextMeshProUGUI wT;

    ///추가
    private UIObjectPoolingManager uIObjectPoolingManager;
    #endregion

    #region 유니티 함수
    private void Start()
    {
        inventory = Inventory.Instance;
        inventoryUI = InventoryUI.Instance;

        uIObjectPoolingManager = UIObjectPoolingManager.Instance;
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
                        uIObjectPoolingManager.ShowNoWeaponText();
                        //wT.text = "무기가 아직 없습니다.";
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
            uIObjectPoolingManager.ShowNoInventoryText();
            //wT.text = "인벤토리가 가득 찼습니다.";
            fullBag();
        }
        else
        {
            uIObjectPoolingManager.ShowNoInventoryText();
            //wT.text = "인벤토리가 가득 찼습니다.";
            fullBag();
        }

        inventoryUI.RedrawSlotUI();
    }


    void addItem()
    {
        inventory.items.Add(item);
        inventory.items[inventory.items.Count - 1].itemCount = 1;
       // inventoryUI.slots[inventoryUI.index].itemCount = 1;

        if (inventory.onChangedItem != null)
        {
            inventory.onChangedItem.Invoke();
        }
        //inventoryUI.index++;
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
       
        //if (warningText.gameObject.activeSelf) warningText.isAgain = true;
        //else warningText.gameObject.SetActive(true);
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
        item.itemCount = _item.itemCount;

        item.maxHp = _item.maxHp;
        item.coolTime = _item.coolTime;
        item.moveSpeed = _item.moveSpeed;
        item.atkSpeed = _item.atkSpeed;
        item.atkPower = _item.atkPower;
        item.atkRange = _item.atkRange;
        item.defPower = _item.defPower;
        item.increase = _item.increase;
        GameObject.Instantiate(item.itemGB, this.transform.position, Quaternion.identity).transform.parent = transform;
    }

    public void SetItemPool(Item _item) //아이템 셋팅
    {
        item.itemName = _item.itemName;
        item.itemType = _item.itemType;
        item.itemExplain = _item.itemExplain;

        item.itemGB = _item.itemGB;
        item.itemIcon = _item.itemIcon;

        item.efts = _item.efts;
        item.itemCount = _item.itemCount;

        item.maxHp = _item.maxHp;
        item.coolTime = _item.coolTime;
        item.moveSpeed = _item.moveSpeed;
        item.atkSpeed = _item.atkSpeed;
        item.atkPower = _item.atkPower;
        item.atkRange = _item.atkRange;
        item.defPower = _item.defPower;
        item.increase = _item.increase;
    }


    //////////////////////// 추가

    // 인벤토리에 아이템 추가
    public void AddInventory()
    {
        if (!FindSame())        // 인벤토리에 이 아이템이 없으면
        {
            addItem();          // 새로 추가
        }
    }

    // 인벤토리에서 같은 아이템을 찾아 카운트를 증가시킴
    private bool FindSame()
    {
        if (!inventory) inventory = Inventory.Instance;
        if (!inventoryUI) inventoryUI = InventoryUI.Instance;

        isFind = false;

        for (int i = 0; i < inventory.items.Count; i++)
        {
            if (inventory.items[i].itemName == item.itemName)
            {
                inventory.items[i].itemCount++;
                isFind = true;
                break;
            }
        }
        this.gameObject.SetActive(false);

        return isFind;
    }
    #endregion
}
