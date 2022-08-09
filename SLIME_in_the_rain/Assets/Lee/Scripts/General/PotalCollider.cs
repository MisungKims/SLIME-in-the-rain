using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PotalCollider : MonoBehaviour
{
    #region ����
    //public 
    [Header("�ҽ� �ڵ� �� case �������")]
    public List<GameObject> ParticleList;
    public bool onStay = false;

    //singleton
    SceneDesign sceneDesign;
    #endregion
    #region �ݶ��̴� �Լ�
    
    private void OnTriggerEnter(Collider other)
    {
        onStay = true;
    }
    private void OnTriggerExit(Collider other)
    {
        onStay = false;
    }
    #endregion


    #region ����Ƽ �Լ�

    private void Start()
    {
        //singleton
        sceneDesign = SceneDesign.Instance;
        int next = sceneDesign.NextScene();
        Coloring(next);

        this.GetComponent<MeshRenderer>().enabled = false;
        this.GetComponent<CapsuleCollider>().enabled = false;
        this.transform.GetChild(1).gameObject.SetActive(false);

    }
    #endregion

    #region �Լ�


    void Coloring(int next)
    {
        GameObject particle = new GameObject();
        Color color;
        switch (next)
        {
            //�ӽ� ������
            case 1:
                ColorUtility.TryParseHtmlString("#FFFFFF50", out color);
                particle = Instantiate(ParticleList[3]);
                break;
            //����123
            case 2:
            case 3:
            case 4:
                ColorUtility.TryParseHtmlString("#FF797950", out color);
                particle = Instantiate(ParticleList[0]);
                break;
            //���ʽ� ����
            case 5:
                ColorUtility.TryParseHtmlString("#FFE90050", out color);
                particle = Instantiate(ParticleList[1]);
                break;
            //���ʽ� ȸ����
            case 6:
                ColorUtility.TryParseHtmlString("#CC79FF50", out color);
                particle = Instantiate(ParticleList[2]);
                break;
            //�Ϲݸ�
            case 7:
            case 8:
            case 9:
                ColorUtility.TryParseHtmlString("#FFFFFF50", out color);
                particle = Instantiate(ParticleList[3]);
                break;
            default:
                ColorUtility.TryParseHtmlString("#00000050", out color);
                //particle = Instantiate(ParticleList[0]);
                break;
        }
        this.GetComponent<Renderer>().material.color = color;
        particle.transform.parent = this.transform;
        particle.transform.position = this.transform.position + Vector3.down;
        particle.transform.localScale = Vector3.one * 0.1f;
    }
    #endregion 
}
