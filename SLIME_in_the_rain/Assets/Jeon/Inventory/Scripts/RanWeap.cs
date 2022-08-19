using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RanWeap : MonoBehaviour
{
    public GameObject[] pos = new GameObject[3];
    [SerializeField]
    private GameObject[] debugPos = new GameObject[5];

    private void Start()
    {
        for (int i = 0; i < pos.Length; i++)
        {
            ItemDatabase.Instance.weaponDrop(pos[i].transform.position + (Vector3.up * 0.5f));
        }

        for (int i = 0; i < debugPos.Length; i++)
        {
            GameObject go = ObjectPoolingManager.Instance.GetFieldItem(ItemDatabase.Instance.AllitemDB[i + 15], debugPos[i].transform.position + Vector3.up * 0.3f);
            go.layer = go.transform.GetChild(0).gameObject.layer;
        }

    }

}
