using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    #region 싱글톤
    private static Inventory instance = null;
    public static Inventory Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }

    #endregion
    Stats saveExtraStats = new Stats(0f, 0f, 0f, 0f, 0f, 0f, 1f, 0f, 1, 0);
    public delegate void OnSlotCountChange(int value);
    public OnSlotCountChange onSlotCountChange;

    public delegate void OnChangedItem();
    public OnChangedItem onChangedItem;
    
   public List<Item> items = new List<Item>();

    private StatManager statManager;

    private float sAtkRange;
    public bool getIng = false;

    private int slotCount;
    public int SlotCount
    {
        get
        {
            return slotCount;
        }
        set
        {
            slotCount = value;
            onSlotCountChange.Invoke(slotCount);
        }
    }

    public void RemoveItem(int _index)
    {
        items.RemoveAt(_index);
        if (onChangedItem != null)
        {
        onChangedItem.Invoke();
        }
    }


    #region 유니티메소드
    private void Update()
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].itemCount <= 0)
            {
                RemoveItem(i);

            }
        }

    }
    void Awake()
    {
        if (null == instance)
        {
            instance = this;

            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    void Start()
    {
       SlotCount = 4;
        statManager = StatManager.Instance;
    }

    #endregion



 
    //////////////////// 추가
    
    // 인벤토리에 공간이 없으면 true 반환
    public bool IsFull()
    {
        if (items.Count < SlotCount) return false;
        else return true;
    }

    public void addItem(Item _item, int _addCount)
    {
        if (findSame(_item))
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].itemName == _item.itemName)
                {
                    items[i].itemCount += _addCount;
                }
            }
        }
        else
        {
            items.Add(_item);
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].itemName == _item.itemName)
                {
                    items[i].itemCount = _addCount;
                }
            }
        }

        if (onChangedItem != null)
        {
            onChangedItem.Invoke();
        }
    }

    public bool findSame(Item _item)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].itemName == _item.itemName)
            {
                return true;
            }
        }
        return false;
    }


   public void statGelatinAdd() //젤라틴 스탯 반영해주는 코루틴
    {
        saveExtraStats.defensePower =0;
        saveExtraStats.maxHP = 0;
        saveExtraStats.moveSpeed = 0;
        saveExtraStats.coolTime = 0;
        saveExtraStats.attackSpeed = 0;
        saveExtraStats.attackPower = 0;
        saveExtraStats.attackRange =0;

        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].itemType == ItemType.gelatin)
            {
                saveExtraStats.defensePower += float.Parse(items[i].defPower)*items[i].itemCount;
                saveExtraStats.maxHP += float.Parse(items[i].maxHp) * items[i].itemCount;
                saveExtraStats.moveSpeed += float.Parse(items[i].moveSpeed) * items[i].itemCount;
                saveExtraStats.coolTime += float.Parse(items[i].coolTime) * items[i].itemCount;
                saveExtraStats.attackSpeed += float.Parse(items[i].atkSpeed) * items[i].itemCount;
                saveExtraStats.attackPower += float.Parse(items[i].atkPower) * items[i].itemCount;
                saveExtraStats.attackRange += float.Parse(items[i].atkRange) * items[i].itemCount;
            }
        }
        statManager.AddGelatinDefensePower(saveExtraStats.defensePower);
        statManager.AddGelatinMaxHP(saveExtraStats.maxHP);
        statManager.AddGelatinMoveSpeed(saveExtraStats.moveSpeed);
        statManager.AddGelatinCoolTime(saveExtraStats.coolTime);
        statManager.AddGelatinAttackSpeed(saveExtraStats.attackSpeed);
        statManager.AddGelatinAttackPower(saveExtraStats.attackPower);
        statManager.AddGelatinDefensePower(saveExtraStats.attackRange);
    }
    public void ResetInven()
    {
        items.Clear();
         SlotCount = 4;
        if (onChangedItem != null)
        {
            onChangedItem.Invoke();
        }
        InventoryUI.Instance.expansCost = 5;
        InventoryUI.Instance.addButtonCostText.text = InventoryUI.Instance.expansCost.ToString() + "J";

        if (onSlotCountChange != null)
        {
            onSlotCountChange.Invoke(slotCount);
        }
    }
}
