/**
 * @brief 오브젝트 n회 타격 시 클리어 맵
 * @author 김미성
 * @date 22-08-09
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using TMPro;

public class HitCountMap : MapManager
{
    #region 변수
    #region 싱글톤
    private static HitCountMap instance = null;
    public static HitCountMap Instance
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

    [Header("-------------- Props")]
    [SerializeField]
    private GameObject[] props;
    [SerializeField]
    private Transform[] propPos;

    [Header("-------------- Count")]
    [SerializeField]
    private TextMeshProUGUI countText;
    [SerializeField]
    private int maxCount = 50;          // 만족해야하는 타수

    private StringBuilder sb = new StringBuilder();

    private bool isClear = false;
    private int count;                  // 현재 타수
    public int Count
    {
        get { return count; }
        set
        {
            if(!isClear)
            {
                count = value;

                sb.Clear();
                sb.Append(count.ToString());
                sb.Append("<size=30>/");
                sb.Append(maxCount);

                countText.text = sb.ToString();

                if (count >= maxCount)
                {
                    ClearMap();
                }
            }
        }
    }

    [Header("-------------- Description Text")]
    // 맵 설명 텍스트
    [SerializeField]
    private GameObject descText;
    private Vector2 startTextPos = new Vector2(0, 55);
    private Vector2 endTextPos = new Vector2(0, -10);
    private RectTransform textTransform;
    private Vector3 offset;
    private float distance;

    [Header("-------------- Object Pool")]
    [SerializeField]
    private ObjectPool particlePooling;     // 파티클 오브젝트 풀링
    #endregion

    protected override void Awake()
    {
        if (null == instance)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        base.Awake();

        InitObject();
        InitMap();
    }

    private void Start()
    {
        StartCoroutine(ShowDescText());
    }

    IEnumerator DisableProps()
    {
        for (int i = 0; i < props.Length; i++)
        {
            props[i].gameObject.SetActive(false);

            yield return new WaitForSeconds(0.4f);
        }
    }

    // 맵 설명 텍스트
    IEnumerator ShowDescText()
    {
        descText.SetActive(true);
        textTransform = descText.GetComponent<RectTransform>();
        textTransform.anchoredPosition = startTextPos;

        yield return new WaitForSeconds(0.5f);

        // 텍스트가 내려옴
        offset = textTransform.anchoredPosition - endTextPos;
        distance = offset.sqrMagnitude;

        while (distance > 0.5f)
        {
            offset = textTransform.anchoredPosition - endTextPos;
            distance = offset.sqrMagnitude;

            textTransform.anchoredPosition = Vector3.Lerp(textTransform.anchoredPosition, endTextPos, Time.deltaTime * 2f);

            yield return null;
        }

        yield return new WaitForSeconds(3f);

        // 텍스트가 올라감
        offset = textTransform.anchoredPosition - startTextPos;
        distance = offset.sqrMagnitude;

        while (distance > 0.5f)
        {
            offset = textTransform.anchoredPosition - startTextPos;
            distance = offset.sqrMagnitude;

            textTransform.anchoredPosition = Vector3.Lerp(textTransform.anchoredPosition, startTextPos, Time.deltaTime * 4f);

            yield return null;
        }
    }


    #region 오브젝트 풀링
    // 파티클 오브젝트를 initCount 만큼 생성
    private void InitObject()
    {
        for (int i = 0; i < particlePooling.initCount; i++)
        {
            GameObject tempGb = GameObject.Instantiate(particlePooling.copyObj, particlePooling.parent.transform);
            tempGb.name = i.ToString();
            tempGb.gameObject.SetActive(false);
            particlePooling.queue.Enqueue(tempGb);
        }
    }

    /// <summary>
    /// 오브젝트를 반환
    /// </summary>
    public GameObject GetParticle(Vector3 pos)
    {
        GameObject tempGb;

        if (particlePooling.queue.Count > 0)             // 큐에 게임 오브젝트가 남아 있을 때
        {
            tempGb = particlePooling.queue.Dequeue();
            tempGb.SetActive(true);
        }
        else         // 큐에 더이상 없으면 새로 생성
        {
            tempGb = GameObject.Instantiate(particlePooling.copyObj, particlePooling.parent.transform);
        }

        tempGb.transform.position = pos;

        return tempGb;
    }

    /// <summary>
    /// 다 쓴 오브젝트를 큐에 돌려줌
    /// </summary>
    public void SetParticle(GameObject gb)
    {
        gb.SetActive(false);

        particlePooling.queue.Enqueue(gb);
    }
    #endregion

    // Prop의 위치를 설정
    private void InitMap()
    {
        Count = 0;
        isClear = false;

        props = ShuffleArray(props);    // 배열을 섞기

        for (int i = 0; i < props.Length; i++)
        {
            props[i].transform.position = propPos[i].position;
        }
    }

    // 배열을 랜덤으로 섞기
    private T[] ShuffleArray<T>(T[] array)
    {
        int random1, random2;
        T temp;

        for (int i = 0; i < array.Length; ++i)
        {
            random1 = Random.Range(0, array.Length);
            random2 = Random.Range(0, array.Length);

            temp = array[random1];
            array[random1] = array[random2];
            array[random2] = temp;
        }

        return array;
    }

    // 맵 클리어 시 호출
    public override void ClearMap()
    {
        isClear = true;

        StartCoroutine(DisableProps());

        base.ClearMap();
    }

   
}
