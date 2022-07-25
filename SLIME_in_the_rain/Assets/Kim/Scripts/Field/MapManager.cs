/**
 * @brief �� �Ŵ���
 * @author ��̼�
 * @date 22-07-24
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [SerializeField]
    private Transform slimeSpawnPos;

    protected virtual void Awake()
    {
        Slime.Instance.transform.position = slimeSpawnPos.position;
    }
}
