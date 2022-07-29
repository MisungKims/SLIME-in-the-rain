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

    public delegate void OnSlotCountChange(int value);
    public OnSlotCountChange onSlotCountChange;

    public delegate void OnChangedItem();
    public OnChangedItem onChangedItem;
    
   public List<Item> items = new List<Item>();

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
        onChangedItem.Invoke();
    }


    #region 유니티메소드
    private void Update()
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].itemCount == 0)
            {
                RemoveItem(i);
            }
        }
       
    }
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

    }
    void Start()
    {
       SlotCount = 4;
    }

    #endregion

 
    //////////////////// 추가
    
    // 인벤토리에 공간이 없으면 true 반환
    public bool IsFull()
    {
        if (items.Count < SlotCount) return false;
        else return true;
    }
}
