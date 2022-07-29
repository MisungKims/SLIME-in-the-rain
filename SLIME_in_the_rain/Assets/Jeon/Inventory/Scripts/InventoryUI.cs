using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    #region 변수
    #region 싱글톤
    private static InventoryUI instance = null;
    public static InventoryUI Instance
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

    Inventory inventory; //인벤토리
    [SerializeField]
    private Button ExpansionButton; //인벤확장버튼
    [SerializeField]
    public Slot[] slots;
    public Transform slotHolder;

    [Header ("GbUI")] 
    public GameObject inventroyPanel;
    public GameObject statsUI;
    public GameObject CombinationUI;
    public GameObject DissolutionUI;

    private JellyManager jellyManager;
    public TextMeshProUGUI JellyTextC; //젤리
    #region onOffBool
   public bool activeInventory = false;
   public bool activeStatsUI = false;
   public bool activeCombination = false;
   public bool activeDissolution = false;
    #endregion
    #endregion


    #region 유니티 메소드
    private void Start()
    {
        inventory = Inventory.Instance;
        jellyManager = JellyManager.Instance;
        slots = slotHolder.GetComponentsInChildren<Slot>();
        inventory.onSlotCountChange += SlotChange;
        inventory.onChangedItem += RedrawSlotUI;
        inventroyPanel.SetActive(activeInventory);
        statsUI.SetActive(activeStatsUI);
        CombinationUI.SetActive(activeCombination);
        DissolutionUI.SetActive(activeDissolution);

      
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

    #region 메소드
    private void SlotChange(int value)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].slotNum = i;
            if (i < inventory.SlotCount)
            {
                slots[i].GetComponent<Button>().interactable = true;
            }
            else
            {
                slots[i].GetComponent<Button>().interactable = false;
            }
        }
    }

    public  void ExpansionSlot() //슬롯 추가
    {
        inventory.SlotCount++;
        if (inventory.SlotCount >= 28)
        {
            ExpansionButton.interactable = false;
        }
    }

    #region 버튼 온오프 관리
    void OnOffStats()
    {
        activeStatsUI = !activeStatsUI;
   
        activeCombination = false;
        activeDissolution = false;
    }
    void OnOffCombination()
    {
        activeCombination = !activeCombination;

        activeStatsUI = false;
        activeDissolution = false;
    }
    void OnOffDissolution()
    {
        activeDissolution = !activeDissolution;
 
        activeStatsUI = false;
        activeCombination = false;
    }
    #endregion
   


   public void RedrawSlotUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].RemoveSlot();
        }
        for (int i = 0; i < inventory.items.Count; i++)
        {
            slots[i].item = inventory.items[i];
            slots[i].UpdateSlotUI();
        }
    }


    #endregion
}
