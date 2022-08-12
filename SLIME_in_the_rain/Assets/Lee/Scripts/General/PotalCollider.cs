using System.Collections.Generic;
using UnityEngine;

public class PotalCollider : MonoBehaviour
{
    #region ����
    //public 
    [Header("����/�Ϲ�/���/�߰����ʽ�")]
    public List<GameObject> ParticleList;
    public bool onStay = false;

    //singleton
    SceneDesign sceneDesign;
    #endregion
    #region �ݶ��̴� �Լ�
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Slime")
        {
            onStay = true;
            this.transform.GetChild(0).GetComponent<Outline>().enabled = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Slime")
        {
            onStay = false;
            this.transform.GetChild(0).GetComponent<Outline>().enabled = false;
        }
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
        this.transform.GetChild(0).GetComponent<Outline>().enabled = false;

    }
    #endregion

    #region �Լ�


    void Coloring(int next)
    {
        GameObject particle = new GameObject();
        Color color;
        ColorUtility.TryParseHtmlString("#FFFFFF50", out color);

        if (next >= sceneDesign.s_bonus)
        {
            if (next == sceneDesign.s_bonus)            //ȸ����
            {
                ColorUtility.TryParseHtmlString("#CC79FF50", out color);
                particle = Instantiate(ParticleList[3]);
            }
            else if (next == sceneDesign.s_bonus + 1)       //����
            {
                ColorUtility.TryParseHtmlString("#FFE90050", out color);
                particle = Instantiate(ParticleList[4]);
            }
        }

        //else if (next >= sceneDesign.s_gimmick)     //��͹�
        //{
        //    ColorUtility.TryParseHtmlString("#FFA61E50", out color);
        //    particle = Instantiate(ParticleList[2]);
        //}
        else if (next >= sceneDesign.s_nomal)       //�Ϲݸ�
        {
            ColorUtility.TryParseHtmlString("#FFFFFF50", out color);
            particle = Instantiate(ParticleList[1]);
        }
        else
        {
            ColorUtility.TryParseHtmlString("#FF797950", out color);
            particle = Instantiate(ParticleList[0]);
        }
        
        this.GetComponent<Renderer>().material.color = color;
        color.a = 1;
        this.transform.GetChild(0).GetComponent<Outline>().OutlineColor = color;
        particle.transform.parent = this.transform;
        particle.transform.position = this.transform.position + Vector3.down;
        particle.transform.localScale = Vector3.one * 0.1f;
    }
    #endregion 
}
