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


    ///�߰�
    private UIObjectPoolingManager uIObjectPoolingManager;

    // �߰� 
    public EObjectFlag flag;
    #endregion

    #region ����Ƽ �Լ�
    private void Start()
    {
        inventory = Inventory.Instance;
        inventoryUI = InventoryUI.Instance;

        uIObjectPoolingManager = UIObjectPoolingManager.Instance;
    }

    #endregion

    protected override IEnumerator DetectSlime()
    {
        slime = Slime.Instance;

        // �����Ӱ��� �Ÿ��� Ž��
        while (canDetect)
        {
            dir = (slime.transform.position - transform.position).normalized;

            velocity = (velocity + acceleration * Time.deltaTime);      // ���ӵ�

            offset = slime.transform.position - transform.position;
            distance = offset.sqrMagnitude;                             // ������ ������ ������ �Ÿ�

            // �Ÿ��� 1�� ���ų� ���� �� �������� ��ġ�� �̵� (����ٴ�)
            if (distance <= 1f)
            {
                // ����� �κ��丮�� ������ ���� ���� ���⸦ �������� ���� �̵�
                if (item.itemType.Equals(ItemType.weapon) && !inventory.IsFull())
                {
                    if (!slime.currentWeapon) uIObjectPoolingManager.ShowNoWeaponText(item.itemExplain);            // ���⸦ �������� �ʾ��� �� �ؽ�Ʈ�� ������
                    else FollowSlime();
                }
                // ����ƾ�� �κ��丮�� ������ ���� ����, ������ ������ �κ��丮�� ���� ���� ���� �� ������
                else if (item.itemType.Equals(ItemType.gelatin))
                {
                    if (!inventory.IsFull()) FollowSlime();
                    else if (isAlreadyGet()) FollowSlime();
                    else uIObjectPoolingManager.ShowNoInventoryText();                                          // �κ��丮�� ������ ���� �� �ؽ�Ʈ�� ������
                }
            }
            else
            {
                followTime = 0.2f;
                velocity = 0.0f;
            }
            yield return null;
        }
    }
    #region �Լ�
    /// <summary>
    /// ������ ȹ��
    /// </summary>
    public override void Get()
    {
        AddInventory();
    }

    void addItem()
    {
        inventory.items.Add(item);

        if (float.Parse(item.maxHp) > 0)
        {
            slime.statManager.AddHP(float.Parse(item.maxHp));
        }

        ObjectPoolingManager.Instance.Set(this.gameObject, flag);

    }

    void fullBag()
    {
        velocity = -velocity;
        targetPos = Vector3.zero;
        targetPos.x = transform.position.x + (dir.x * velocity);
        targetPos.y = transform.position.y;
        targetPos.z = transform.position.z + (dir.z * velocity);
        transform.position = targetPos;
    }

    public void SetItem(Item _item) //������ ����
    {
        item.itemName = _item.itemName;
        item.itemType = _item.itemType;
        item.itemExplain = _item.itemExplain;

        item.itemGB = _item.itemGB;
        item.itemIcon = _item.itemIcon;

        item.efts = _item.efts;
        item.itemCount = 1;

        item.maxHp = _item.maxHp;
        item.coolTime = _item.coolTime;
        item.moveSpeed = _item.moveSpeed;
        item.atkSpeed = _item.atkSpeed;
        item.atkPower = _item.atkPower;
        item.atkRange = _item.atkRange;
        item.defPower = _item.defPower;
        item.increase = _item.increase;

        GameObject.Instantiate(item.itemGB, this.transform.position, Quaternion.identity).transform.parent = transform;

        if (item.itemType.Equals(ItemType.gelatin)) flag = EObjectFlag.gelatin;
        else if (item.itemType.Equals(ItemType.weapon)) flag = EObjectFlag.weapon;
    }

    public void SetItemPool(Item _item) //������ ����
    {
        item.itemName = _item.itemName;
        item.itemType = _item.itemType;
        item.itemExplain = _item.itemExplain;

        item.itemGB = _item.itemGB;
        item.itemIcon = _item.itemIcon;

        item.efts = _item.efts;
        item.itemCount = 1;

        item.maxHp = _item.maxHp;
        item.coolTime = _item.coolTime;
        item.moveSpeed = _item.moveSpeed;
        item.atkSpeed = _item.atkSpeed;
        item.atkPower = _item.atkPower;
        item.atkRange = _item.atkRange;
        item.defPower = _item.defPower;
        item.increase = _item.increase;

        if (item.itemType.Equals(ItemType.gelatin)) flag = EObjectFlag.gelatin;
        else if (item.itemType.Equals(ItemType.weapon)) flag = EObjectFlag.weapon;
    }


    //////////////////////// �߰�

    // �κ��丮�� ������ �߰�
    public void AddInventory()
    {
        if (!FindSame())        // �κ��丮�� �� �������� ������
        {
            canDetect = false;
            addItem();          // ���� �߰�
        }

        if (inventory.onChangedItem != null)
        {

            inventory.onChangedItem.Invoke();
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
            if (inventory.items[i].itemName.Equals(item.itemName))
            {
                canDetect = false;
                inventoryUI.slots[i].SetSlotCount();
                isFind = true;
                ObjectPoolingManager.Instance.Set(this.gameObject, flag);
                break;
            }
        }

        return isFind;
    }

    // �� �������� �κ��丮�� �ִ���?
    private bool isAlreadyGet()
    {
        for (int i = 0; i < inventory.items.Count; i++)
        {
            if (inventory.items[i].itemName.Equals(item.itemName))
                return true;
        }

        return false;
    }
    #endregion
}


////////////////////////////////////
/////if (!inventory.getIng)
//{
//    inventory.getIng = true;
//    AddInventory();


//    //if (!inventory.IsFull()) //�κ��丮 ���� ������
//    //{
//    //    AddInventory(); 

//    //    //switch (item.itemType) //������ Ÿ�Կ� ����
//    //    //{
//    //    //    case ItemType.weapon:
//    //    //        AddInventory();
//    //    //        break;
//    //    //    case ItemType.gelatin:
//    //    //        AddInventory();
//    //    //        break;
//    //    //    default:
//    //    //        break;
//    //    //}
//    //}
//    //else if (inventory.items.Count == inventory.SlotCount && item.itemType == ItemType.gelatin)
//    //{
//    //    AddInventory();
//    //    if (!isFind)
//    //    {
//    //        uIObjectPoolingManager.ShowNoInventoryText();
//    //        fullBag();
//    //    }
//    //}
//    //else
//    //{
//    //    uIObjectPoolingManager.ShowNoInventoryText();
//    //    fullBag();
//    //}
//    inventory.getIng = false;
//}
//if (inventory.onChangedItem != null)
//{
//    inventory.onChangedItem.Invoke();
//}