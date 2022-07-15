using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    #region �̱���
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
    public List<Item> AllitemDB = new List<Item>();

    public Sprite[] imageDB;
    public List<ItemEffect> itemEffect = new List<ItemEffect>();
    public GameObject fieldItemPrefab;
    public Vector3[] pos;

    public TextAsset ItemDbT;
  

    private void Start()
    {
        ///itemdb.txt > ����ȭ
        string[] line = ItemDbT.text.Substring(0, ItemDbT.text.Length - 1).Split('\n');
        for (int i = 0; i < line.Length; i++)
        {
            string[] row = line[i].Split('\t');
            ItemType r0;
            if (row[0] == "gelatin")
            {
                r0 = ItemType.gelatin;
            }
            else
            {
                r0 = ItemType.weapon;
            }
            AllitemDB.Add(new Item(r0, row[1], row[2]));
        }
        
        for (int i = 0; i < AllitemDB.Count; i++)//itemdb �̹��� ����ȭ
        {
            if (GameObject.Find(AllitemDB[i].itemName) != null)
            {
                AllitemDB[i].itemGB = transform.Find(AllitemDB[i].itemName).gameObject;

                for (int j = 0; j < imageDB.Length; j++)
                {
                    if (AllitemDB[i].itemName == imageDB[j].name)
                    { AllitemDB[i].itemIcon = imageDB[j]; }
                }
            }
        }
        for (int i = 0; i < AllitemDB.Count; i++)
        {
            if (AllitemDB[i].itemType == ItemType.weapon)
            {
                for (int j = 0; j < itemEffect.Count; j++)
                {
                    if (AllitemDB[i].itemName == itemEffect[j].name)
                    {
                        itemEffect[j].weapon = ObjectPoolingManager.Instance.Get(itemEffect[j].eWeaponType).GetComponent<Weapon>();
                        AllitemDB[i].efts.Add(itemEffect[j]);
                    }
                }
            }
        }

        for (int i = 0; i < pos.Length; i++)//������ ���� -> ���� ��, ������Ʈ ������� ���������� ��µǰ� , ������Ʈ Ǯ�� �̶� ���� ����ϸ� �ɵ�
        {
            GameObject go = Instantiate(fieldItemPrefab, pos[i],Quaternion.identity);
            go.GetComponent<FieldItems>().SetItem(AllitemDB[Random.Range(0, AllitemDB.Count)]); 
        }
    }


}
