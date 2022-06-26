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
    #region 싱글톤
    private static Slime instance = null;
    public static Slime Instance
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

    public Transform weaponPos;

    public Stats stats;

    public Weapon currentWeapon;

    [SerializeField]
    private SkinnedMeshRenderer skinnedMesh;            // 슬라임의 Material

    Vector3 mousePos;

    Vector3 targetPos;

    bool isAttacking;   // 공격 중인지?

    //////// 이동
    enum AnimState { idle, move, attack, damaged, die }     // 애니메이션의 상태
    AnimState animState = AnimState.idle;           

    Vector3 direction;                  // 이동 방향

    //////// 대시
    //bool isDash = false;
    //float dashSpeed = 100f;       // 대시 속도
    //float defaultTime = 0.1f;
    //float dashTime;
    //float dashDefaultCoolTime = 1f;
    //float dashCoolTime;

    //////// 무기
    [SerializeField]
    private LayerMask weaponLayer;

    private float detectRadius = 1.1f;

    Collider[] colliders;

    [HideInInspector]
    public Rigidbody rigid;
    Animator anim;
    #endregion

    #region 유니티 함수
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

        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        InitStats();
    }

    private void Start()
    {
        StartCoroutine(AutoAttack());
        StartCoroutine(Skill());
        StartCoroutine(Dash());
    }

    private void Update()
    {
        DetectWeapon();
    }

    void FixedUpdate()
    {
        Move();
        
        anim.SetInteger("animation", (int)animState);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectRadius);
    }
    #endregion

    #region 코루틴
    /// <summary>
    /// 무기를 들고 있을 때 좌클릭하면 평타
    /// </summary>
    IEnumerator AutoAttack()
    {
        while (true)
        {
            if (currentWeapon && Input.GetMouseButtonDown(0))
            {
                isAttacking = true;

                LookAtMousePos();

                yield return new WaitForSeconds(0.01f);

                currentWeapon.SendMessage("AutoAttack", targetPos, SendMessageOptions.DontRequireReceiver);

                yield return new WaitForSeconds(0.2f);

                isAttacking = false;
            }

            yield return null;
        }
    }

    /// <summary>
    /// 무기를 들고 있을 때 좌클릭하면 평타
    /// </summary>
    IEnumerator Skill()
    {
        while (true)
        {
            if (currentWeapon && Input.GetMouseButtonDown(1))
            {
                isAttacking = true;

                LookAtMousePos();

                yield return new WaitForSeconds(0.01f);

                currentWeapon.SendMessage("Skill", targetPos, SendMessageOptions.DontRequireReceiver);

                yield return new WaitForSeconds(0.2f);

                isAttacking = false;
            }

            yield return null;
        }
    }

    /// <summary>
    /// 무기를 들고 있을 때 좌클릭하면 평타
    /// </summary>
    IEnumerator Dash()
    {
        while (true)
        {
            if (currentWeapon && Input.GetKeyDown(KeyCode.Space))
            {
                currentWeapon.SendMessage("Dash", SendMessageOptions.DontRequireReceiver);
            }

            yield return null;
        }
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
    /// 슬라임과 오브젝트 사이의 거리를 구함
    /// </summary>
    /// <param name="target">거리를 구할 오브젝트</param>
    float GetDistance(Transform targetPos)
    {
        Vector3 offset = transform.position - targetPos.position;

        return offset.sqrMagnitude;
    }

    #region 움직임
    /// <summary>
    /// 슬라임의 움직임
    /// </summary>
    void Move()
    {
        if (isAttacking) return;

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

    ///// <summary>
    ///// 스페이스 바를 누르면 대시
    ///// </summary>
    //void Dash()
    //{
    //    if (currentWeapon) return;          // 무기 장착 중에는 무기의 대시

    //    if (Input.GetKeyDown(KeyCode.Space) && dashCoolTime <= 0)
    //    {
    //        isDash = true;
    //    }

    //    if (dashTime <= 0)
    //    {
    //        defaultTime = 0.1f;

    //        if (isDash)
    //        {
    //            animState = AnimState.idle;

    //            rigid.position += Vector3.forward * dashSpeed * Time.deltaTime;

    //            dashTime = defaultTime;
    //            dashCoolTime = dashDefaultCoolTime;
    //        }
    //    }
    //    else
    //    {
    //        dashTime -= Time.deltaTime;
    //        defaultTime = dashTime;
    //    }

    //    if (dashCoolTime > 0f)
    //    {
    //        dashCoolTime -= Time.deltaTime;
    //    }

    //    isDash = false;
    //}
    #endregion

    #region 무기
    /// <summary>
    /// 주변에 있는 무기 감지
    /// </summary>
    void DetectWeapon()
    {
        colliders = Physics.OverlapSphere(transform.position, detectRadius, weaponLayer);

        if (colliders.Length == 1)      // 감지한 무기가 한 개일 때
        {
            EquipWeapon(0);
        }
        else if (colliders.Length > 1)
        {
            // 감지한 무기들 중 제일 가까운 거리에 있는 무기를 장착
            int minIndex = 0;
            float minDis = GetDistance(colliders[0].transform);

            for (int i = 1; i < colliders.Length; i++)          // 가까운 거리에 있는 무기 찾기
            {
                float distance = GetDistance(colliders[i].transform);

                if (minDis > distance)
                {
                    minDis = distance;
                    minIndex = i;
                }
            }

            EquipWeapon(minIndex);
        }
    }

    /// <summary>
    /// 감지한 무기 장착
    /// </summary>
    /// <param name="index"></param>
    void EquipWeapon(int index)
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (currentWeapon)
            {
                currentWeapon.gameObject.layer = 6;
                ObjectPoolingManager.Instance.Set(currentWeapon);
                currentWeapon = null;
            }

            colliders[index].SendMessage("DoAttach", SendMessageOptions.DontRequireReceiver);
        }
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
        weapon.transform.localEulerAngles = Vector3.zero;
        stats = weapon.stats;

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

    void LookAtMousePos()
    {
        mousePos = Input.mousePosition;
        mousePos.z = 10f;    // 마우스와 슬라임 사이의 간격

        if (!IsHitMonster())         // 몬스터를 클릭하지 않았을 때
            targetPos = Camera.main.ScreenToWorldPoint(mousePos);

        targetPos.y = transform.position.y;
        transform.LookAt(targetPos);            // 마우스의 위치를 바라봄
    }

    /// <summary>
    /// 몬스터를 클릭했는지?
    /// </summary>
    /// <returns></returns>
    bool IsHitMonster()
    {
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.CompareTag("Monster"))                // 몬스터 클릭 시
            {
                targetPos = hit.transform.position;         // 슬라임이 바라볼 위치
                return true;
            }
        }

        return false;
    }
    #endregion

    #endregion
}