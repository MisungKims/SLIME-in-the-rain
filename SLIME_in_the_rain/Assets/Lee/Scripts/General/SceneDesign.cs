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
    public int randomNomal;
    public int randomGimmik;
    [Header(" ")]
    public int s_boss;
    public int s_nomal;
    public int s_gimmick;
    public int s_bonus;
    public bool mapClear = false;       //�� Ŭ����� ������
    public bool finalClear = false;     //���� Ŭ����� ������
    public int next;                    //�ؽ�Ʈ ���� �Ѱ���
    public int mapCounting;
    public bool goBoss = false;         //������ �����Ҷ�
    //private
    int bossCount;
    int bossLevel;
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
    #endregion
    public void ResetScene()
    {
        mapCounting = 0;
        bossCount = 0;
        bossLevel = 0;
    }
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
            next = bossLevel + 1;       // 2,3,4
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
            //next = 2;
            next = Random.Range(s_nomal, s_gimmick);

        }
        return next;
    }
}
