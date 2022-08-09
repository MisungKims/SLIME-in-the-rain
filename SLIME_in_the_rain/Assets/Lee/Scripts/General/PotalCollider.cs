using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PotalCollider : MonoBehaviour
{
    #region 변수
    //public 
    [Header("소스 코드 내 case 순서대로")]
    public List<GameObject> ParticleList;
    public bool onStay = false;

    //singleton
    SceneDesign sceneDesign;
    #endregion
    #region 콜라이더 함수
    
    private void OnTriggerEnter(Collider other)
    {
        onStay = true;
    }
    private void OnTriggerExit(Collider other)
    {
        onStay = false;
    }
    #endregion


    #region 유니티 함수

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

    #region 함수


    void Coloring(int next)
    {
        GameObject particle = new GameObject();
        Color color;
        switch (next)
        {
            //임시 마을로
            case 1:
                ColorUtility.TryParseHtmlString("#FFFFFF50", out color);
                particle = Instantiate(ParticleList[3]);
                break;
            //보스123
            case 2:
            case 3:
            case 4:
                ColorUtility.TryParseHtmlString("#FF797950", out color);
                particle = Instantiate(ParticleList[0]);
                break;
            //보너스 골드방
            case 5:
                ColorUtility.TryParseHtmlString("#FFE90050", out color);
                particle = Instantiate(ParticleList[1]);
                break;
            //보너스 회복방
            case 6:
                ColorUtility.TryParseHtmlString("#CC79FF50", out color);
                particle = Instantiate(ParticleList[2]);
                break;
            //일반맵
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
