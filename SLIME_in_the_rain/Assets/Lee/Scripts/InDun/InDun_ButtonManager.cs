using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;


public class InDun_ButtonManager : MonoBehaviour                                 //������ �ν��Ͻ� ���� ui �ߺ��Ǵ°� ó���ϰ� �ϱ�
{
    #region ����
    [Header("---- ESC�� ���� UI ������� �������� ----")]
    public List<GameObject> canvasList;
    [Header("---- ���� ----")]
    public Button settingIcon;
    #endregion


    #region ����Ƽ�Լ�
    private void Start()
    {
        ///onClick ���� ��ũ��Ʈ
        //���� ��ư�� ���� ��ư onClick �߰�
        settingIcon.onClick.AddListener(delegate { SettingButton(); });
    }



    //ESC ������ �� ���� ��� ����
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            for (int i = canvasList.Count - 1 , unenable  = 0; i > -1; i--)     //������� ��
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


    //�ӽ�
    //public void GoInDun()
    //{
    //    SceneManager.LoadScene("Village");
    //}
    public void ExitGame()
    {
        Application.Quit();
    }
    #endregion

}
