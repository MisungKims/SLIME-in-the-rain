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
    //int s_title;
    public int s_village;
    public int s_boss;     //2������ ����
    public int s_bonus;    //Ȯ�� �Ǹ� ����
    public int s_nomal;    //Ȯ�� �Ǹ� ����
    public bool mapClear = false;
    public bool FinalClear = false;
    public int nowSceneIndex;
    public int next;
    public int mapCounting;

    //private
    int bossCount;
    int bossLevel;
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
        if (nowSceneIndex == 2)       //���߿� �������� �κ�
        {
            mapClear = true;
        }
    }

    public void mapClearCount()
    {
        if (nowSceneIndex == s_village)   //�����̸�
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
        
        if (goBoss)     //[���� -> ����] �� �����ϸ� ������ ��
        {
            next = bossLevel + 1;       // 2,3,4
        }
        else if (nowSceneIndex >= s_bonus)   //[���� -> ����] �� ���°� ����, �Ϲݸ� Ŭ����� ����������
        {
            int ran = Random.Range(0, 100);
            if (ran < randomPercent)      //70%Ȯ���� �Ϲݸ�
            {
                next = Random.Range(s_nomal, SceneManager.sceneCount-1);
            }
            else
            {
                next = Random.Range(s_bonus, s_nomal - 1);
            }
        }
        else if (nowSceneIndex >= s_boss)  //[������ 2, 3 -> ����]  ������ �Ϲ� ����
        {
            next = 1;
            //next = Random.Range(s_nomal, SceneManager.sceneCount);
        }
        else if (nowSceneIndex == 1)  //[���� -> ����] ������ �Ϲ� ����
        {
            next = 10;
            //next = Random.Range(s_nomal, SceneManager.sceneCount-1);

        }
        else if (nowSceneIndex == 0)   // Ÿ��Ʋ->����
        {
            next = s_village;
        }
        else
        {
            Debug.LogError("�� ���� ���� �ȵ�");
            next = 0;
        }
        return next;
    }
}
