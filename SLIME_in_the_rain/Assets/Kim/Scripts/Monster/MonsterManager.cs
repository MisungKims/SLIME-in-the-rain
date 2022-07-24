/**
 * @brief 몬스터 매니저
 * @author 김미성
 * @date 22-07-10
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    #region 변수
    #region 싱글톤
    private static MonsterManager instance = null;
    public static MonsterManager Instance
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

    // 몬스터가 돌아다닐 수 있는 범위
    [SerializeField]
    private GameObject range;
    private BoxCollider rangeCollider;
    #endregion

    #region 유니티 함수
    private void Awake()
    {
        if (null == instance)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        rangeCollider = range.GetComponent<BoxCollider>();
    }
    #endregion

    // 몬스터가 돌아다닐 수 있는 범위 중 랜덤한 위치를 반환
    public Vector3 GetRandomPosition()
    {
        Vector3 originPosition = range.transform.position;

        // 콜라이더의 사이즈를 가져오는 bound.size 사용
        float range_X = rangeCollider.bounds.size.x;
        float range_Z = rangeCollider.bounds.size.z;

        range_X = Random.Range((range_X / 2) * -1, range_X / 2);
        range_Z = Random.Range((range_Z / 2) * -1, range_Z / 2);
        Vector3 RandomPostion = new Vector3(range_X, 0f, range_Z);

        Vector3 respawnPosition = originPosition + RandomPostion;

        return respawnPosition;
    }
}
