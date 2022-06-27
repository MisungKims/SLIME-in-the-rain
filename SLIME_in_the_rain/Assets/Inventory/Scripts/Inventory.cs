using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    #region Singleton
    public static Inventory instance;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    #endregion

    public delegate void OnSlotCountChange(int value);
    public OnSlotCountChange onSlotCountChange;

    public delegate void OnChangeItem();
    public OnChangeItem onChangeItem;


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


    void Start()
    {
       SlotCount = 4;   
    }

    // Update is called once per frame
    public bool AddItem(Item _item)
    {
        if (items.Count < slotCount)
        {
            items.Add(_item);
            //if (onChangeItem != null)
            //{
                onChangeItem.Invoke();
                return true;
         //   }
        }
        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FieldItem"))
        {
            FieldItems fieldItems = other.GetComponent<FieldItems>();
            if (AddItem(fieldItems.GetItem()))
            {

                fieldItems.DestroyItem();
            }
        }
    }




}
