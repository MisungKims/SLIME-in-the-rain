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
    private Image[] runeImage;

    Color color;
    float fadeInSpeed = 0.01f;
    float typingSpeed = 0.1f;
    float waitTime = 2f;

    SingletonManager singletonManager;
    Slime slime;
    SceneDesign sceneDesign;
    JellyManager jellyManager;
    Inventory inventory;
    ICamera _camera;


    // Start is called before the first frame update
    void Start()
    {
        //싱글톤
        singletonManager = SingletonManager.Instance;
        slime = Slime.Instance;
        sceneDesign = SceneDesign.Instance;
        jellyManager = JellyManager.Instance;
        inventory = Inventory.Instance;
        _camera = ICamera.Instance;


        singletonManager.Init_Result();




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
        if (sceneDesign.finalClear == true)
        {
            titleText.text = "CLEAR!!!";
            titleText.color = new Color(1f, 0f, 0f, 1f);
            StartCoroutine(CameraShake.StartShake(1f, 10f));
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
        int reachedStage = (sceneDesign.mapCounting - sceneDesign.bossLevel);
        if (reachedStage % sceneDesign.stageNum == 0)
        {
            if (sceneDesign.s_nomal - 1 > reachedStage)
            {
                stringArr[0] = "도달한 스테이지: " + (reachedStage / sceneDesign.stageNum).ToString() + "-" + (reachedStage % sceneDesign.stageNum).ToString();
            }
            else
            {
                stringArr[0] = "도달한 스테이지: " + (reachedStage / sceneDesign.stageNum).ToString() + "-" + "Boss";
            }
        }
        else
        {
            stringArr[0] = "도달한 스테이지: " + (reachedStage / sceneDesign.stageNum).ToString() + "-" + (reachedStage % sceneDesign.stageNum).ToString();
        }

        textMeshArr[1] = playtimeText;
        Debug.Log(sceneDesign.Timer);
        int hour = (int)(sceneDesign.Timer / 3600);
        int min = (int)((sceneDesign.Timer - (3600 * hour)) / 60);
        int sec = (int)((sceneDesign.Timer - ((3600 * hour) + (60 * min))));
        stringArr[1] = "플레이 타임: " + hour.ToString("D2") + ":" + min.ToString("D2") + ":" + sec.ToString("D2");

        textMeshArr[2] = killcountText;
        stringArr[2] = "잡은 몬스터 수: " + slime.killCount;

        textMeshArr[3] = jellycountText;
        stringArr[3] = "획득 젤리량: " + (jellyManager.JellyCount - sceneDesign.jellyInit).ToString();

        StartCoroutine(Typing(textMeshArr, stringArr, typingSpeed));

        //GetRune();
    }
    //3. 룬  <-- 순서대로 나오고싶어요...
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

        //Save
        PlayerPrefs.SetInt("jellyCount", jellyManager.JellyCount);
    }

    void ResultGelatin()
    {
        for (int i = 0, count = 0; i < inventory.items.Count; i++)
        {
            if (inventory.items[i].itemType == ItemType.gelatin)
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
