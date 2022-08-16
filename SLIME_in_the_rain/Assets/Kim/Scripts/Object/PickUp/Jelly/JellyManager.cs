/**
 * @brief ���� �Ŵ���
 * @author ��̼�
 * @date 22-06-27
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class JellyGrade
{
    public Material mat;
    public int weight;
    public int jellyAmount;
}

public class JellyManager : MonoBehaviour
{
    #region ����
    #region �̱���
    private static JellyManager instance = null;
    public static JellyManager Instance
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
    [SerializeField]
    private int jellyCount = 0;
    public int JellyCount { get { return jellyCount; } set { jellyCount = value; } }

    // ����ġ ����
    [SerializeField]
    private JellyGrade[] jellyGrades = new JellyGrade[4];
    private int total = 0;

    #endregion

    #region ����Ƽ �Լ�
    void Awake()
    {
        if (null == instance)
        {
            instance = this;

            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

        InitWeight();
    }
    #endregion

    #region �Լ�
    void InitWeight()
    {
        for (int i = 0; i < jellyGrades.Length; i++)
        {
            total += jellyGrades[i].weight;
        }
    }

    // ����ġ�������� ������ ����� ��ȯ
    public JellyGrade GetRandomJelly()
    {
        int weight = 0;
        int selectNum = 0;

        selectNum = Mathf.RoundToInt(total * Random.Range(0.0f, 1.0f));

        for (int i = 0; i < jellyGrades.Length; i++)
        {
            weight += jellyGrades[i].weight;
            if (selectNum <= weight)
            {
                return jellyGrades[i];
            }
        }

        return null;
    }
    #endregion
}
