/**
 * @brief 룬 버튼
 * @author 김미성
 * @date 22-06-30
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class RuneButton : MonoBehaviour
{
    #region 변수
    public Rune rune;

    // 룬의 이름
    [SerializeField]
    private TextMeshProUGUI runeNameTxt;
    public string RuneName
    {
        set { runeNameTxt.text = value; }
    }

    // 룬의 설명
    [SerializeField]
    private TextMeshProUGUI runeDescTxt;
    public string RuneDesc
    {
        set { runeDescTxt.text = value; }
    }

    // 룬의 이미지
    [SerializeField]
    private Image runeImage;

    private RuneManager runeManager;
    private SelectRuneWindow selectRuneWindow;
    #endregion

    #region 유니티 함수
    private void Awake()
    {
        runeManager = RuneManager.Instance;
        selectRuneWindow = SelectRuneWindow.Instance;
    }
    #endregion

    #region 함수
    // 버튼 UI 설정
    public void SetButton(Rune rune)
    {
        this.rune = rune;

        RuneName = rune.RuneName;
        RuneDesc = rune.RuneDescription;
        runeImage.sprite = rune.RuneSprite;
    }

    // 해당 룬을 선택
    public void Select()
    {
        runeManager.AddMyRune(rune);
        selectRuneWindow.CloseWindow();
    }

    
    #endregion
}
