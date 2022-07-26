using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapWorldObject : MonoBehaviour
{
    public Sprite Icon;
    public Color IconColor = Color.white;


    private void Start()
    {
        Minimap.Instance.RegisterMinimapWorldObject(this);
    }
}
