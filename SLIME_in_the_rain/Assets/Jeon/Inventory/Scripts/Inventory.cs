using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    #region ΩÃ±€≈Ê
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

    #region ¿Ø¥œ∆º∏ﬁº“µÂ
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
}
