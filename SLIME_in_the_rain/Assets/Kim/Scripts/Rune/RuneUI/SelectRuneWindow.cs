/**
 * @brief 룬 선택 창 (인던 입장 창)
 * @author 김미성
 * @date 22-06-30
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SelectRuneWindow : MonoBehaviour
{
    #region 변수
    #region 싱글톤
    private static SelectRuneWindow instance = null;
    public static SelectRuneWindow Instance
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

    [SerializeField]
    private RuneButton[] runeButtons = new RuneButton[3];


    // 리롤
    private int rerollMaxCount = 3;
    [SerializeField]
    private TextMeshProUGUI rerollCountTxt;
    private int rerollCount;
    public int RerollCount
    {
        get { return rerollCount; }
        set 
        {
            rerollCount = value;
            rerollCountTxt.text = rerollCount.ToString(); 
        }
    }

    // 캐싱
    private RuneManager runeManager;
    #endregion

    #region 유니티 함수
    private void Awake()
    {
        if (null == instance)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        
        RerollCount = rerollMaxCount;
    }

    private void Start()
    {
        runeManager = RuneManager.Instance;
        JellyManager.Instance.JellyCount = 200; // 나중에 지우기 
        SetButtons();
    }

    private void OnEnable()
    {
        //SetButtons();
    }
    #endregion

    #region 함수
    public void SetButtons()
    {
        for (int i = 0; i < runeButtons.Length; i++)
        {
            runeButtons[i].SetButton(RuneManager.Instance.GetRandomRune());
        }
    }

    public void Reroll()
    {
        if (JellyManager.Instance.JellyCount < 100 || rerollCount <= 0) return;     // 젤리 개수가 100 보다 작거나 리롤 횟수가 0번 남았을 때 return

        SetButtons();

        JellyManager.Instance.JellyCount -= 100;            // 젤리 100개 회수

        RerollCount--;
    }

    // 룬 선택창 나가기 시 랜덤 젤라틴 지급
    public void CloseWindow()
    {
        // TODO : 랜덤 젤라틴 지급

        this.gameObject.SetActive(false);
    }
    #endregion
}
