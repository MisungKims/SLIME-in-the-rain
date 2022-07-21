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
    private GameObject panel;

    [SerializeField]
    private Canvas GetGelatinCanvas;     // ����ƾ ȹ�� ĵ����
    [SerializeField]
    private GetGelatinWindow GetGelatinWindow;     // ����ƾ ȹ�� ĵ����

    [SerializeField]
    private RuneButton[] runeButtons = new RuneButton[3];           // �� ���� ��ư �迭


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

    [SerializeField]
    private FadeOutText warningText;     // ���� ���� ���â

    // ĳ��
    private RuneManager runeManager;
    private JellyManager jellyManager;
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
    }
    private void OnEnable()
    {
        Init();
    }

    private void Start()
    {
        runeManager = RuneManager.Instance;
        jellyManager = JellyManager.Instance;

        jellyManager.JellyCount = 200; // ���߿� ����� 
    }
    #endregion

    #region �Լ�
    // �� ���� â �ʱ�ȭ
    void Init()
    {
        SetButtons();
        RerollCount = rerollMaxCount;
        GetGelatinCanvas.enabled = false;
    }

    // ��ư �ʱ�ȭ
    public void SetButtons()
    {
        if (!runeManager) runeManager = RuneManager.Instance;

        for (int i = 0; i < runeButtons.Length; i++)
        {
            runeButtons[i].SetUI(runeManager.GetRandomRune());
        }
    }

    // �� ��ư ���ΰ�ħ
    public void Reroll()
    {
        // ���� ������ 100 ���� �۰ų� ���� Ƚ���� 0�� ������ ��
        if (jellyManager.JellyCount < 100 || rerollCount <= 0)
        {
            // ���� ���� ���â�� ���
            if (warningText.gameObject.activeSelf) warningText.isAgain = true;
            else warningText.gameObject.SetActive(true);

            return;     
        }

        SetButtons();

        jellyManager.JellyCount -= 100;            // ���� 100�� ȸ��

        RerollCount--;
    }

    // ���� ����ƾ ����â ������
    public void GetGelatin()
    {
        GetGelatinCanvas.enabled = true;
        GetGelatinWindow.SetUI();
    }

    // �� ����â�� �ݱ�
    public void CloseWindow()
    {
        panel.SetActive(false);
    }
    #endregion
}
