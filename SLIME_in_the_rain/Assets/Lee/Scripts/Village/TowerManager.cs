using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TowerManager : MonoBehaviour
{
    #region ����
    public TextMeshProUGUI farmNameText;
    public Button priceButton;
    public TextMeshProUGUI farmPriceText;
    public TextMeshProUGUI farmExplainText;

    //private
    string level;

    //singleton
    JellyManager jellyManager;
    StatManager statManager;
    InventoryUI inventoryUI;

    #endregion

    #region ����Ƽ �Լ�
    private void Start()
    {

        //singleton
        jellyManager = JellyManager.Instance;
        statManager = StatManager.Instance;
        inventoryUI = InventoryUI.Instance;
        //OnClick
        priceButton.onClick.AddListener(delegate { ClickEvent(); });
        
    }
    private void OnEnable()
    {
        if(TowerCollider.thisObject != null)
        {
            Texting();
        }
    }
    #endregion

    #region �Լ�
    void ClickEvent()
    {
        if ((jellyManager.JellyCount - int.Parse(farmPriceText.text)) > 0)
        {
            level = (int.Parse(level) + 1).ToString();
            StatUp();
            PlayerPrefs.SetString(TowerCollider.thisObject.name + "level", level);
            Texting();
            TowerCollider.thisObject.GetComponent<FarmManager>().TowerBuilding(1);
            
            jellyManager.JellyCount -= int.Parse(farmPriceText.text);
        }
        else
        {
            this.transform.parent.GetComponent<VillageCanvas>().PanelCorou();
        }
    }
    void Texting()
    {
        level = PlayerPrefs.GetString(TowerCollider.thisObject.name + "level");
        int intLevel = int.Parse(level);
        //���� Ÿ�� ���� ����
        switch (TowerCollider.thisObject.name)
        {
            case "MaxHP":
                farmNameText.text = "ü�� ���� ����";
                farmExplainText.text = "[ �ִ� ü�� ]" + " +" + level;
                farmPriceText.text = ((int)(intLevel * intLevel) + 10).ToString();
                break;
            case "CoolTime":
                farmNameText.text = "�� ���� ����";
                farmExplainText.text = "[ ��Ÿ�� ���� ]" + " +" + level;
                farmPriceText.text = ((int)(intLevel * intLevel) + 10).ToString();
                break;
            case "MoveSpeed":
                farmNameText.text = "�̼� �� ����";
                farmExplainText.text = "[ �̵��ӵ� ]" + " +" + level;
                farmPriceText.text = ((int)(intLevel * intLevel) + 10).ToString();
                break;
            case "AttackSpeed":
                farmNameText.text = "���� �� ����";
                farmExplainText.text = "[ ���� �ӵ� ]" + " +" + level;
                farmPriceText.text = ((int)(intLevel * intLevel) + 10).ToString();
                break;
            case "AttackPower":
                farmNameText.text = "�� ���� ����";
                farmExplainText.text = "[ �� ]" + " +" + level;
                farmPriceText.text = ((int)(intLevel * intLevel) + 10).ToString();
                break;
            case "AttackRange":
                farmNameText.text = "��Ÿ� ���� ����";
                farmExplainText.text = "[ ��Ÿ� ]" + " +" + level;
                farmPriceText.text = ((int)(intLevel * intLevel) + 10).ToString();
                break;
            case "DefensePower":
                farmNameText.text = "���� ���� ����";
                farmExplainText.text = "[ ���� ]" + " +" + level;
                farmPriceText.text = ((int)(intLevel * intLevel) + 10).ToString();
                break;
            case "InventorySlot":
                farmNameText.text = "�κ� ���� Ȯ�� ����";
                farmExplainText.text = "[ �⺻ �κ��丮 ���� ]" + " +" + level;
                farmPriceText.text = ((int)(intLevel * intLevel)+100).ToString();
                break;
            case "Empty":
            default:
                break;
        }
    }
    void StatUp()
    {
        level = PlayerPrefs.GetString(TowerCollider.thisObject.name + "level");
        float floatLevel = float.Parse(level);
        //���� Ÿ�� ���� ����
        switch (TowerCollider.thisObject.name)
        {
            case "MaxHP":
                statManager.AddMaxHP(0.1f);
                break;
            case "CoolTime":
                statManager.AddCoolTime(0.1f);
                break;
            case "MoveSpeed":
                statManager.AddMaxHP(0.1f);
                break;
            case "AttackSpeed":
                statManager.AddMaxHP(0.1f);
                break;
            case "AttackPower":
                statManager.AddMaxHP(0.1f);
                break;
            case "AttackRange":
                statManager.AddMaxHP(0.1f);
                break;
            case "DefensePower":
                statManager.AddMaxHP(0.1f);
                break;
            case "InventorySlot":
                inventoryUI.ExpansionSlot(1);
                break;
            case "Empty":
            default:
                break;
        }
    }
    #endregion



}
