using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CombinationUI : MonoBehaviour
{
    #region 싱글톤
    private static CombinationUI instance = null;
    public static CombinationUI Instance
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

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;


    }
    [Header("Text")]
    public TextMeshProUGUI WarningTxt;
    public TextMeshProUGUI gelatin1Txt;
    public TextMeshProUGUI gelatin2Txt;
    
    private Sprite lastComGelatin;
    private Sprite lastGelatin1;
    private Sprite lastGelatin2;
    [Header("Image")]
    public Image ComGelatin;
    public Image gelatin1;
    public Image gelatin2;
    
    public TMP_InputField countInputField;
    public GameObject input;
    private int countInput = 0;
    private int SelcetNum = -1;

    public Item gelatin1It;
    public Item gelatin2It;
    public Item ComGelatinIt;

    public int gelatin1Cont;
    public int gelatin2Cont;

    private int slotNum1 = -1;
    private int slotNum2 = -1;
    Item SelectItem;
    bool firstSet = false;
    bool secondGelatin = false;
    bool secondCount = false;
    bool sameGel = false;

    InventoryUI inventoryUI;
    Inventory inventory;


    //꺼질때, 사용시 초기화도 해줘야함

    private void OnEnable()
    {
        if (firstSet == false)
        {
            lastComGelatin = ComGelatin.sprite;
            lastGelatin1 = gelatin1.sprite;
            lastGelatin2 = gelatin2.sprite;
            firstSet = true;
        }

        ResetData();
    }
    private void Start()
    {
        inventory = Inventory.Instance;
        inventoryUI = InventoryUI.Instance;

        countInputField.onValueChanged.AddListener(ValueChanged);
    }

    private void OnDisable()
    {
        ResetData();
    }


    void ValueChanged(string text) //최댓값이 개수가 넘지 않도록
    {
        if (int.Parse(text) >= SelectItem.itemCount)
        {
            countInputField.text = SelectItem.itemCount.ToString();
        }
    }

    public void inputEnter() //개수 받는 버튼
    {
        if (int.TryParse(countInputField.text, out int i))
        {
            if (i > 0)
            {
                countInput = i;
                CombinationUIGelatin(countInput);
                countInputField.text = 0.ToString();
                countInputField.transform.parent.gameObject.SetActive(false);
            }
            else
            {
                StartCoroutine(Wt("1이상의 수를 입력해주세요."));
            }
        }
        else
        {
            StartCoroutine(Wt("입력오류"));
        }
    }

    


    public void CombinationUIGelatin(int _Count) //젤라틴 개수 받기
    {
        if (!secondCount)
        {
            gelatin1Cont = _Count;
            gelatin1Txt.text = gelatin1Cont.ToString();
            
            secondCount = true;
        }
        else
        {
            gelatin2Cont = _Count;
            gelatin2Txt.text = gelatin2Cont.ToString();
            secondCount = false;
        }
    }

    public void GelatinCount() //젤라틴 정보 받기
    {
        if(!secondGelatin)
        {
            gelatin1It = SelectItem;
            gelatin1.sprite = gelatin1It.itemIcon;
            slotNum1 = SelcetNum;
            secondGelatin = true;
        }
        else
        {
            gelatin2It = SelectItem;
            gelatin2.sprite = gelatin2It.itemIcon;
            slotNum2 = SelcetNum;
            secondGelatin = false;
        }
    }
    public void WeaponDisGelatinAdd()//버튼 작동
    {
        if (gelatin1It != null && gelatin2It != null && gelatin1Cont >=0&& gelatin2Cont>=0)
        {

            if (inventory.SlotCount - inventory.items.Count >= 1)
            {
                ComList(gelatin1It, gelatin1Cont, gelatin2It, gelatin2Cont);
            }

            else
            {
                StartCoroutine(Wt("인벤토리 공간이 부족합니다."));
            }
        }
        else
        {
            StartCoroutine(Wt("2가지 젤라틴을 선택해주세요."));
        }
    }



    void addItem(Item _item) //아이템 추가
    {
        inventory.items.Add(_item);
        inventory.items[inventory.items.Count - 1].itemCount += 1;

        if (inventory.onChangedItem != null)
        {
            inventory.onChangedItem.Invoke();
        }
    }



    IEnumerator Wt(string _str) //경고문구
    {
        WarningTxt.text = _str;
        WarningTxt.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        WarningTxt.gameObject.SetActive(false);
    }

    private void ResetData() //리셋
    {
        ComGelatin.sprite = lastComGelatin;
        gelatin1.sprite = lastGelatin1;
        gelatin2.sprite = lastGelatin2;

        countInputField.transform.parent.gameObject.SetActive(false);
        countInput = 0;
        gelatin1It = null;
        gelatin2It = null;
        gelatin1Cont = -1;
        gelatin2Cont = -1;
        ComGelatinIt = null;
        SelcetNum = -1;
        slotNum1 = -1;
        slotNum2 = -1;
        secondGelatin = false;
        secondCount = false;
        gelatin1Txt.text = "";
        gelatin2Txt.text = "";
    }

    public void inputEndCount(int _slotNum)
    {

        countInputField.transform.parent.gameObject.SetActive(true);

        SelectItem = inventory.items[_slotNum];
        SelcetNum = _slotNum;
        GelatinCount();



    }

    #region 젤라틴조합리스트
    void ComList(Item _gelatin1It,int _gelatin1Cont, Item _gelatin2It, int _gelatin2Cont)
    {
        string gelatin1St = _gelatin1It.itemName;
        string gelatin2St = _gelatin2It.itemName;

        switch (gelatin1St)
        {
            case "CyanGelatin":
                if (_gelatin1Cont == 1&& _gelatin2Cont == 1)
                {
                    switch (gelatin2St)
                    {
                        case "YellowGelatin":
                            ComGelatinIt = Comb( "GreenGelatin");
                            break;
                        case "MagentaGelatin":
                            ComGelatinIt = Comb("BlueGelatin");
                            break;
                        default:
                            faildComb();
                            break;
                    }
                }
                else
                {
                    faildComb();
                }
                break;
            case "YellowGelatin":
                if (_gelatin1Cont == 1 && _gelatin2Cont == 1)
                {
                    switch (gelatin2St)
                    {
                        case "CyanGelatin":
                            ComGelatinIt = Comb("GreenGelatin");
                            break;
                        case "MagentaGelatin":
                            ComGelatinIt = Comb("RedGelatin");
                            break;
                        default:
                            faildComb();
                            break;
                    }
                }
                else if (_gelatin1Cont == 14 && _gelatin2Cont == 6)
                {
                    switch (gelatin2St)
                    {
                        case "GreenGelatin":
                            ComGelatinIt = Comb("LightGreenGelatin");
                            break;
                        case "RedGelatin":
                            ComGelatinIt = Comb("OrangeGelatin");
                            break;
                        default:
                            faildComb();
                            break;
                    }
                }
                else
                {
                    faildComb();
                }
                break;
            case "MagentaGelatin":
                if (_gelatin1Cont == 1 && _gelatin2Cont == 1)
                {
                    switch (gelatin2St)
                    {
                        case "CyanGelatin":
                            ComGelatinIt = Comb("BlueGelatin");
                            break;
                        case "YellowGelatin":
                            ComGelatinIt = Comb("RedGelatin");
                            break;
                        default:
                            faildComb();
                            break;
                    }
                }
                else
                {
                    faildComb();
                }
                break;
            case "RedGelatin":
                if (_gelatin1Cont == 10 && _gelatin2Cont == 10)
                {
                    switch (gelatin2St)
                    {
                        case "BlueGelatin":
                            ComGelatinIt = Comb("PupleGelatin");
                            break;
                        default:
                            faildComb();
                            break;
                    }
                }
                else if (_gelatin1Cont == 6 && _gelatin2Cont == 14)
                {
                    switch (gelatin2St)
                    {
                        case "WhiteGelatin":
                            ComGelatinIt = Comb("PinkGelatin");
                            break;
                        case "YellowGelatin":
                            ComGelatinIt = Comb("OrangeGelatin");
                            break;
                        default:
                            faildComb();
                            break;
                    }
                }
                else
                {
                    faildComb();
                }
                break;
            case "GreenGelatin":
                if (_gelatin1Cont == 6 && _gelatin2Cont == 14)
                {
                    switch (gelatin2St)
                    {
                        case "YellowGelatin":
                            ComGelatinIt = Comb("OrangeGelatin");
                            break;
                        default:
                            faildComb();
                            break;
                    }
                }
                else if (_gelatin1Cont == 16 && _gelatin2Cont == 4)
                {
                    switch (gelatin2St)
                    {
                        case "MagentaGelatin":
                            ComGelatinIt = Comb("NavyGelatin");
                            break;
                        default:
                            faildComb();
                            break;
                    }
                }
                else
                {
                    faildComb();
                }
                break;
            case "WhiteGelatin":
                if (_gelatin1Cont == 14 && _gelatin2Cont == 6)
                {
                    switch (gelatin2St)
                    {
                        case "RedGelatin":
                            ComGelatinIt = Comb("PinkGelatin");
                            break;
                        default:
                            faildComb();
                            break;
                    }
                }
                else if (_gelatin1Cont == 16 && _gelatin2Cont == 4)
                {
                    switch (gelatin2St)
                    {
                        case "BlueGelatin":
                            ComGelatinIt = Comb("SkyGelatin");
                            break;
                        default:
                            faildComb();
                            break;
                    }
                }
                else
                {
                    faildComb();
                }
                break;
            case "BlueGelatin":
                if (_gelatin1Cont == 4 && _gelatin2Cont == 16)
                {
                    switch (gelatin2St)
                    {
                        case "WhiteGelatin":
                            ComGelatinIt = Comb("SkyGelatin");
                            break;
                        default:
                            faildComb();
                            break;
                    }
                }
                else if (_gelatin1Cont == 10 && _gelatin2Cont == 10)
                {
                    switch (gelatin2St)
                    {
                        case "RedGelatin":
                            ComGelatinIt = Comb("PupleGelatin");
                            break;
                        default:
                            faildComb();
                            break;
                    }
                }
                else
                {
                    faildComb();
                }
                break;
            default:
                faildComb();
                break;
        }
        if (ComGelatinIt != null)
        {
            StartCoroutine(comok(_gelatin1Cont, _gelatin2Cont));
        }
        inventoryUI.RedrawSlotUI();
    }

    IEnumerator comok(int _gelatin1Cont, int _gelatin2Cont)
    {
        ComGelatin.sprite = ComGelatinIt.itemIcon;
        StartCoroutine(Wt("젤라틴 합성 성공"));
        yield return new WaitForSeconds(2.0f);
        sameGel = false;
        inventory.items[slotNum1].itemCount -= _gelatin1Cont;
        inventory.items[slotNum2].itemCount -= _gelatin2Cont;
        for (int i = 0; i < inventory.items.Count; i++)
        {
            if (inventory.items[i].itemName == ComGelatinIt.itemName)
            {
                inventory.items[i].itemCount++;
                sameGel = true;
                break;
            }
        }
        if (!sameGel)
        {
            addItem(ComGelatinIt);
        }
        ResetData();
        inventoryUI.RedrawSlotUI();
    }

    #endregion
    void faildComb()
    {
        inventory.items[slotNum1].itemCount--;
        inventory.items[slotNum2].itemCount--;
        StartCoroutine(Wt("실패했습니다."));
        ResetData();
        inventoryUI.RedrawSlotUI();
    }

    /*IEnumerator enterCouru()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            inputEnter();
        }

        yield return new WaitForSeconds(0.1f);
    }*/

    Item Comb(string flag)
    {
        FieldItems tempGb;
        tempGb = ObjectPoolingManager.Instance.Get2(flag);

        return tempGb.item;
    }
}


