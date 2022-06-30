/**
 * @brief 발사체 오브젝트
 * @author 김미성
 * @date 22-06-27
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EProjectileFlag     // 투사체 Flag
{
    arrow,
    ice,
    fire,
    iceSkill,
    fireSkill,
    dagger,
    sword
}

public class Projectile : MonoBehaviour
{
    #region 변수
    [SerializeField]
    protected float speed;

    private float damageAmount;
    public float DamageAmount { set { damageAmount = value; } }

    [SerializeField]
    protected EProjectileFlag flag;

    // 캐싱
    WaitForSeconds waitFor1s = new WaitForSeconds(1f);
    WaitForSeconds waitFor2s = new WaitForSeconds(2f);
    #endregion

    #region 유니티 함수
    private void OnEnable()
    {
        StartCoroutine(Remove());
    }

    private void Update()
    {
        Move();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DamagedObject"))
        {
            DoDamage(other);
        }
    }
    #endregion

    #region 코루틴

    // 2초 후에 없어짐
    IEnumerator Remove()
    {
        yield return waitFor2s;

        ObjectPoolingManager.Instance.Set(this.gameObject, flag);
    }

    #endregion

    #region 함수
    protected virtual void Move()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }

    // 데미지를 입힘
    protected virtual void DoDamage(Collider other)
    {
        ObjectPoolingManager.Instance.Set(this.gameObject, flag);

        IDamage damagedObject = other.transform.GetComponent<IDamage>();
        if (damagedObject != null)
        {
            damagedObject.Damaged();
        }
    }
    #endregion
}
