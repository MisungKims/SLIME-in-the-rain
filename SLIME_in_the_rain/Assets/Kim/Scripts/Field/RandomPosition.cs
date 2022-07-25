/**
 * @brief ���� ������ ��ġ�� ��ȯ
 * @author ��̼�
 * @date 22-07-24
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPosition : MonoBehaviour
{
    private Vector3 spawnPosition;
    private float randomX;
    private float randomZ;

    protected float minX;
    protected float minZ;
    protected float maxX;
    protected float maxZ;

    // ���� �߿��� ������ ��ġ�� ������
    protected Vector3 GetRandomPosition()
    {
        randomX = Random.Range(minX, maxX);
        randomZ = Random.Range(minZ, maxZ);

        spawnPosition = new Vector3(randomX, 2.5f, randomZ);
        return spawnPosition;
    }
}
