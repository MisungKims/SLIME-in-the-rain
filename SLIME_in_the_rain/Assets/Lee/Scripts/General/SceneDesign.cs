using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneDesign : MonoBehaviour
{
    #region ����
    #region �̱���
    private static SceneDesign instance = null;
    public static SceneDesign Instance
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
    //public
    //�� ���� �Է� ����
    public int randomNomal;
    public int randomGimmik;
    [Header(" ")]
    public int s_result;
    public int s_boss;
    public int s_nomal;
    public int s_gimmick;
    public int s_bonus;
    //�� ������ ����
    public int next;
    public bool mapClear;       //�� Ŭ����� ������
    public bool goBoss;         //������ �����Ҷ�
    //ResultCanvas�� ���� ����
    public bool finalClear;     //���� Ŭ����� ������
    public int mapCounting;
    public float Timer = 0f;
    public int jellyInit;
    public int bossLevel;

    //private
    int bossCount;


    //singleton
    Slime slime;


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

    }
    IEnumerator StraightClear()
    {
        while(!finalClear)
        {
            yield return null;
        }
        SceneManager.LoadScene(s_result);
    }
    private void Start()
    {
        slime = Slime.Instance;
        StartCoroutine(StraightClear());
    }
    private void Update()
    {
        if(!finalClear)
        {
            Timer += Time.deltaTime;
        }
        
    }
    #endregion

    #region �Լ�

    public void MapCount()
    {
        if(!goBoss)
        {
            bossCount++;
            mapCounting++;
            if (bossCount == 5)  //�� 5�� Ŭ�����ϸ� ������ ������ ����
            {
                goBoss = true;
                bossCount = 0;
                bossLevel++;
            }
        }
        else
        {
            mapCounting++;
            bossCount = 0;
            goBoss = false;
            if(mapCounting == 18)
            {
                finalClear = true;
                SceneManager.LoadScene(s_result);

            }
        }
    }


    public int NextScene(int now)
    {
        next = -1;
        do
        {
            if (goBoss)     //[���� -> ����] �� �����ϸ� ������ ��
            {
                next = bossLevel + s_boss - 1;       // 3,4,5
            }
            else if (now >= s_nomal)
            {
                int ran = Random.Range(0, 100);
                if (ran < randomNomal)      //70%Ȯ���� �Ϲݸ�
                {
                    next = Random.Range(s_nomal, s_gimmick);
                }
                else if (ran < randomNomal + randomGimmik)
                {
                    next = Random.Range(s_gimmick, s_bonus);
                }
                else
                {
                    next = Random.Range(s_bonus, SceneManager.sceneCountInBuildSettings);
                }
            }
            else if (now >= s_boss)   //[������ 2, 3 -> ����]  ������ �Ϲ� ����
            {
                next = Random.Range(s_nomal, s_gimmick);
            }
            else if (now == 1)  //[���� -> ����] ������ �Ϲ� ����
            {
                next = 11;
                //next = Random.Range(s_nomal, s_gimmick);

            }
        } while (next == now);
        return next;
    }

    public void SceneInit()     //VillageManager ������ ������
    {
        next = -1;
        mapClear = false;  
        goBoss = false; 
        finalClear = false;
        
        bossCount = 0;

        bossLevel = 0;
        Timer = 0f;
        mapCounting = 0;
    }
    #endregion
}
