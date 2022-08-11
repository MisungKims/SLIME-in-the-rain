using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class ButtonManager : MonoBehaviour                                 //������ �ν��Ͻ� ���� ui �ߺ��Ǵ°� ó���ϰ� �ϱ�
{
    #region ����
    [Header("---- ESC�� �� UI (SetActive) ----")]
    public List<GameObject> canvasList;
    [Header("---- ���� ----")]
    public Button settingIcon;

    bool isSettingCanvasOn = false;
    #endregion


    #region ����Ƽ�Լ�

    private void Start()
    {
        ///onClick
        //���� ��ư
        settingIcon.onClick.AddListener(delegate { SettingButton(); });
        for (int i = 0; i < canvasList.Count; i++)
        {
            canvasList[i].gameObject.SetActive(false);
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
                if (canvasList[i].activeSelf)
                {
                    canvasList[i].SetActive(false);
                    
                    break;
                }
                //�⺻ ȭ�� �϶� ESC ������ ����â ��
                if (!canvasList[i].activeSelf)
                {
                    unenable++;
                }
                if (unenable == canvasList.Count)
                {
                    canvasList[canvasList.Count - 1].SetActive(true);
                }
            }
        }
    }
    #endregion


    #region �Լ�
    void SettingButton()
    {
        //PlayerPrefs.GetString();
        canvasList[canvasList.Count - 1].SetActive(true);
        
    }

    #endregion

}
