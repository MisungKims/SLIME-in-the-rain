using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EUIFlag
{
    damageText,
    hpBar
}

public class UIObjectPoolingManager : MonoBehaviour
{
    #region 변수
    #region 싱글톤
    private static UIObjectPoolingManager instance;
    public static UIObjectPoolingManager Instance
    {
        get { return instance; }
    }
    #endregion

    public List<ObjectPool> uiPoolingList = new List<ObjectPool>();

    public FadeOutText stunText;
    public FadeOutText noItemText;
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

        InitCanvas();
    }
    #endregion

    #region 함수

    // 오브젝트를 initCount 만큼 생성
    private void InitCanvas()
    {
        for (int i = 0; i < uiPoolingList.Count; i++)     // poolingList를 탐색해 각 오브젝트를 미리 생성
        {
            for (int j = 0; j < uiPoolingList[i].initCount; j++)
            {
                GameObject tempGb = GameObject.Instantiate(uiPoolingList[i].copyObj, uiPoolingList[i].parent.transform);
                tempGb.name = j.ToString();
                tempGb.gameObject.SetActive(false);
                uiPoolingList[i].queue.Enqueue(tempGb);
            }
        }
    }

    /// <summary>
    /// 오브젝트를 반환
    /// </summary>
    public GameObject Get(EUIFlag flag)
    {
        int index = (int)flag;
        GameObject tempGb;

        if (uiPoolingList[index].queue.Count > 0)             // 큐에 게임 오브젝트가 남아 있을 때
        {
            tempGb = uiPoolingList[index].queue.Dequeue();
            tempGb.SetActive(true);
        }
        else         // 큐에 더이상 없으면 새로 생성
        {
            tempGb = GameObject.Instantiate(uiPoolingList[index].copyObj, uiPoolingList[index].parent.transform);
        }

        return tempGb;
    }
    
    /// <summary>
    /// 오브젝트를 반환
    /// </summary>
    public GameObject Get(EUIFlag flag, Vector3 pos)
    {
        int index = (int)flag;
        GameObject tempGb;

        if (uiPoolingList[index].queue.Count > 0)             // 큐에 게임 오브젝트가 남아 있을 때
        {
            tempGb = uiPoolingList[index].queue.Dequeue();
            tempGb.SetActive(true);
        }
        else         // 큐에 더이상 없으면 새로 생성
        {
            tempGb = GameObject.Instantiate(uiPoolingList[index].copyObj, uiPoolingList[index].parent.transform);
        }

        tempGb.transform.position = pos;

        return tempGb;
    }

    /// <summary>
    /// 다 쓴 오브젝트를 큐에 돌려줌
    /// </summary>
    public void Set(GameObject gb, EUIFlag flag)
    {
        int index = (int)flag;
        gb.SetActive(false);

        uiPoolingList[index].queue.Enqueue(gb);
    }

    // 기절 텍스트 보여줌
    public void ShowStunText()
    {
        stunText.ShowText();
    }

    // 기절 텍스트 보여줌
    public void ShowNoItemText()
    {
        noItemText.ShowText();
    }
    #endregion
}
