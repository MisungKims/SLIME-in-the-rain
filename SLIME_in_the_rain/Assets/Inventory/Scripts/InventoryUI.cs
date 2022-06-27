using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    Inventory inven;

    public GameObject inventroyPanel;
    [SerializeField]
    private Button AddButton;
    bool activeInventory = false;

    public Slot[] slots;
    public Transform slotHolder;

    private void Start()
    {
        inven = Inventory.instance;
        slots = slotHolder.GetComponentsInChildren<Slot>();
        inven.onSlotCountChange += SlotChange;
        inventroyPanel.SetActive(activeInventory);
    }

    private void SlotChange(int value)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < inven.SlotCount)
            {
                slots[i].GetComponent<Button>().interactable = true;
            }
            else
            {
                slots[i].GetComponent<Button>().interactable = false;
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            activeInventory = !activeInventory;
            inventroyPanel.SetActive(activeInventory);
        }
    }

    public  void AddSlot()
    {
        inven.SlotCount++;
        if (inven.SlotCount >= 28)
        {
            AddButton.interactable = false;
        }
    }
}
