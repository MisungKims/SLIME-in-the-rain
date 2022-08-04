/**
 * @brief 미니맵에 표시될 오브젝트
 * @author 김미성
 * @date 22-08-04
 */

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
