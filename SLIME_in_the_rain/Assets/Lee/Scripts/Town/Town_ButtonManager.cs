using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;


public class Town_ButtonManager : MonoBehaviour                                 //������ �ν��Ͻ� ���� ui �ߺ��Ǵ°� ó���ϰ� �ϱ�
{
    #region ����
    [Header("---- ESC�� ���� UI ������� �������� ----")]
    public List<GameObject> canvasList;
    [Header("---- ���� ----")]
    public Button settingIcon;
    [Header("---- ���� ----")]
    public List<Button> shopButtonList;
    #endregion


    #region ����Ƽ�Լ�
    private void Start()
    {
        ///onClick ���� ��ũ��Ʈ
        //���� ��ư�� ���� ��ư onClick �߰�
        settingIcon.onClick.AddListener(delegate { SettingButton(); });

        for (int  i = 0;  i < shopButtonList.Count;  i++)
        {
            shopButtonList[i].GetComponent<Button>().onClick.AddListener(delegate { Remain(); });
        }
    }



    //ESC ������ �� ���� ��� ����
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            for (int i = canvasList.Count - 1 , unenable  = 0; i > -1; i--)     //����(0), ���� ������(������) ������ �ȵ�  ,,, �κ��丮(1)�� ���� �ڽĸ� ������ �ؾ���
            {
                //â�� �ϳ��� �������� ESC �������� �� â�� ����
                if (canvasList[i].gameObject.activeSelf)
                {
                    canvasList[i].gameObject.SetActive(false);
                    break;
                }
                //�⺻ ȭ�� �϶� ESC ������ ����â ��
                if (!canvasList[i].gameObject.activeSelf)
                {
                    unenable++;
                }
                if (unenable == canvasList.Count)
                {
                    canvasList[canvasList.Count - 1].gameObject.SetActive(true);
                }
            }
        }
    }
    #endregion


    #region �Լ�
    void SettingButton()
    {
        canvasList[canvasList.Count - 1].gameObject.SetActive(true);
    }
    void Remain()
    {
        GameObject clickObject = EventSystem.current.currentSelectedGameObject;     //��ư ���� ������Ʈ ������
        TextMeshProUGUI remainText = clickObject.transform.GetChild(clickObject.transform.childCount - 2).GetChild(3).GetComponent<TextMeshProUGUI>();        //���� ��ư -> �ڿ��� 2��°�� update -> 4��° �ڽ��� remain
        remainText.text = (int.Parse(remainText.text) - 1).ToString();
        
        //������ 0�� ������ ����
        if(int.Parse(remainText.text) == 0)
        {
            clickObject.GetComponent<Town_Shop_Button>().enabled = false;     //��ư �۵� ��ũ��Ʈ ��
            clickObject.transform.GetChild(clickObject.transform.childCount - 1).gameObject.SetActive(true);        //������ư -> �ڿ��� ù��°�� panel on
            clickObject.transform.GetChild(clickObject.transform.childCount - 1).gameObject.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0.6f); 
            clickObject.GetComponent<Button>().interactable = false;
        }
    }


    //�ӽ�
    public void GoInDun()
    {
        SceneManager.LoadScene("Village");
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    #endregion

}
