using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ResultCanvas : MapManager
{
    [Header("")]
    public TextMeshProUGUI titleText;
    [Header("")]
    public TextMeshProUGUI stageText;
    public TextMeshProUGUI playtimeText;
    public TextMeshProUGUI killcountText;
    public TextMeshProUGUI jellycountText;
    [Header("")]
    public GameObject gelatinObj;
    [Header("")]
    public Button villageButton;
    public Button titleButton;

    //룬
    private Transform runeSlot;
    private Image[] runeImage;

    Color color;
    float fadeInSpeed = 0.01f;
    float typingSpeed = 0.1f;
    float waitTime = 2f;

    Slime slime;
    SceneDesign sceneDesign;
    StatManager statManager;
    JellyManager jellyManager;
    RuneManager runeManager;
    Inventory inventory;

    
    // Start is called before the first frame update
    void Start()
    {
        //싱글톤
        slime = Slime.Instance;
        sceneDesign = SceneDesign.Instance;
        statManager = StatManager.Instance;
        jellyManager = JellyManager.Instance;
        runeManager = RuneManager.Instance;
        inventory = Inventory.Instance;

        runeSlot = runeManager.gameObject.transform.GetChild(0);

        Init();




        //for (int i = 0; i < runeImage.Length; i++)
        //{
        //    Color color = runeImage[i].color;
        //    color.a = 0;
        //    runeImage[i].color = color;
        //}

        //초기화
        titleText.text = "";
        titleText.color = new Color32(255, 0, 0, 0);
        stageText.text = "";
        playtimeText.text = "";
        killcountText.text = "";
        jellycountText.text = "";

        //OnClick
        villageButton.onClick.AddListener(delegate { ClickButton(1); });
        titleButton.onClick.AddListener(delegate { ClickButton(0); });

        //타이틀 -> 결과Texting -> 룬 -> 젤라틴 순으로 뜹니다
        StartCoroutine(TitleText());
        ResultGelatin();

    }

    

    //1. 타이틀
    IEnumerator TitleText()
    {
        if(sceneDesign.finalClear == true)
        {
            titleText.text = "CLEAR!!!";
            titleText.color = new Color(1f, 0f, 0f, 1f);
            StartCoroutine(CameraShake.StartShake(1f,10f));
        }
        else
        {
            titleText.text = "DEAD...";
            float fadeIn = 0;
            while (fadeIn <= 1.0f)
            {
                fadeIn += 0.01f;
                yield return new WaitForSeconds(fadeInSpeed);
                titleText.color = new Color(1f, 0f, 0f, fadeIn);
            }
        }
        TypingAll();
    }
    
    //2-2. 타이핑 효과
    IEnumerator Typing(TextMeshProUGUI[] typingText, string[] message, float speed)
    {
        for (int i = 0; i < typingText.Length; i++)
        {
            for (int j = 0; j < message[i].Length; j++)
            {
                typingText[i].text = message[i].Substring(0, j + 1);
                yield return new WaitForSeconds(speed);
            }
        }
    }


    //2-1. 타이핑 <- 코루틴
    void TypingAll()
    {
        TextMeshProUGUI[] textMeshArr = new TextMeshProUGUI[4];
        string[] stringArr = new string[4];

        //결과 타이핑 
        textMeshArr[0] = stageText;
        int stage = 5;
        int reachedStage = (sceneDesign.mapCounting - sceneDesign.bossLevel);
        if (reachedStage % stage == 0)
        {
            if(5>reachedStage)
            {
                stringArr[0] = "도달한 스테이지: " + (reachedStage / stage).ToString() + "-" + (reachedStage % stage).ToString();
            }
            else
            {
                stringArr[0] = "도달한 스테이지: " + (reachedStage / stage).ToString() + "-" + "Boss";
            }
        }
        else
        {
            stringArr[0] = "도달한 스테이지: " + (reachedStage / stage).ToString() + "-" + (reachedStage % stage).ToString();
        }

        textMeshArr[1] = playtimeText;
        Debug.Log(sceneDesign.Timer);
        int hour = (int)(sceneDesign.Timer / 3600);
        Debug.Log(hour);
        int min = (int)((sceneDesign.Timer - (3600 * hour))/60);
        Debug.Log(min);
        int sec = (int)((sceneDesign.Timer - ((3600 * hour) + (60 * min))));
        Debug.Log(sec);
        stringArr[1] = "플레이 타임: " + hour.ToString("D2") +":"+min.ToString("D2")+":"+sec.ToString("D2");

        textMeshArr[2] = killcountText;
        stringArr[2] = "잡은 몬스터 수: "+ slime.killCount;

        textMeshArr[3] = jellycountText;
        stringArr[3] = "획득 젤리량: " + (jellyManager.JellyCount- sceneDesign.jellyInit).ToString();

        StartCoroutine(Typing(textMeshArr, stringArr, typingSpeed));

        //GetRune();
    }
    //3. 룬
    void GetRune()
    {
        for (int i = 0; i < runeImage.Length; i++)
        {
            for (float j = 0; j <= 1f; j += 0.1f)
            {
                Color color = runeImage[i].color;
                color.a = j;
                runeImage[i].color = color;
            }
        }   
    }


    //Last. 버튼 onClick
    void ClickButton(int sceneNum)
    {
        SceneManager.LoadScene(sceneNum);
    }
    //싱글톤 Transform 재정의
    void Init()
    {

        slime.transform.localScale = Vector3.one * 500f;
        Vector3 pos;
        pos.x = 410f; pos.y = 250f; pos.z = 0;
        runeSlot.position = pos;
        runeSlot.localScale = Vector3.one * 1.2f;
    }

    void ResultGelatin()
    {
        for (int i = 0, count = 0; i < inventory.items.Count; i++) 
        {
            if(inventory.items[i].itemType == ItemType.gelatin)
            {
                Vector3 pos = gelatinObj.transform.position;
                pos.x -= (count * 70);
                GameObject _image = Instantiate(gelatinObj, pos, Quaternion.Euler(Vector3.zero));
                _image.transform.parent = gelatinObj.transform.parent;
                _image.SetActive(true);
                _image.GetComponent<Image>().sprite = inventory.items[i].itemIcon;
                _image.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (inventory.items[i].itemCount).ToString();
                count++;
            }
        }
    }
}
