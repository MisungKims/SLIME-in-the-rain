using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Town_Ui : MonoBehaviour
{
    #region ����
    //���̾� ������� �ֱ�
    public List<GameObject> canvasList = new List<GameObject>();

    #endregion

    #region ����Ƽ�Լ�

    //ESC ������ �� ���� ���̾��� Ui�� ����
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            for (int i = canvasList.Count - 1; i > - 1; i--)
            {
                if (canvasList[i].gameObject.activeSelf)
                {
                    canvasList[i].gameObject.SetActive(false);
                    break;
                }
            }
        }
    }
    #endregion

}
