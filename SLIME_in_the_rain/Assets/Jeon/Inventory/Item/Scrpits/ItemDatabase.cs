using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    #region ΩÃ±€≈Ê
    private static ItemDatabase instance = null;
    public static ItemDatabase Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }
    #endregion
    void Awake()
    {
        if (null == instance)
        {
            instance = this;

            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    public List<Item> itemDB = new List<Item>();

    public GameObject fieldItemPrefab;
    public Vector3[] pos;

    private void Start()
    {
        for (int i = 0; i < pos.Length; i++)
        {
            GameObject go = Instantiate(fieldItemPrefab, pos[i],Quaternion.identity);
            go.GetComponent<FieldItems>().SetItem(itemDB[Random.Range(0, 2)]);
        }
    }


}
