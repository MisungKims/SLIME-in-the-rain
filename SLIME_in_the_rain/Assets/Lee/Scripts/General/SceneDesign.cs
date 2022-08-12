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
    public int randomPercent;

    [Header("�������� ī�װ� �� ���� �ε��� �Է�")]
    public int s_boss;     
    public int s_nomal;    
    public int s_gimmick;   
    public int s_bonus;    
    public bool mapClear = false;
    public bool FinalClear = false;
    public int nowSceneIndex;
    public int next;
    public int mapCounting;

    [Header(" ")]
    //private
    [SerializeField]
    int bossCount;
    [SerializeField]
    int bossLevel;
    [SerializeField]
    bool goBoss = false;
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
        bossLevel = 0;
        
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)     //sceneLoaded ��������Ʈ
    {
        NowScene();
        Debug.Log("OnSceneLoaded: " + scene.name);
        Debug.Log(mode);
    }
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    #endregion

    void NowScene()
    {
        nowSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (nowSceneIndex == 1)
        {
            mapClear = true;
        }
    }

    public void mapClearCount()
    {
        if (nowSceneIndex == 1)   //�����̸�
        {
            bossCount = 0;  //����ī���� 0���� �ʱ�ȭ
            mapCounting = 0;
        }
        //������ �ƴҶ� ��Ŭ�����
        else if (mapClear)   
        {
            if(bossLevel == 3)      //3��° ���� �׿�����
            {
                FinalClear = true;
            }
            bossCount++;
            mapCounting++;
            Debug.Log(bossCount);
        }
        if(bossCount == 5)  //�� 5�� Ŭ�����ϸ� ������ ����
        {
            goBoss = true;
            bossCount = 0;
            bossLevel++;
        }
    }


    public int NextScene()
    {
        
        if (goBoss)                             //[���� -> ����] �� �����ϸ� ������ ��
        {
            next = bossLevel + 1;               // 2,3,4
        }
        else if (nowSceneIndex >= s_nomal)      //��� ���� Ŭ����� ����������
        {
            int ran = Random.Range(0, 100);
            if (ran < randomPercent)            //70%Ȯ���� �Ϲݸ�
            {
                next = Random.Range(s_nomal, s_gimmick - 1);
            }
            else if(ran < 90)
            {
                next = Random.Range(s_gimmick, s_bonus - 1);
            }
            else
            {
                next = Random.Range(s_bonus, SceneManager.sceneCountInBuildSettings - 1);
            }
        }
        else if (nowSceneIndex >= s_boss)  //[������ 2, 3 -> ����]  ������ �Ϲ� ����
        {
            next = Random.Range(s_nomal, SceneManager.sceneCountInBuildSettings - 1);
        }
        else if (nowSceneIndex == 1)  //[���� -> ����] ������ �Ϲ� ����
        {
            next = Random.Range(s_nomal, SceneManager.sceneCountInBuildSettings - 1);

        }
        else if (nowSceneIndex == 0)   // Ÿ��Ʋ->����
        {
            next = 1;
        }
        else
        {
            Debug.LogError("�� ���� ���� �ȵ�");
            next = 0;
        }
        return next;
    }
}
