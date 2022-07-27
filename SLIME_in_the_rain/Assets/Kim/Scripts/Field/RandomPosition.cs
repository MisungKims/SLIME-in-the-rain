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
    private static Vector3 spawnPosition;
    private static float randomX;
    private static float randomZ;

    // ���� �߿��� ������ ��ġ�� ������
    public static Vector3 GetRandomPosition(float minX, float maxX, float minZ, float maxZ, float y)
    {
        randomX = Random.Range(minX, maxX);
        randomZ = Random.Range(minZ, maxZ);

        spawnPosition = new Vector3(randomX, y, randomZ);

        return spawnPosition;
    }
}
