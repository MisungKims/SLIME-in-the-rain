using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ASDF : MonoBehaviour
{
    // Start is called before the first frame update
    SceneDesign sceneDesign;
    private void Start()
    {
        sceneDesign = SceneDesign.Instance;
    }
    public void Click()
    {
        sceneDesign.mapClear = true;
    }
}
