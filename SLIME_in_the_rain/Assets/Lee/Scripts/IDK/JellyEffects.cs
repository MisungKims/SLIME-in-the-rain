using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JellyEffects : MonoBehaviour
{
    [SerializeField]
    float time;

    IEnumerator wfs()
    {
        yield return new WaitForSeconds(3.0f);
        time = 0;
    }
    private void Start()
    { 
        
    }


    // Update is called once per frame
    void Update()
    {

        if (time < 0.5f)
        {
            GetComponent<Image>().color = new Color(0.9433962f, 0.4138483f, 0.4138483f, 1 - time);
        }
        else
        {
            GetComponent<Image>().color = new Color(0.9433962f, 0.4138483f, 0.4138483f, time);
            if (time > 1f)
            {
                StartCoroutine(wfs());
            }
        }
        time += Time.deltaTime;
    }
}
