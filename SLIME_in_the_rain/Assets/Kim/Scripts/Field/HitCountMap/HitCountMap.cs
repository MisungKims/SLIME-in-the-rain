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

    [Header("-------------- Object Pool")]
    [SerializeField]
    private ObjectPool particlePooling;     // ��ƼŬ ������Ʈ Ǯ��
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

    IEnumerator DisableProps()
    {
        for (int i = 0; i < props.Length; i++)
        {
            props[i].gameObject.SetActive(false);

            yield return new WaitForSeconds(0.4f);
        }
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

    // Prop�� ��ġ�� ����
    private void InitMap()
    {
        Count = 0;
        isClear = false;

        props = ShuffleArray(props);    // �迭�� ����

        for (int i = 0; i < props.Length; i++)
        {
            props[i].transform.position = propPos[i].position;
        }
    }

    // �迭�� �������� ����
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

    // �� Ŭ���� �� ȣ��
    public override void ClearMap()
    {
        isClear = true;

        StartCoroutine(DisableProps());

        base.ClearMap();
    }

   
}
