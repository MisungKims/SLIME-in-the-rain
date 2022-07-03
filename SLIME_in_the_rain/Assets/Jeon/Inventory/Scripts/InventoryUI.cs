using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    #region ����
    Inventory inven; //�κ��丮
    [SerializeField]
    private Button ExpansionButton; //�κ�Ȯ���ư
    [SerializeField]
    private Slot[] slots;
    public Transform slotHolder;

    [Header ("GbUI")] 
    public GameObject inventroyPanel;
    public GameObject statsUI;
    public GameObject CombinationUI;
    public GameObject DissolutionUI;

    private JellyManager jellyManager;

    public TextMeshProUGUI JellyTextC; //����
    #region onOffBool
    bool activeInventory = false;
    bool activeStatsUI = false;
    bool activeCombination = false;
    bool activeDissolution = false;
    #endregion
    #endregion


    #region ����Ƽ �޼ҵ�
    private void Start()
    {
        inven = Inventory.Instance;
        slots = slotHolder.GetComponentsInChildren<Slot>();
        inven.onSlotCountChange += SlotChange;
        inven.onChangedItem += RedrawSlotUI;
        inventroyPanel.SetActive(activeInventory);
        statsUI.SetActive(activeStatsUI);
        CombinationUI.SetActive(activeCombination);
        DissolutionUI.SetActive(activeDissolution);
        jellyManager = JellyManager.Instance;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            activeInventory = !activeInventory;
            inventroyPanel.SetActive(activeInventory);
           
            activeCombination = false;
            activeDissolution = false;
            activeStatsUI = false;
        }

            statsUI.SetActive(activeStatsUI);
            CombinationUI.SetActive(activeCombination);
            DissolutionUI.SetActive(activeDissolution);
            JellyTextC.text = jellyManager.JellyCount.ToString();
    }
    #endregion

    #region �޼ҵ�
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

    public  void ExpansionSlot() //���� �߰�
    {
        inven.SlotCount++;
        if (inven.SlotCount >= 28)
        {
            ExpansionButton.interactable = false;
        }
    }

    #region ��ư �¿���
    public void OnOffStats()
    {
        activeStatsUI = !activeStatsUI;
   
        activeCombination = false;
        activeDissolution = false;
    }
    public void OnOffCombination()
    {
        activeCombination = !activeCombination;

        activeStatsUI = false;
        activeDissolution = false;
    }
    public void OnOffDissolution()
    {
        activeDissolution = !activeDissolution;
 
        activeStatsUI = false;
        activeCombination = false;
    }
    #endregion
   


    void RedrawSlotUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].RemoveSlot();
        }
        for (int i = 0; i < inven.items.Count; i++)
        {
            slots[i].item = inven.items[i];
            slots[i].UpdateSlotUI();
        }
    }
    #endregion
}
