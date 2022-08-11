using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TowerManager : MonoBehaviour
{
    #region 변수
    public TextMeshProUGUI farmNameText;
    public Button priceButton;
    public TextMeshProUGUI farmPriceText;
    public TextMeshProUGUI farmExplainText;

    //private
    string level;

    //singleton
    JellyManager jellyManager;

    #endregion

    #region 유니티 함수
    private void Start()
    {
        
        //singleton
        jellyManager = JellyManager.Instance;
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

    #region 함수
    void ClickEvent()
    {
        if (jellyManager.JellyCount >= int.Parse(farmPriceText.text))
        {
            level = (int.Parse(level) + 1).ToString();
            PlayerPrefs.SetString(TowerCollider.thisObject.name + "level", level);
            Texting();
            TowerCollider.thisObject.GetComponent<FarmManager>().TowerBuilding(1);
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
        //현재 타워 정보 정리
        switch (TowerCollider.thisObject.name)
        {
            case "MaxHP":
                farmNameText.text = "체력 버섯 농장";
                farmExplainText.text = "[ 최대 체력 ]" + " +" + level;
                farmPriceText.text = ((int)(intLevel * intLevel)).ToString();
                break;
            case "CoolTime":
                farmNameText.text = "쿨감 버섯 농장";
                farmExplainText.text = "[ 쿨타임 감소 ]" + " +" + level;
                farmPriceText.text = ((int)(intLevel * intLevel)).ToString();
                break;
            case "MoveSpeed":
                farmNameText.text = "이속 꽃 농장";
                farmExplainText.text = "[ 이동속도 ]" + " +" + level;
                farmPriceText.text = ((int)(intLevel * intLevel)).ToString();
                break;
            case "AttackSpeed":
                farmNameText.text = "공속 꽃 농장";
                farmExplainText.text = "[ 공격 속도 ]" + " +" + level;
                farmPriceText.text = ((int)(intLevel * intLevel)).ToString();
                break;
            case "AttackPower":
                farmNameText.text = "힘 버섯 농장";
                farmExplainText.text = "[ 힘 ]" + " +" + level;
                farmPriceText.text = ((int)(intLevel * intLevel)).ToString();
                break;
            case "AttackRange":
                farmNameText.text = "사거리 버섯 농장";
                farmExplainText.text = "[ 사거리 ]" + " +" + level;
                farmPriceText.text = ((int)(intLevel * intLevel)).ToString();
                break;
            case "DefensePower":
                farmNameText.text = "방어력 버섯 농장";
                farmExplainText.text = "[ 방어력 ]" + " +" + level;
                farmPriceText.text = ((int)(intLevel * intLevel)).ToString();
                break;
            case "InventorySlot":
                farmNameText.text = "인벤 슬롯 확장 광석";
                farmExplainText.text = "[ 기본 인벤토리 슬롯 ]" + " +" + level;
                farmPriceText.text = ((int)(intLevel * intLevel)+100).ToString();
                break;
            case "Empty":
            default:
                break;
        }
    }
    public void Delete()
    {
        PlayerPrefs.DeleteAll();
    }
    #endregion



}
