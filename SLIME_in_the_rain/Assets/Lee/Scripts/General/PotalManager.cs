using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PotalManager : MonoBehaviour
{
    //public
    [Header("��Ż ������ ��ǥ�� ���� �� ������Ʈ(ȸ���� ���߼���)")]
    public List<GameObject> parentObj;
    [Header(" ")]
    public GameObject potalPrefab;
    [Header("����/�Ϲ�/���/�߰����ʽ�")]
    public List<GameObject> ParticleList;

    //private
    Vector3 vec3;

    //bool
    bool potalMake = false;
    bool doCollision = false;

    //singleton
    SceneDesign sceneDesign;

    // Start is called before the first frame update
    private void Start()
    {
        //singleton
        sceneDesign = SceneDesign.Instance;
        sceneDesign.mapClear = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (sceneDesign.mapClear && !potalMake)
        {
            
            sceneDesign.MapCount();
            sceneDesign.mapClear = false;
            if(!sceneDesign.finalClear)
            {
                PotalCreate();
                potalMake = true;
                doCollision = true;
            }

            
        }
        if(doCollision)
        {
            for (int i = 0; i < parentObj.Count; i++)
            {
                if (parentObj[i].transform.childCount > 0)
                {
                    GameObject ipotal = parentObj[i].transform.GetChild(0).gameObject;
                    if (ipotal.GetComponent<PotalCollider>().onStay)
                    {
                        if (Input.GetKey(KeyCode.G))
                        {
                            SceneManager.LoadScene(ipotal.GetComponent<PotalCollider>().next);
                        }
                    }
                }
                else
                {
                    Debug.Log("�ι�° ��Ż�� ���� ���� �ʾҽ��ϴ�");
                }
                
            }
               
        }
    }


    void PotalCreate()
    {
        for (int i = 0; i < parentObj.Count; i++)
        {
            //�ν��Ͻ���Ż ����
            GameObject ipotal;
            ipotal = Instantiate(potalPrefab, parentObj[i].transform);
            Positioning(ipotal, parentObj[i]);
            ipotal.GetComponent<PotalCollider>().next = sceneDesign.NextScene(SceneManager.GetActiveScene().buildIndex);
            //��Ż�� ���� ������
            Coloring(ipotal, ipotal.GetComponent<PotalCollider>().next);
            if(sceneDesign.s_nomal > ipotal.GetComponent<PotalCollider>().next)
            {
                break;
            }
        }
    }
    void Positioning(GameObject instance,GameObject parent)
    {
        vec3.x = 0;
        vec3.y = 1;
        vec3.z = 0;
        instance.transform.localPosition = vec3;
        instance.transform.rotation = parent.transform.rotation;
        instance.transform.localScale = Vector3.one;
    }
    void Coloring(GameObject gameObject, int next)
    {
        GameObject particle = new GameObject();
        Color color;
        ColorUtility.TryParseHtmlString("#FFFFFF50", out color);
        int BonusIndex = sceneDesign.s_bonus;
        if (next >= BonusIndex)
        {
            if (next == BonusIndex)                //ȸ����
            {
                ColorUtility.TryParseHtmlString("#FA6EF350", out color);
                particle = Instantiate(ParticleList[3]);
            }
            else if (next == ++BonusIndex)       //����
            {
                ColorUtility.TryParseHtmlString("#FFE90050", out color);
                particle = Instantiate(ParticleList[4]);
            }
        }
        else if (next >= sceneDesign.s_gimmick)             //��͹�
        {
            ColorUtility.TryParseHtmlString("#6642FF50", out color);
            particle = Instantiate(ParticleList[2]);
        }
        else if (next >= sceneDesign.s_nomal)               //�Ϲݸ�
        {
            ColorUtility.TryParseHtmlString("#FFFFFF50", out color);
            particle = Instantiate(ParticleList[1]);
        }
        else                                                //������
        {
            ColorUtility.TryParseHtmlString("#FF797950", out color);
            particle = Instantiate(ParticleList[0]);
        }

        gameObject.GetComponent<Renderer>().material.color = color;
        color.a = 1;
        gameObject.transform.GetChild(0).GetComponent<Outline>().OutlineColor = color;
        gameObject.transform.GetChild(0).GetComponent<Outline>().enabled = false;
        particle.transform.parent = gameObject.transform;
        particle.transform.position = gameObject.transform.position + Vector3.down;
        particle.transform.localScale = Vector3.one * 0.1f;
    }
}
