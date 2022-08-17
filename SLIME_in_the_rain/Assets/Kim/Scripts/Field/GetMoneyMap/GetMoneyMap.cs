/**
 * @brief �ʿ� �������� ��ȭ�� ����
 * @author ��̼�
 * @date 22-07-24
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GetMoneyMap : MapManager
{
    #region ����
    #region �̱���
    private static GetMoneyMap instance = null;
    public static GetMoneyMap Instance
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

    private int jellyIndex = (int)EObjectFlag.jelly;
    private int gelatinIndex = (int)EObjectFlag.gelatin;
    private int randObj;

    [Header("-------------- Get Money Map")]
    [SerializeField]
    private int spawnRange;
    private Vector3 randPos;

    [SerializeField]
    private int objCount = 70;       // ������Ʈ�� ����

    // Ÿ�� ī��Ʈ
    [SerializeField]
    private TextMeshProUGUI secondText;
    private int second;
    public int Second 
    { 
        get { return second; }
        set 
        { 
            second = value;
            secondText.text = second.ToString();
        } 
    }

   public  float sumSpeed = 0;

    [Header("-------------- Object Pool")]
    [SerializeField]
    private ObjectPool particlePooling;     // ��ƼŬ ������Ʈ Ǯ��
    [SerializeField]
    private ObjectPool speedUpPooling;     // ��ƼŬ ������Ʈ Ǯ��
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
    }

    #region �ڷ�ƾ
    private IEnumerator Start()
    {
        yield return null;

        objectPoolingManager = ObjectPoolingManager.Instance;

        StartCoroutine(TimeCount());
        StartCoroutine(SpwanMoney());
        StartCoroutine(SpawnSpeedUp());
    }

    IEnumerator SpwanMoney()
    {
        while (second > 0)
        {
            int rand = Random.Range(4, 10);
            for (int i = 0; i < rand; i++)
            {
                randObj = Random.Range(jellyIndex, gelatinIndex + 1);       // �������� ����, ����ƾ�� ���Ͽ� �ʿ� ������

                RandomPosition.GetRandomNavPoint(Vector3.zero, 10, out randPos);
                randPos.y = 3;
                objectPoolingManager.Get((EObjectFlag)randObj, randPos);
            }

            yield return new WaitForSeconds(Random.Range(0.1f, 1f));
        }
    }

    IEnumerator SpawnSpeedUp()
    {
        while (second > 0)
        {
            yield return new WaitForSeconds(Random.Range(5f, 10f));

            RandomPosition.GetRandomNavPoint(Vector3.zero, 10, out randPos);
            randPos.y = 2.3f;
            GetSpeedUp(randPos);
        }
    }

    // �ð��� ���� �ڷ�ƾ
    IEnumerator TimeCount()
    {
        Second = 30;

        while (second > 0)
        {
            yield return new WaitForSeconds(1f);

            Second--;
        }

        ClearMap();
    }

    public override void ClearMap()
    {
        StartCoroutine(objectPoolingManager.SetMoney());
        StatManager.Instance.AddMoveSpeed(sumSpeed * -1);

        base.ClearMap();
    }
    #endregion

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

        for (int i = 0; i < speedUpPooling.initCount; i++)
        {
            GameObject tempGb = GameObject.Instantiate(speedUpPooling.copyObj, speedUpPooling.parent.transform);
            tempGb.name = i.ToString();
            tempGb.gameObject.SetActive(false);
            speedUpPooling.queue.Enqueue(tempGb);
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

    /// <summary>
    /// ������Ʈ�� ��ȯ
    /// </summary>
    public GameObject GetSpeedUp(Vector3 pos)
    {
        GameObject tempGb;

        if (speedUpPooling.queue.Count > 0)             // ť�� ���� ������Ʈ�� ���� ���� ��
        {
            tempGb = speedUpPooling.queue.Dequeue();
            tempGb.SetActive(true);
        }
        else         // ť�� ���̻� ������ ���� ����
        {
            tempGb = GameObject.Instantiate(speedUpPooling.copyObj, speedUpPooling.parent.transform);
        }

        tempGb.transform.position = pos;

        return tempGb;
    }

    /// <summary>
    /// �� �� ������Ʈ�� ť�� ������
    /// </summary>
    public void SetSpeedUp(GameObject gb)
    {
        gb.SetActive(false);

        speedUpPooling.queue.Enqueue(gb);
    }
    #endregion
}
