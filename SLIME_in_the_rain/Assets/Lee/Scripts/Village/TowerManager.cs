using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TowerManager : MonoBehaviour
{
    #region ����
    //Singletons
    ICanvas canvas;
    JellyManager jellyManager;
    [Header("Tower Prefabs")]
    public List<GameObject> EmptyList;
    public List<GameObject> HPList;
    public List<GameObject> CoolTimeList;
    public List<GameObject> MoveSpeedList;
    public List<GameObject> AttackSpeedList;
    public List<GameObject> AttackPowerList;
    public List<GameObject> AttackRangeList;
    public List<GameObject> DefensePowerList;
    public List<GameObject> InventorySlotList;


    //private
    bool onStay = false;
    GameObject ShopCanvas;
    GameObject TowerCanvas;
    Button priceButton;
    TextMeshProUGUI farmPrice;
    TextMeshProUGUI farmName;
    TextMeshProUGUI farmExplain;
    string level;
    int intLevel;
    string statStr;
    #endregion

    #region ����Ƽ �Լ�
    // Start is called before the first frame update
    void Start()
    {
        //�̱���
        canvas = ICanvas.Instance;
        jellyManager = JellyManager.Instance;

        //Find / GetChild
        ShopCanvas = canvas.transform.Find("Shop").gameObject;
        TowerCanvas = canvas.transform.Find("Tower").gameObject;
        //towerObj -> 2��° Button -> �ڿ��� 2��°�� Updates -> TMP
        farmPrice = TowerCanvas.transform.GetChild(1).GetChild(TowerCanvas.transform.GetChild(1).childCount - 2).GetChild(0).GetComponent<TextMeshProUGUI>();
        priceButton = TowerCanvas.transform.GetChild(1).GetComponent<Button>();
        //towerObj -> �ڿ��� 3��° Updates -> �̸� TMP
        farmName = TowerCanvas.transform.GetChild(TowerCanvas.transform.childCount - 3).GetChild(0).GetComponent<TextMeshProUGUI>();
        //towerObj -> �ڿ��� 3��° Updates -> ���� TMP
        farmExplain = TowerCanvas.transform.GetChild(TowerCanvas.transform.childCount - 3).GetChild(1).GetComponent<TextMeshProUGUI>();

        //onClick
        priceButton.onClick.AddListener(delegate { ClickEvent(); });

        //Load
        Load();
        Building();
    }

    // Update is called once per frame
    private void Update()
    {
        if (onStay)
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                switch (this.tag)
                {
                    case "Shop":
                        ShopOpen();
                        break;
                    case "Tower":
                        TowerOpen();
                        break;
                    default:
                        break;
                }
            }
        }
    }
    #endregion

    #region �ݶ��̴� �Լ�
    private void OnCollisionEnter(Collision collision)
    {

        if (collision.transform.tag == "Slime")
        {
            onStay = true;
            this.transform.GetComponent<Outline>().enabled = true;
            
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.tag == "Slime")
        {
            onStay = false;
            this.transform.GetComponent<Outline>().enabled = false;
        }
    }
    #endregion

    #region �Լ�
    void ClickEvent()
    {
        if (onStay)
        {
            if (jellyManager.JellyCount > int.Parse(farmPrice.text))
            {
                Load();
                level = (int.Parse(level) + 1).ToString();
                BuildingLevelUp();
                Texting();
                Save();
            }
            else
            {
                //Ÿ�� -> �ڿ��� 2��° �ǳ�+TMP
                GameObject cantBuy = TowerCanvas.transform.GetChild(TowerCanvas.transform.childCount - 2).gameObject;
                cantBuy.SetActive(true);
                StartCoroutine(ActiveOnOffWFS(cantBuy));
            }
        }
    }
    void ShopOpen() //���� ���� ui
    {
        ShopCanvas.SetActive(true);
    }
    void TowerOpen()
    {
        Texting();
        TowerCanvas.SetActive(true);    //Ÿ��UI ON
        Save();
    }

    void Building()
    {
        List<GameObject> list = GetTowerList(this.name);

        //�ڽ����� ������Ʈ ����
        for (int i = 0; i < int.Parse(level); i++)
        {
            GameObject building;
            building = Instantiate(list[Random.Range(0, list.Count)]);
            building.transform.parent = this.transform;

            //Position
            Vector3 setPos;
            setPos.x = Random.Range(-1.9f, 1.9f);
            setPos.y = 0;
            setPos.z = -2 + Random.Range(-1.9f, 1.9f);
            building.transform.localPosition = setPos;
            building.SetActive(true);

            //Rotation
            Quaternion setRot = new Quaternion();
            setRot.y = Random.Range(0, 360);
            building.transform.rotation = setRot;

            //Scale
            int ran = Random.Range(0, 100);
            float minmax = 2.5f;
            if (ran > 99)
            {
                building.transform.localScale *= 10f;
            }
            else if(ran > 95)
            {
                building.transform.localScale *= Random.Range(minmax, 3f);
            }
            else
            {
                building.transform.localScale *= Random.Range(1f, minmax);
            }
        }
    }
    void BuildingLevelUp()
    {
        List<GameObject> list = GetTowerList(this.name);

        //�ڽ����� ������Ʈ ����
        GameObject building;
        building = Instantiate(list[Random.Range(0, list.Count)]);
        building.transform.parent = this.transform;

        //Position
        Vector3 setPos;
        setPos.x = Random.Range(-1.9f, 1.9f);
        setPos.y = 0;
        setPos.z = -2 + Random.Range(-1.9f, 1.9f);
        building.transform.localPosition = setPos;
        building.SetActive(true);

        //Rotation
        Quaternion setRot = new Quaternion();
        setRot.y = Random.Range(0, 360);
        building.transform.rotation = setRot;

        //Scale
        int ran = Random.Range(0, 100);
        float minmax = 2.5f;
        if (ran > 99)
        {
            building.transform.localScale *= 5f;
        }
        else if (ran > 95)
        {
            building.transform.localScale *= Random.Range(minmax, 3f);
        }
        else
        {
            building.transform.localScale *= Random.Range(1f, minmax);

        }
    }

    public void GetTower(string name)
    {
        switch (name)
        {
            case "MaxHP":
                farmName.text = "ü�� ���� ����";
                statStr = "�ִ� ü��";
                break;
            case "CoolTime":
                farmName.text = "�� ���� ����";
                statStr = "��Ÿ�� ����";
                break;
            case "MoveSpeed":
                farmName.text = "�̼� �� ����";
                statStr = "�̵��ӵ�";
                break;
            case "AttackSpeed":
                farmName.text = "���� �� ����";
                statStr = "���� �ӵ�";
                break;
            case "AttackPower":
                farmName.text = "�� ���� ����";
                statStr = "��";
                break;
            case "AttackRange":
                farmName.text = "��Ÿ� �� ����";
                statStr = "��Ÿ�";
                break;
            case "DefensePower":
                farmName.text = "���� �� ����";
                statStr = "����";
                break;
            case "InventorySlot":
                farmName.text = "�κ� �� ����";
                statStr = "�⺻ �κ��丮 ����";
                break;
            case "Empty":
                break;
            default:
                break;
        }
    }

    public List<GameObject> GetTowerList(string name)
    {
        switch (name)
        {
            case "MaxHP":
                return HPList;
            case "CoolTime":
                return CoolTimeList;
            case "MoveSpeed":
                return MoveSpeedList;
            case "AttackSpeed":
                return AttackSpeedList;
            case "AttackPower":
                return AttackPowerList;
            case "AttackRange":
                return AttackRangeList;
            case "DefensePower":
                return DefensePowerList;
            case "InventorySlot":
                return DefensePowerList;
            case "Empty":
                return EmptyList;
            default:
                return null;
        }
    }

    void Texting()
    {
        GetTower(this.name);
        farmExplain.text = "[" + statStr + "]" + " +" + level;
        intLevel = int.Parse(level);
        farmPrice.text = ((int)(intLevel * intLevel / 2)).ToString();
    }

    IEnumerator ActiveOnOffWFS(GameObject gameObject)
    {
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }

    void Load()
    {
        level = PlayerPrefs.GetString(this.name + "level");
        if (level == "")
        {
            level = "0";
        }
    }
    void Save()
    {
        PlayerPrefs.SetString(name + "level", level);
    }

    public void Delete()
    {
        PlayerPrefs.DeleteAll();
    }

    #endregion


}
