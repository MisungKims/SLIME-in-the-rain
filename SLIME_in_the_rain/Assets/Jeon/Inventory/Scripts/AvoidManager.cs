using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AvoidManager : MonoBehaviour
{
    #region 싱글톤
    private static AvoidManager instance = null;
    public static AvoidManager Instance
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

    void Awake()
    {
        if (null == instance)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    public TextMeshProUGUI countDownText;
    public TextMeshProUGUI startTimeText;
    private int startTime;
    private float playCountTime;
   public bool isplay= false;

    void Start()
    {
         StartCoroutine(Restart_Timer());
    }

    IEnumerator Restart_Timer()
    {
        playCountTime = 60.0f;
        startTimeText.gameObject.SetActive(true);
        startTime = 3;
        startTimeText.text = startTime.ToString();
        yield return new WaitForSeconds(0.9f);
        startTimeText.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        startTimeText.gameObject.SetActive(true);
        startTime = 2;
        startTimeText.text = startTime.ToString();
        yield return new WaitForSeconds(0.9f);
        startTimeText.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        startTimeText.gameObject.SetActive(true);
        startTime = 1;
        startTimeText.text = startTime.ToString();
        yield return new WaitForSeconds(0.9f);
        startTimeText.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        startTimeText.gameObject.SetActive(true);
        startTime = 0;
        startTimeText.text = startTime.ToString();
        yield return new WaitForSeconds(1f);
        startTimeText.gameObject.SetActive(false);
        isplay = true;
    }

    private void countDown()
    {
        if (0 < playCountTime)
        {
            playCountTime -= Time.deltaTime;
            countDownText.text = $"{playCountTime:N1}";
        }
        else
        {
            playCountTime = 0;
            isplay = false;
            countDownText.text = $"{playCountTime:N1}";
            //다음 게임으로 넘어가는 상황
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (isplay)
        {
            countDown();
        }
    }
}
