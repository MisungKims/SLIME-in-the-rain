using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RanWeap : MonoBehaviour
{
    public GameObject[] pos = new GameObject[3];

    

    private void Start()
    {
        for (int i = 0; i < pos.Length; i++)
        {
           ItemDatabase.Instance.weaponDrop(pos[i].transform.position);
        }

    }

}
