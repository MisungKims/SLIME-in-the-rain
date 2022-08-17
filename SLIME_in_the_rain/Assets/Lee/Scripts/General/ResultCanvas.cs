using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResultCanvas : MonoBehaviour
{
    SceneDesign sceneDesign;
    StatManager statManager;
    JellyManager jellyManager;
    RuneManager runeManager;

    public TextMeshProUGUI titleText;
    public TextMeshProUGUI stageText;
    public TextMeshProUGUI playtimeText;
    public TextMeshProUGUI killcountText;
    public TextMeshProUGUI jellycountText;
    public List<Image> runeList;
    public Transform gelatinResult;

    Color color;
    float fadeInSpeed = 0.01f;
    float typingSpeed = 0.1f;
    float waitTime = 2f;

    // Start is called before the first frame update

    void Start()
    {
        //싱글톤
        sceneDesign = SceneDesign.Instance;
        statManager = StatManager.Instance;
        jellyManager = JellyManager.Instance;
        runeManager = RuneManager.Instance;


        //초기화
        titleText.text = "";
        titleText.color = new Color32(255, 0, 0, 0);
        stageText.text = "";
        playtimeText.text = "";
        killcountText.text = "";
        jellycountText.text = "";

        StartCoroutine(TitleText());
        Invoke("TypingAll", 2f);
    }

    //타이핑 효과
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
        
    }
    void TypingAll()
    {

        TextMeshProUGUI[] textMeshArr = new TextMeshProUGUI[4];
        string[] stringArr = new string[4];

        //결과 타이핑 
        textMeshArr[0] = stageText;
        int stage = 5 + 1;
        stringArr[0] = "도달한 스테이지: " + (sceneDesign.mapCounting / stage).ToString() + "-" + (sceneDesign.mapCounting % stage).ToString();

        textMeshArr[1] = playtimeText;
        Debug.Log(sceneDesign.Timer);
        int hour = (int)(sceneDesign.Timer / 3600);
        int min = (int)((sceneDesign.Timer - (3600 * hour))/60);
        int sec = (int)((sceneDesign.Timer - ((3600 * hour) + (60 * min)))/60);
        stringArr[1] = "플레이 타임: " + hour.ToString("D2") +":"+min.ToString("D2")+":"+sec.ToString("D2");

        textMeshArr[2] = killcountText;
        stringArr[2] = "잡은 몬스터 수: 00";

        textMeshArr[3] = jellycountText;
        stringArr[3] = "획득 젤리량: " + (jellyManager.JellyCount- sceneDesign.jellyInit).ToString();

        StartCoroutine(Typing(textMeshArr, stringArr, typingSpeed));
    }

}
