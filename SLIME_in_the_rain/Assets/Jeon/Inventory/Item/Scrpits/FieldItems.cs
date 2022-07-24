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
  
    ////////// �߰� : slime ���� ���ֱ�
    
    Inventory inventory;
    InventoryUI inventoryUI;
    [SerializeField]
    public FadeOutText warningText;

    public TextMeshProUGUI wT;

    #endregion

    #region ����Ƽ �Լ�
    private void Start()
    {
        inventory = Inventory.Instance;
        inventoryUI = InventoryUI.Instance;
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
                        wT.text = "���Ⱑ ���� �����ϴ�.";
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
            wT.text = "�κ��丮�� ���� á���ϴ�.";
            fullBag();
        }
        else
        {
            wT.text = "�κ��丮�� ���� á���ϴ�.";
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

    public void SetItem(Item _item) //������ ����
    {
        item.itemName = _item.itemName;
        item.itemType = _item.itemType;
        item.itemExplain = _item.itemExplain;

        item.itemGB = _item.itemGB;
        item.itemIcon = _item.itemIcon;

        item.efts = _item.efts;

        GameObject.Instantiate(item.itemGB, this.transform.position, Quaternion.identity).transform.parent = transform;
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
