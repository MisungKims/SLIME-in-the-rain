using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EUIFlag
{
    damageText,
    hpBar
}

public class UIObjectPoolingManager : MonoBehaviour
{
    #region ����
    #region �̱���
    private static UIObjectPoolingManager instance;
    public static UIObjectPoolingManager Instance
    {
        get { return instance; }
    }
    #endregion


    public List<ObjectPool> uiPoolingList = new List<ObjectPool>();

    public Slider hpSlime;
    public FadeOutText stunText;
    public FadeOutText noInventoryText;
    public FadeOutText noWeaponText;
    public UpText inWaterText;
    public Transform canvas;

    private Vector3 originPos = Vector3.up * -279;
    private Vector3 upPos = Vector3.up * -230;
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

        InitCanvas();
    }

    #endregion

    #region �Լ�

    // ������Ʈ�� initCount ��ŭ ����
    private void InitCanvas()
    {
        for (int i = 0; i < uiPoolingList.Count; i++)     // poolingList�� Ž���� �� ������Ʈ�� �̸� ����
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
    /// ������Ʈ�� ��ȯ
    /// </summary>
    public GameObject Get(EUIFlag flag)
    {
        int index = (int)flag;
        GameObject tempGb;

        if (uiPoolingList[index].queue.Count > 0)             // ť�� ���� ������Ʈ�� ���� ���� ��
        {
            tempGb = uiPoolingList[index].queue.Dequeue();
            tempGb.SetActive(true);
        }
        else         // ť�� ���̻� ������ ���� ����
        {
            tempGb = GameObject.Instantiate(uiPoolingList[index].copyObj, uiPoolingList[index].parent.transform);
        }

        return tempGb;
    }
    
    /// <summary>
    /// ������Ʈ�� ��ȯ
    /// </summary>
    public GameObject Get(EUIFlag flag, Vector3 pos)
    {
        int index = (int)flag;
        GameObject tempGb;

        if (uiPoolingList[index].queue.Count > 0)             // ť�� ���� ������Ʈ�� ���� ���� ��
        {
            tempGb = uiPoolingList[index].queue.Dequeue();
            tempGb.SetActive(true);
        }
        else         // ť�� ���̻� ������ ���� ����
        {
            tempGb = GameObject.Instantiate(uiPoolingList[index].copyObj, uiPoolingList[index].parent.transform);
        }

        tempGb.transform.position = pos;

        return tempGb;
    }

    /// <summary>
    /// �� �� ������Ʈ�� ť�� ������
    /// </summary>
    public void Set(GameObject gb, EUIFlag flag)
    {
        int index = (int)flag;
        gb.SetActive(false);

        uiPoolingList[index].queue.Enqueue(gb);
    }

    // ���� �ؽ�Ʈ ������
    public void ShowStunText()
    {
        stunText.ShowText();
    }


    // ���� ���� �ؽ�Ʈ ������
    public void ShowInWaterText()
    {
        inWaterText.ShowText();
        inWaterText.GetComponent<RectTransform>().anchoredPosition = Vector3.up * 72f;
    }

    // �κ��丮�� ���� ���� �ؽ�Ʈ ������
    public void ShowNoInventoryText()
    {
        noInventoryText.ShowText();
        SetTextPosition(noInventoryText.gameObject, noWeaponText.gameObject);
    }

    // ���� ���� �ؽ�Ʈ ������
    public void ShowNoWeaponText()
    {
        noWeaponText.ShowText();
        SetTextPosition(noWeaponText.gameObject, noInventoryText.gameObject);
    }


    // �ؽ�Ʈ�� ��ġ ����
    private void SetTextPosition(GameObject textObject, GameObject otherText)
    {
        // �ٸ� �ؽ�Ʈ�� Ȱ��ȭ �Ǿ����� ��, �� �ؽ�Ʈ�� ��ġ�� ���� textObject�� ��ġ ����
        if (otherText.activeSelf)
        {
            if (otherText.GetComponent<RectTransform>().anchoredPosition.Equals(originPos))     
                textObject.GetComponent<RectTransform>().anchoredPosition = upPos;
            else if (otherText.GetComponent<RectTransform>().anchoredPosition.Equals(upPos))
                textObject.GetComponent<RectTransform>().anchoredPosition = originPos;
        }
        else textObject.GetComponent<RectTransform>().anchoredPosition = originPos;
    }

    // canvas�� �ִ� ��� UI ����
    public void InitUI()
    {
        for (int i = 0; i < 2; i++)
        {
            Transform child = canvas.GetChild(i);
            for (int j = 0; j < child.childCount; j++)
            {
                if (child.GetChild(j).gameObject.activeSelf)
                    Set(child.GetChild(j).gameObject, (EUIFlag)i);
            }
        }

        hpSlime.transform.parent.gameObject.SetActive(false);
        stunText.gameObject.SetActive(false);
        noInventoryText.gameObject.SetActive(false);
        noWeaponText.gameObject.SetActive(false);
        inWaterText.gameObject.SetActive(false);
    }
    #endregion
}
