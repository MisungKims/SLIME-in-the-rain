using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    #region ����
    #region �̱���
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

    Inventory inventory; //�κ��丮
    [SerializeField]
    private Button ExpansionButton; //�κ�Ȯ���ư
    [SerializeField]
    public Slot[] slots;
    public Transform slotHolder;
    [SerializeField]
    private FadeOutText wT;

    [Header("GbUI")]
    public GameObject inventroyPanel;
    public GameObject statsUI;
    public GameObject CombinationUI;
    public GameObject DissolutionUI;

    private JellyManager jellyManager;
    public TextMeshProUGUI JellyTextC; //����
    [SerializeField]
    int expansCost = 5;


    public GameObject tooltip;

    #region onOffBool
    public bool activeInventory = false;
    public bool activeStatsUI = false;
    public bool activeCombination = false;
    public bool activeDissolution = false;
    #endregion
    #endregion


    #region ����Ƽ �޼ҵ�
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
            StartCoroutine(inventory.RedrawCoru());
            activeInventory = !activeInventory;
            inventroyPanel.SetActive(activeInventory);

            if (!inventroyPanel.activeSelf && tooltip.activeSelf)
            {
                tooltip.SetActive(false);
            }
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
    #endregion

    #region �޼ҵ�
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

    public void ExpansionSlot() //���� �߰�
    {

        if (jellyManager.JellyCount >= expansCost)
        {
            inventory.SlotCount++;
            if (inventory.SlotCount >= 28)
            {
                ExpansionButton.interactable = false;
            }
            expansCost += expansCost;
        }
        else
        {
            wT.ShowText();
        }

    }

    public void ExpansionSlot(int _level)
    {
        inventory.SlotCount += _level;
        if (inventory.SlotCount >= 28)
        {
            ExpansionButton.interactable = false;
        }
    }
    #region ��ư �¿��� ����
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
        for (int i = 0; i < inventory.items.Count; i++)
        {
            if (inventory.items[i].itemCount <= 0)
            {
                inventory.items.RemoveAt(i);
            }
        }

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].RemoveSlot();
        }
        for (int i = 0; i < inventory.items.Count; i++)
        {
            slots[i].item = inventory.items[i];
            slots[i].UpdateSlotUI();
        }
        inventory.statGelatinAdd();

    }
    public void ShowTooltip(Item _item)
    {
        tooltip.SetActive(true);
        StatsUIManager.Instance.countText.text = _item.itemCount.ToString();
        StatsUIManager.Instance.nameText.text = _item.itemExplain;


        optionSet(_item);
    }




    public void optionSet(Item _item)
    {
        StatsUIManager.Instance.optionText.text = "";
        string skill;
        if (_item.itemType == ItemType.weapon)
        {
            switch (_item.itemName)
            {
                case "Dagger":
                    skill = "����";
                    break;
                case "Sword":
                    skill = "��������";
                    break;
                case "IceStaff":
                    skill = "��������";
                    break;
                case "FireStaff":
                    skill = "ȭ�����";
                    break;
                case "Bow":
                    skill = "��ä��ȭ��";
                    break;
                default:
                    skill = "";
                    break;
            }
            StatsUIManager.Instance.optionText.text += "��ų : " + skill + "\n";
        }
        if (float.Parse(_item.maxHp) > 0)
        {
            StatsUIManager.Instance.optionText.text += "�ִ�ü�� : " + _item.maxHp + "\n";
        }
        if (float.Parse(_item.coolTime) > 0)
        {
            StatsUIManager.Instance.optionText.text += "��Ÿ�� : " + _item.coolTime + "%\n";
        }
        if (float.Parse(_item.moveSpeed) > 0)
        {
            StatsUIManager.Instance.optionText.text += "�̵��ӵ� : " + _item.moveSpeed + "\n";
        }
        if (float.Parse(_item.atkSpeed) > 0)
        {
            StatsUIManager.Instance.optionText.text += "���ݼӵ� : " + _item.atkSpeed + "\n";
        }
        if (float.Parse(_item.atkPower) > 0)
        {
            StatsUIManager.Instance.optionText.text += "���ݷ� : " + _item.atkPower + "\n";
        }
        if (float.Parse(_item.atkRange) > 0)
        {
            StatsUIManager.Instance.optionText.text += "���ݹ��� : " + _item.atkRange + "\n";
        }
        if (float.Parse(_item.defPower) > 0)
        {
            StatsUIManager.Instance.optionText.text += "���� : " + _item.defPower + "\n";
        }
        if (float.Parse(_item.increase) > 0)
        {
            StatsUIManager.Instance.optionText.text += "������ ���� : " + _item.increase + "\n" + "%";
        }




    }

    #endregion
}
