using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

public class FieldItems : PickUp
{
    #region ����
    public Item item;
    public GameObject gb;
    bool isFind = false;
  
    Inventory inventory;
    InventoryUI inventoryUI;
    [SerializeField]
    public FadeOutText warningText;

    public TextMeshProUGUI wT;

    ///�߰�
    private UIObjectPoolingManager uIObjectPoolingManager;
    #endregion

    #region ����Ƽ �Լ�
    private void Start()
    {
        inventory = Inventory.Instance;
        inventoryUI = InventoryUI.Instance;

        uIObjectPoolingManager = UIObjectPoolingManager.Instance;
    }

    #endregion

    #region �Լ�
    /// <summary>
    /// ������ ȹ��
    /// </summary>
    public override void Get()
    {
        if (inventory.items.Count < inventory.SlotCount) //�κ��丮 ���� ������
        {
            switch (item.itemType) //������ Ÿ�Կ� ����
            {
                case ItemType.weapon:
                    if (slime.currentWeapon == null)
                    {
                        uIObjectPoolingManager.ShowNoWeaponText();
                        //wT.text = "���Ⱑ ���� �����ϴ�.";
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
            //wT.text = "�κ��丮�� ���� á���ϴ�.";
            fullBag();
        }
        else
        {
            uIObjectPoolingManager.ShowNoInventoryText();
            //wT.text = "�κ��丮�� ���� á���ϴ�.";
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

    public void SetItem(Item _item) //������ ����
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

    public void SetItemPool(Item _item) //������ ����
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


    //////////////////////// �߰�

    // �κ��丮�� ������ �߰�
    public void AddInventory()
    {
        if (!FindSame())        // �κ��丮�� �� �������� ������
        {
            addItem();          // ���� �߰�
        }
    }

    // �κ��丮���� ���� �������� ã�� ī��Ʈ�� ������Ŵ
    private bool FindSame()
    {
        if (!inventory) inventory = Inventory.Instance;
        if (!inventoryUI) inventoryUI = InventoryUI.Instance;

        isFind = false;

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

        return isFind;
    }
    #endregion
}