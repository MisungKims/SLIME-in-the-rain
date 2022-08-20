/**
 * @brief ������Ʈ nȸ Ÿ�� �� Ŭ���� ��
 * @author ��̼�
 * @date 22-08-09
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using TMPro;

public class HitCountMap : MapManager
{
    #region ����
    #region �̱���
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
    private GameObject prop;
    [SerializeField]
    private Transform monsters;

    [SerializeField]
    private Monster[] monsterArray;
    [SerializeField]
    private Transform[] monsterPos;

    private List<GameObject> monsterArr = new List<GameObject>();

    [SerializeField]
    private GameObject[] props;
    [SerializeField]
    private Transform[] propPos;

    [Header("-------------- Count")]
    [SerializeField]
    private TextMeshProUGUI countText;
    [SerializeField]
    private int maxCount = 50;          // �����ؾ��ϴ� Ÿ��

    private StringBuilder sb = new StringBuilder();

    private bool isClear = false;
    private int count;                  // ���� Ÿ��
    public int Count
    {
        get { return count; }
        set
        {
            if(!isClear)
            {
                count = value;

                SetCountText();
            }
        }
    }

    [Header("-------------- Description Text")]
    // �� ���� �ؽ�Ʈ
    [SerializeField]
    private GameObject descText;
    private Vector2 startTextPos = new Vector2(0, 55);
    private Vector2 endTextPos = new Vector2(0, -10);
    private RectTransform textTransform;
    private Vector3 offset;
    private float distance;

    [Header("-------------- Object Pool")]
    [SerializeField]
    private ObjectPool particlePooling;     // ��ƼŬ ������Ʈ Ǯ��
    #endregion

    #region ����Ƽ �Լ�
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

    #endregion

    #region �ڷ�ƾ
    IEnumerator DisableProps()
    {
        prop.gameObject.SetActive(false);

        yield return new WaitForSeconds(0.3f);

        Vector3 moneyPos;
        int randObj; 
        
        int randJellyCount = Random.Range(20, 30);      // ��ȭ ��� ����
        for (int i = 0; i < randJellyCount; i++)
        {
            moneyPos = prop.transform.position;     // ��ȭ�� ��ġ
            moneyPos.x += Random.Range(-1f, 1f);
            moneyPos.y += 2f;
            moneyPos.z += Random.Range(-1f, 1f);

            randObj = Random.Range(0, 2);         // Ȯ���� ���� ����, ����ƾ, ���⸦ ����
            switch (randObj)
            {
                case 0:   // ����
                    objectPoolingManager.Get(EObjectFlag.jelly, moneyPos);
                    break;
                case 1:   // ����ƾ
                    objectPoolingManager.Get(EObjectFlag.gelatin, moneyPos);
                    break;
                default:
                    objectPoolingManager.Get(EObjectFlag.jelly, moneyPos);
                    break;
            }
        }
    }

    IEnumerator MonsterDie()
    {
        for (int i = 0; i < monsterArr.Count; i++)
        {
            Transform monsterParent = monsterArr[i].transform;
            for (int j = 0; j < monsterParent.childCount; j++)
            {
                monsterParent.GetChild(j).GetComponent<Monster>().isDie = true;

                yield return null;
            }
        }
    }

    // �� ���� �ؽ�Ʈ
    IEnumerator ShowDescText()
    {
        descText.SetActive(true);
        textTransform = descText.GetComponent<RectTransform>();
        textTransform.anchoredPosition = startTextPos;

        yield return new WaitForSeconds(0.5f);

        // �ؽ�Ʈ�� ������
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

        // �ؽ�Ʈ�� �ö�
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

    IEnumerator SpawnMonster()
    {
        WaitForSeconds waitFor10s = new WaitForSeconds(10f);
        WaitForSeconds waitFor3s = new WaitForSeconds(3f);

        for (int i = 0; i < monsterArr.Count; i++)
        {
            if (i == 0) yield return waitFor3s;
            else yield return waitFor10s;

            if (isClear) break;

            monsterArr[i].SetActive(true);
        }

        while (!isClear)
        {
            for (int i = 0; i < monsterArray.Length; i++)
            {
                Debug.Log(monsterArray[i]);
                yield return waitFor10s;

                for (int j = 0; j < monsterPos.Length; j++)
                {
                    Debug.Log(monsterPos[j]);
                    Instantiate(monsterArray[i].gameObject, monsterPos[j].position, monsterPos[j].rotation, monsters.GetChild(i));
                }
            }
        }
    }

    #endregion

    #region �Լ�
    // �� �ʱ⼳��
    private void InitMap()
    {
        Count = 0;
        isClear = false;

        for (int i = 0; i < monsters.childCount; i++)
        {
            monsterArr.Add(monsters.GetChild(i).gameObject);
        }
    }

    // ī��Ʈ�� ���� �ؽ�Ʈ�� ����
    void SetCountText()
    {
        if (count == 1) StartCoroutine(SpawnMonster());

        sb.Clear();
        sb.Append(count.ToString());
        sb.Append("<size=30>/");
        sb.Append(maxCount);

        countText.text = sb.ToString();

        if (count >= maxCount) ClearMap();
    }

    // �� Ŭ���� �� ȣ��
    public override void ClearMap()
    {
        isClear = true;

        StartCoroutine(DisableProps());
        StartCoroutine(MonsterDie());

        base.ClearMap();
    }

    #region ������Ʈ Ǯ��
    // ��ƼŬ ������Ʈ�� initCount ��ŭ ����
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
    /// ������Ʈ�� ��ȯ
    /// </summary>
    public GameObject GetParticle(Vector3 pos)
    {
        GameObject tempGb;

        if (particlePooling.queue.Count > 0)             // ť�� ���� ������Ʈ�� ���� ���� ��
        {
            tempGb = particlePooling.queue.Dequeue();
            tempGb.SetActive(true);
        }
        else         // ť�� ���̻� ������ ���� ����
        {
            tempGb = GameObject.Instantiate(particlePooling.copyObj, particlePooling.parent.transform);
        }

        tempGb.transform.position = pos;

        return tempGb;
    }


    /// <summary>
    /// �� �� ������Ʈ�� ť�� ������
    /// </summary>
    public void SetParticle(GameObject gb)
    {
        gb.SetActive(false);

        particlePooling.queue.Enqueue(gb);
    }
    #endregion
    #endregion
}
