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
    public int s_boss;
    public int s_nomal;
    public int s_gimmick;
    public int s_bonus;
    //�� ������ ����
    public bool doTutorial = false;      //���߿� false�� �ٲٱ�
    public int next;
    public bool mapClear = false;       //�� Ŭ����� ������
    public bool goBoss = false;         //������ �����Ҷ�
    //ResultCanvas�� ���� ����
    public bool finalClear = false;     //���� Ŭ����� ������
    public bool isDead = false;
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
    private void Start()
    {
        slime = Slime.Instance;
    }
    private void Update()
    {
        if(!finalClear)
        {
            Timer += Time.deltaTime;
        }
    }
    #endregion
    static IEnumerator OnResult()
    {
        while (Slime.Instance.statManager.myStats.HP > 0 && !SceneDesign.instance.finalClear)
        {
            Debug.Log("��������� ���� ���� �ڷ�ƾ �۵���");
            yield return null;
        }
        SceneManager.LoadScene(3);
    }


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
            }
        }
    }


    public int NextScene(int now)
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
            next = Random.Range(s_nomal, s_gimmick);

        }
        return next;
    }


    //Village ������ ������
    public void ResetScene()
    {
        mapCounting = 0;
        bossCount = 0;
        bossLevel = 0;
        Timer = 0;
        StartCoroutine(OnResult());
    }
    #endregion
}
