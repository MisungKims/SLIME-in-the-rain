/**
 * @brief �� ��ư
 * @author ��̼�
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
    #region ����
    public Rune rune;

    // ���� �̸�
    [SerializeField]
    private TextMeshProUGUI runeNameTxt;
    public string RuneName
    {
        set { runeNameTxt.text = value; }
    }

    // ���� ����
    [SerializeField]
    private TextMeshProUGUI runeDescTxt;
    public string RuneDesc
    {
        set { runeDescTxt.text = value; }
    }

    // ���� �̹���
    [SerializeField]
    private Image runeImage;

    private RuneManager runeManager;
    private SelectRuneWindow selectRuneWindow;
    #endregion

    #region ����Ƽ �Լ�
    private void Awake()
    {
        runeManager = RuneManager.Instance;
        selectRuneWindow = SelectRuneWindow.Instance;
    }
    #endregion

    #region �Լ�
    // ��ư UI ����
    public void SetButton(Rune rune)
    {
        this.rune = rune;

        RuneName = rune.RuneName;
        RuneDesc = rune.RuneDescription;
        runeImage.sprite = rune.RuneSprite;
    }

    // �ش� ���� ����
    public void Select()
    {
        runeManager.AddMyRune(rune);
        selectRuneWindow.CloseWindow();
    }

    
    #endregion
}
