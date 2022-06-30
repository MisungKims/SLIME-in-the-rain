/**
 * @brief �� ���� â (�δ� ���� â)
 * @author ��̼�
 * @date 22-06-30
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SelectRuneWindow : MonoBehaviour
{
    #region ����
    #region �̱���
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


    // ����
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

    // ĳ��
    private RuneManager runeManager;
    #endregion

    #region ����Ƽ �Լ�
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
        JellyManager.Instance.JellyCount = 200; // ���߿� ����� 
        SetButtons();
    }

    private void OnEnable()
    {
        //SetButtons();
    }
    #endregion

    #region �Լ�
    public void SetButtons()
    {
        for (int i = 0; i < runeButtons.Length; i++)
        {
            runeButtons[i].SetButton(RuneManager.Instance.GetRandomRune());
        }
    }

    public void Reroll()
    {
        if (JellyManager.Instance.JellyCount < 100 || rerollCount <= 0) return;     // ���� ������ 100 ���� �۰ų� ���� Ƚ���� 0�� ������ �� return

        SetButtons();

        JellyManager.Instance.JellyCount -= 100;            // ���� 100�� ȸ��

        RerollCount--;
    }

    // �� ����â ������ �� ���� ����ƾ ����
    public void CloseWindow()
    {
        // TODO : ���� ����ƾ ����

        this.gameObject.SetActive(false);
    }
    #endregion
}
