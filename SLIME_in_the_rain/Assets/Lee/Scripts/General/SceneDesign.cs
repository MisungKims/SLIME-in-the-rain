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
    public int randomPercent;
    //int s_title;
    public int s_village;
    public int s_boss;     //2번부터 보스
    public int s_bonus;    //확정 되면 ㄱㄱ
    public int s_nomal;    //확정 되면 ㄱㄱ
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
        bossLevel = 0;
        
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)     //sceneLoaded 델리게이트
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
        if (nowSceneIndex == 2)       //나중에 지워야할 부분
        {
            mapClear = true;
        }
    }

    public void mapClearCount()
    {
        if (nowSceneIndex == s_village)   //마을이면
        {
            bossCount = 0;  //보스카운터 0으로 초기화
            mapCounting = 0;
        }
        //마을이 아닐때 맵클리어시
        else if (mapClear)   
        {
            if(bossLevel == 3)      //3번째 보스 죽였을때
            {
                FinalClear = true;
            }
            bossCount++;
            mapCounting++;
            Debug.Log(bossCount);
        }
        if(bossCount == 5)  //맵 5개 클리어하면 보스방 열림
        {
            goBoss = true;
            bossCount = 0;
            bossLevel++;
        }
    }


    public int NextScene()
    {
        
        if (goBoss)     //[던전 -> 보스] 로 가야하면 보스맵 얼림
        {
            next = bossLevel + 1;       // 2,3,4
        }
        else if (nowSceneIndex >= s_bonus)   //[던전 -> 보스] 로 가는거 제외, 일반맵 클리어시 다음맵으로
        {
            int ran = Random.Range(0, 100);
            if (ran < randomPercent)      //70%확률로 일반맵
            {
                next = Random.Range(s_nomal, SceneManager.sceneCount-1);
            }
            else
            {
                next = Random.Range(s_bonus, s_nomal - 1);
            }
        }
        else if (nowSceneIndex >= s_boss)  //[보스맵 2, 3 -> 던전]  무조건 일반 던전
        {
            next = 1;
            //next = Random.Range(s_nomal, SceneManager.sceneCount);
        }
        else if (nowSceneIndex == 1)  //[마을 -> 던전] 무조건 일반 던전
        {
            next = 10;
            //next = Random.Range(s_nomal, SceneManager.sceneCount-1);

        }
        else if (nowSceneIndex == 0)   // 타이틀->마을
        {
            next = s_village;
        }
        else
        {
            Debug.LogError("씬 관련 설정 안됨");
            next = 0;
        }
        return next;
    }
}
