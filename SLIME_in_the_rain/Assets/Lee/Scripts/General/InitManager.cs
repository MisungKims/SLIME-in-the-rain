using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitManager : MonoBehaviour
{
    //void OnEnable()
    //{
    //    // 씬 매니저의 sceneLoaded에 체인을 건다.
    //    SceneManager.sceneLoaded += OnSceneLoaded;
    //}

    //// 체인을 걸어서 이 함수는 매 씬마다 호출된다.
    //void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    //{
    //    if (scene.buildIndex == 0)
    //    {
    //        Init_Title();
    //    }
    //    if(scene.buildIndex == 1)
    //    {
    //        Init_Village();
    //    }

    //    Debug.Log("OnSceneLoaded: " + scene.name);
    //    //Debug.Log(mode);
    //}

    //void OnDisable()
    //{
    //    SceneManager.sceneLoaded -= OnSceneLoaded;
    //}
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


        ////슬라임
        //slime.InitSlime();
        //slime.transform.localScale = Vector3.one;


        ////스탯  
        //statManager.InitStats();

        ////룬
        //Transform runeSlot = runeManager.gameObject.transform.GetChild(0);
        //Vector3 pos;
        //pos.x = 22.5f; pos.y = 56.5f; pos.z = 0;
        //runeSlot.position = pos;
        //runeSlot.localScale = Vector3.one * 0.7f;
        //if (!runeManager.transform.GetChild(0).gameObject.activeSelf)
        //{
        //    runeManager.transform.GetChild(0).gameObject.SetActive(true);
        //}
        //runeManager.InitRune();

        ////젤리
        //if (PlayerPrefs.HasKey("jellyCount"))
        //{
        //    jellyManager.JellyCount = PlayerPrefs.GetInt("jellyCount");
        //}
        //else
        //{
        //    jellyManager.JellyCount = 0;
        //}

        ////인벤토리
        //inventory.ResetInven();

        ////씬디자인
        //sceneDesign.finalClear = false;

    }
