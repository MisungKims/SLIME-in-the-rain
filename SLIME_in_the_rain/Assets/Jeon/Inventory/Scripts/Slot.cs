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
    public Button button;

    [SerializeField]
    public GameObject itemCountImage;
    [SerializeField]
    private TextMeshProUGUI itemCountText;
    public Image itemIcon;

  
    private void Start()
    {
        button = this.transform.GetComponent<Button>();
    }

    public void UpdateSlotUI()
    {
        itemIcon.sprite = item.itemIcon;
        itemIcon.gameObject.SetActive(true);
        
        if (item.itemType == ItemType.gelatin)
        {
        itemCountImage.SetActive(true);
        itemCountText.text = Inventory.Instance.items[slotNum].itemCount.ToString();
        }
    }

    public void SetSlotCount() //���� �߰�
    {
        Inventory.Instance.items[slotNum].itemCount++;
        itemCountText.text = Inventory.Instance.items[slotNum].itemCount.ToString();
    }

    public void RemoveSlot() //���� �Լ�
    {
        item = null;
        itemCountImage.SetActive(false);
        itemIcon.gameObject.SetActive(false);
    }

    public void OnPointerUp(PointerEventData eventData) //Ŭ���� ���
    {
        if (item != null)
        {
            bool isUse = item.Use(slotNum);
            if (isUse)
            {
                Inventory.Instance.RemoveItem(slotNum);
            }
        }
    }

}
