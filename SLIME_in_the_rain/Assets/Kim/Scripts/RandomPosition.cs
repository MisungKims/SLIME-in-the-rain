/**
 * @brief 맵의 랜덤한 위치를 반환
 * @author 김미성
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

    // 범위 중에서 랜덤한 위치를 가져옴
    protected Vector3 GetRandomPosition()
    {
        randomX = Random.Range(minX, maxX);
        randomZ = Random.Range(minZ, maxZ);

        spawnPosition = new Vector3(randomX, 2.5f, randomZ);
        return spawnPosition;
    }
}
