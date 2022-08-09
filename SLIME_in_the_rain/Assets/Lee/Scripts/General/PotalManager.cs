using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PotalManager : MonoBehaviour
{
    //public
    public GameObject potalPrefab;
    [Header("포탈 생성할 좌표를 가진 빈 오브젝트(회전각 맞추세요)")]
    public List<GameObject> parentObj;

    //private
    int now;
    Vector3 vec3;

    //bool
    bool doCollision = false;

    //singleton
    SceneDesign sceneDesign;

    // Start is called before the first frame update
    private void Start()
    {
        //singleton
        sceneDesign = SceneDesign.Instance;

        now = sceneDesign.nowSceneIndex;
        PotalCreate();
    }

    // Update is called once per frame
    void Update()
    {
        if (sceneDesign.mapClear)
        {
            PotalGateOpen();
            sceneDesign.mapClear = false;
            doCollision = true;
        }
        if(doCollision)
        {
            for (int i = 0; i < parentObj.Count; i++)
            {
                GameObject ipotal = parentObj[i].transform.GetChild(0).gameObject;
                if (ipotal.GetComponent<PotalCollider>().onStay)
                {
                    if (Input.GetKey(KeyCode.G))
                    {
                        SceneManager.LoadScene(sceneDesign.next);
                        
                    }
                }
            }
        }
    }

    void PotalCreate()
    {
        for (int i = 0; i < parentObj.Count; i++)
        {
            GameObject ipotal;
            ipotal = Instantiate(potalPrefab, parentObj[i].transform);
            vec3.x = 0;
            vec3.y = 1;
            vec3.z = 0;
            ipotal.transform.localPosition = vec3;
            ipotal.transform.rotation = parentObj[i].transform.rotation;
            ipotal.transform.localScale = Vector3.one;
        }

    }
    void PotalGateOpen()
    {
        for (int i = 0; i < parentObj.Count; i++)
        {
            GameObject ipotal = parentObj[i].transform.GetChild(0).gameObject;
            ipotal.GetComponent<MeshRenderer>().enabled = true;
            ipotal.GetComponent<CapsuleCollider>().enabled = true;
            ipotal.transform.GetChild(1).gameObject.SetActive(true);
        }
    }
}
