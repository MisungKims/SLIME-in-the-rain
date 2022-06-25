/**
 * @brief 슬라임 오브젝트
 * @author 김미성
 * @date 22-06-25
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{
    #region 변수
    public Transform weaponPos;

    public Stats stats;

    public Weapon currentWeapon;

    [SerializeField]
    private SkinnedMeshRenderer skinnedMesh;

    // 이동
    enum AnimState { idle, move, attack, damaged, die }     // 애니메이션의 상태
    AnimState animState = AnimState.idle;           

    Vector3 direction;                  // 이동 방향

    // 대시
    bool isDash = false;
    float dashSpeed = 100f;       // 대시 속도
    float defaultTime = 0.1f;
    float dashTime;
    float dashDefaultCoolTime = 1f;
    float dashCoolTime;

    Rigidbody rigid;
    Animator anim;
    #endregion

    #region 유니티 함수
    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        InitStats();
    }

    private void Update()
    {
        Dash();
    }

    void FixedUpdate()
    {
        Move();
        
        anim.SetInteger("animation", (int)animState);
    }

    #endregion

    #region 함수
    /// <summary>
    /// 스탯 초기화
    /// </summary>
    void InitStats()
    {
        stats = new Stats(100, 1f, 1.2f, 1f, 1f, 1f);
    }

    /// <summary>
    /// 슬라임의 움직임
    /// </summary>
    void Move()
    {
        float dirX = Input.GetAxis("Horizontal");
        float dirZ = Input.GetAxis("Vertical");

        if (dirX != 0 || dirZ != 0)
        {
            animState = AnimState.move;

            direction = new Vector3(dirX, 0, dirZ);

            if (direction != Vector3.zero)
            {
                float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

                rigid.rotation = Quaternion.Euler(0, angle, 0);         // 회전
            }
            rigid.position += direction * stats.moveSpeed * Time.deltaTime;   // 이동
        }
        else
        {
            animState = AnimState.idle;
        }

    }

    /// <summary>
    /// 스페이스 바를 누르면 대시
    /// </summary>
    void Dash()
    {
        if (Input.GetKeyDown(KeyCode.Space) && dashCoolTime <= 0)
        {
            isDash = true;
        }

        if (dashTime <= 0)
        {
            defaultTime = 0.1f;

            if (isDash)
            {
                animState = AnimState.idle;

                rigid.position += Vector3.forward * dashSpeed * Time.deltaTime;

                dashTime = defaultTime;
                dashCoolTime = dashDefaultCoolTime;
            }
        }
        else
        {
            dashTime -= Time.deltaTime;
            defaultTime = dashTime;
        }

        if (dashCoolTime > 0f)
        {
            dashCoolTime -= Time.deltaTime;
        }

        isDash = false;
    }

    /// <summary>
    /// 무기 변경
    /// </summary>
    /// <param name="weapon"></param>
    public void ChangeWeapon(Weapon weapon)
    {
        currentWeapon = weapon;

        weapon.transform.parent = weaponPos;
        weapon.transform.localPosition = Vector3.zero;

        ChangeMaterial();
    }

    /// <summary>
    /// 슬라임의 색(머터리얼) 변경
    /// </summary>
    void ChangeMaterial()
    {
        if (currentWeapon)
        {
            skinnedMesh.material = currentWeapon.slimeMat;
        }
    }
    #endregion
}