using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System;

public class Slot : MonoBehaviour,IPointerUpHandler
{
    public int slotNum;
    public Item item;
    public int itemCount = 1;
   
    private Inventory inventory;

    [SerializeField]
    private GameObject itemCountImage;
    [SerializeField]
    private TextMeshProUGUI itemCountText;
    public Image itemIcon;


    private void Start()
    {
        inventory = Inventory.Instance;
    }

    public void UpdateSlotUI()
    {
        itemIcon.sprite = item.itemIcon;
        itemIcon.gameObject.SetActive(true);
        
        if (item.itemType == ItemType.gelatin)
        {
        itemCountImage.SetActive(true);
        itemCountText.text = itemCount.ToString();
        }
    }

    public void SetSlotCount()
    {
        itemCount++;
        itemCountText.text = itemCount.ToString();
    }

    private void ClearSlot()
    {
        item = null;
        itemIcon.sprite = null;
        itemCountImage.SetActive(false);
    }

    public void RemoveSlot()
    {
        item = null;
        itemIcon.gameObject.SetActive(false);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        bool isUse = item.Use(slotNum);
        if (isUse)
        {
            Inventory.Instance.RemoveItem(slotNum);
        }
    }

}
