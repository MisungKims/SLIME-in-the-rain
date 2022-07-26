using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InDun_Manager : MonoBehaviour
{
    ICamera _camera;
    // Start is called before the first frame update
    void Start()
    {
        _camera = ICamera.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        _camera.Focus_Slime();
    }
}
