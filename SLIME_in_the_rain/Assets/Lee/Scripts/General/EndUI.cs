using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndUI : MonoBehaviour
{
    SceneDesign sceneDesign;
    StatManager statManager;

    public TextMeshProUGUI titleText;
    public TextMeshProUGUI stageText;

    // Start is called before the first frame update
    void Start()
    {
        sceneDesign = SceneDesign.Instance;
        statManager = StatManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (statManager.myStats.HP == 0)
        {
            OnResult();
        }
        if(sceneDesign.FinalClear == true)
        {
            OnResult();
        }
    }
    void OnResult()
    {
        if(sceneDesign.FinalClear == true)
        {
            titleText.text = "CLEAR!!!";
        }
        else
        {
            titleText.text = "DEAD...";
            int stage = 5 + 1;
            stageText.text = (sceneDesign.mapCounting / stage).ToString() +" - "+ (sceneDesign.mapCounting % stage).ToString() ;    //13�� ����������
            //���� ī����
            //�̹��� ȹ�� ����
            //��1 ��2 ��3
            //����ƾ ����

            //������ ���ư���
        }



    }
}
