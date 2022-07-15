using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Town_Camera : MonoBehaviour
{
    public Slime slime;
    Vector3 vec3 = new Vector3();


    // Start is called before the first frame update
    void Start()
    {
        vec3.z = Camera.main.transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(slime.gameObject.transform.position);
        vec3.x = slime.transform.position.x;
        vec3.y = slime.transform.position.z + 20;

        Camera.main.transform.position = vec3;
    }
}
