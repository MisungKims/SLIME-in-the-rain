using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneDesign : MonoBehaviour
{
    #region 변수
    #region 싱글톤
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
    public bool mapClear = false;       //맵 클리어시 
    public bool FinalClear = false;     //게임 클리어시
    public int next;        //넥스트 값을 넘겨줌
   

    public int mapCounting;
    //private
    int bossCount;
    int bossLevel;
    bool goBoss = false;
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
        if (bossLevel == 3)      //3번째 보스 죽였을때
        {
            FinalClear = true;
        }
        bossCount++;
        mapCounting++;
        if (bossCount == 5)  //맵 5개 클리어하면 보스방 열림
        {
            goBoss = true;
            bossCount = 0;
            bossLevel++;
        }
    }


    public int NextScene(int now)
    {

        if (goBoss)     //[던전 -> 보스] 로 가야하면 보스맵 얼림
        {
            next = bossLevel + 1;       // 2,3,4
            goBoss = false;
        }
        else if (now >= s_nomal)
        {
            int ran = Random.Range(0, 100);
            if (ran < randomNomal)      //70%확률로 일반맵
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
        else if (now >= s_boss)   //[보스맵 2, 3 -> 던전]  무조건 일반 던전
        {
            next = Random.Range(s_nomal, s_gimmick - 1);
        }
        else if (now == 1)  //[마을 -> 던전] 무조건 일반 던전
        {
            //next = 2;
            next = Random.Range(s_nomal, s_gimmick - 1);

        }
        return next;
    }
}
