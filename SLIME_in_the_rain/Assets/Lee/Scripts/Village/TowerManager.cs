using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TowerManager : MonoBehaviour
{
    #region 변수
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

    #region 유니티 함수
    // Start is called before the first frame update
    void Start()
    {
        //싱글톤
        canvas = ICanvas.Instance;
        jellyManager = JellyManager.Instance;

        //Find / GetChild
        ShopCanvas = canvas.transform.Find("Shop").gameObject;
        TowerCanvas = canvas.transform.Find("Tower").gameObject;
        //towerObj -> 2번째 Button -> 뒤에서 2번째인 Updates -> TMP
        farmPrice = TowerCanvas.transform.GetChild(1).GetChild(TowerCanvas.transform.GetChild(1).childCount - 2).GetChild(0).GetComponent<TextMeshProUGUI>();
        priceButton = TowerCanvas.transform.GetChild(1).GetComponent<Button>();
        //towerObj -> 뒤에서 3번째 Updates -> 이름 TMP
        farmName = TowerCanvas.transform.GetChild(TowerCanvas.transform.childCount - 3).GetChild(0).GetComponent<TextMeshProUGUI>();
        //towerObj -> 뒤에서 3번째 Updates -> 설명 TMP
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

    #region 콜라이더 함수
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

    #region 함수
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
                //타워 -> 뒤에서 2번째 판넬+TMP
                GameObject cantBuy = TowerCanvas.transform.GetChild(TowerCanvas.transform.childCount - 2).gameObject;
                cantBuy.SetActive(true);
                StartCoroutine(ActiveOnOffWFS(cantBuy));
            }
        }
    }
    void ShopOpen() //상점 관련 ui
    {
        ShopCanvas.SetActive(true);
    }
    void TowerOpen()
    {
        Texting();
        TowerCanvas.SetActive(true);    //타워UI ON
        Save();
    }

    void Building()
    {
        List<GameObject> list = GetTowerList(this.name);

        //자식으로 오브젝트 생성
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

        //자식으로 오브젝트 생성
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
                farmName.text = "체력 버섯 농장";
                statStr = "최대 체력";
                break;
            case "CoolTime":
                farmName.text = "쿨감 버섯 농장";
                statStr = "쿨타임 감소";
                break;
            case "MoveSpeed":
                farmName.text = "이속 꽃 농장";
                statStr = "이동속도";
                break;
            case "AttackSpeed":
                farmName.text = "공속 꽃 농장";
                statStr = "공격 속도";
                break;
            case "AttackPower":
                farmName.text = "힘 버섯 농장";
                statStr = "힘";
                break;
            case "AttackRange":
                farmName.text = "사거리 꽃 농장";
                statStr = "사거리";
                break;
            case "DefensePower":
                farmName.text = "방어력 꽃 농장";
                statStr = "방어력";
                break;
            case "InventorySlot":
                farmName.text = "인벤 꽃 농장";
                statStr = "기본 인벤토리 슬롯";
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
