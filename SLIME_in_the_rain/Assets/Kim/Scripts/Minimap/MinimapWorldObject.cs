/**
 * @brief �̴ϸʿ� ǥ�õ� ������Ʈ
 * @author ��̼�
 * @date 22-08-04
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapWorldObject : MonoBehaviour
{
    public Sprite Icon;
    public Color IconColor = Color.white;

    [SerializeField]
    private bool isSlime = false;
    [SerializeField]
    private bool isRegisterOnEnable = false;


    private void OnEnable()
    {
        if (Minimap.Instance)
        {
            if (!isSlime && isRegisterOnEnable)
            {
                Minimap.Instance.RegisterMinimapWorldObject(this);
            }
        }
    }

    private void Start()
    {
        if(!isSlime && !isRegisterOnEnable && Minimap.Instance) Minimap.Instance.RegisterMinimapWorldObject(this);
    }

}
