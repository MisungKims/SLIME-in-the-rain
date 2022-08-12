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
    public bool mapClear = false;       //�� Ŭ����� 
    public bool FinalClear = false;     //���� Ŭ�����
    public int next;        //�ؽ�Ʈ ���� �Ѱ���
   

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
    }
    #endregion
    public void resetScene()
    {
        mapCounting = 0;
        bossCount = 0;
        bossLevel = 0;
    }
    public void MapCount()
    {
        if (bossLevel == 3)      //3��° ���� �׿�����
        {
            FinalClear = true;
        }
        bossCount++;
        mapCounting++;
        if (bossCount == 5)  //�� 5�� Ŭ�����ϸ� ������ ����
        {
            goBoss = true;
            bossCount = 0;
            bossLevel++;
        }
    }


    public int NextScene(int now)
    {

        if (goBoss)     //[���� -> ����] �� �����ϸ� ������ ��
        {
            next = bossLevel + 1;       // 2,3,4
            goBoss = false;
        }
        else if (now >= s_nomal)
        {
            int ran = Random.Range(0, 100);
            if (ran < randomNomal)      //70%Ȯ���� �Ϲݸ�
            {
                next = Random.Range(s_nomal, s_gimmick - 1);
            }
            else if (ran < randomNomal + randomGimmik)
            {
                next = Random.Range(s_gimmick, s_bonus - 1);
            }
            else
            {
                next = Random.Range(s_bonus, SceneManager.sceneCountInBuildSettings - 1);
            }
        }
        else if (now >= s_boss)   //[������ 2, 3 -> ����]  ������ �Ϲ� ����
        {
            next = Random.Range(s_nomal, s_gimmick - 1);
        }
        else if (now == 1)  //[���� -> ����] ������ �Ϲ� ����
        {
            //next = 2;
            next = Random.Range(s_nomal, s_gimmick - 1);

        }
        return next;
    }
}
