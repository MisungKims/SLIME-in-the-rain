/**
 * @brief 젤리 매니저
 * @author 김미성
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
    #region 변수
    #region 싱글톤
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

    // 가중치 랜덤
    [SerializeField]
    private JellyGrade[] jellyGrades = new JellyGrade[4];
    private int total = 0;

    #endregion

    #region 유니티 함수
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

    #region 함수
    void InitWeight()
    {
        for (int i = 0; i < jellyGrades.Length; i++)
        {
            total += jellyGrades[i].weight;
        }
    }

    // 가중치랜덤으로 젤리의 등급을 반환
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
