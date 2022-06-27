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

    private Rigidbody rigid;

    private Animator anim;

    [SerializeField]
    private SkinnedMeshRenderer skinnedMesh;            // 슬라임의 Material


    //////// 스탯
    private Stats originStats;      // 기본 슬라임의 스탯
    public Stats myStats;           // 현재 슬라임의 스탯
    private Stats extraStats;       // 젤라틴, 룬 등으로 추가될 양


    //////// 무기
    [Header("------------ 무기")]
    public Transform weaponPos;     // 무기 장착 시 무기의 parent

    public Weapon currentWeapon;    // 장착 중인 무기

    [SerializeField]
    private LayerMask weaponLayer;

    private float detectRadius = 1.1f;      // 무기를 감지할 범위

    Collider[] colliders;


    //////// 대시
    [Header("------------ 대시")]
    float dashDistance = 1.3f;          // 대시할 거리
    public float dashTime = 1f;        // 대시 지속 시간
    public float currentDashTime;
    public bool isDash { get; set; }                // 대시 중인지?
    bool isCanDash;     // 대시 가능한지?


    //////// 공격
    Vector3 mousePos;

    Vector3 targetPos;

    bool isAttacking;   // 평타 중인지?


    //////// 이동
    enum AnimState { idle, move, dash, damaged, die }     // 애니메이션의 상태
    AnimState animState = AnimState.idle;

    Vector3 direction;                  // 이동 방향


    //////// 캐싱
    WaitForSeconds waitForRotate = new WaitForSeconds(0.01f);       // 슬라임의 회전을 기다리는
    WaitForSeconds waitForAttack = new WaitForSeconds(0.2f);       // 공격을 기다리는

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

        isCanDash = true;
    }

    private void Start()
    {
        StartCoroutine(AutoAttack());
        StartCoroutine(Skill());
    }

    private void Update()
    {
        DetectWeapon();

        SpaceBar();
    }

    void FixedUpdate()
    {
        Move();
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

                yield return waitForRotate;         // 0.01초 대기

                currentWeapon.SendMessage("AutoAttack", targetPos, SendMessageOptions.DontRequireReceiver);

                yield return waitForAttack;         // 0.2초 대기

                isAttacking = false;
            }

            yield return null;
        }
    }

    /// <summary>
    /// 무기를 들고 있을 때 우클릭하면 스킬
    /// </summary>
    IEnumerator Skill()
    {
        while (true)
        {
            if (currentWeapon && Input.GetMouseButtonDown(1))
            {
                isAttacking = true;

                LookAtMousePos();

                yield return waitForRotate;         // 0.01초 대기

                currentWeapon.SendMessage("Skill", targetPos, SendMessageOptions.DontRequireReceiver);

                yield return waitForAttack;         // 0.2초 대기

                isAttacking = false;

                yield return new WaitForSeconds(myStats.coolTime - 0.2f);
            }

            yield return null;
        }
    }

    IEnumerator DoDash()
    {
        isCanDash = false;

        PlayAnim(AnimState.dash);       // 대시 애니메이션 실행

        currentDashTime = dashTime;
        while (currentDashTime >= 0)
        {
            transform.position += transform.forward * dashDistance * Time.deltaTime * 0.8f;

            currentDashTime -= Time.deltaTime;
        }

        isDash = false;

        yield return new WaitForSeconds(0.5f);

        isCanDash = true;
    }
    #endregion

    #region 함수
    /// <summary>
    /// 스탯 초기화
    /// </summary>
    void InitStats()
    {
        originStats = new Stats(100, 1f, 1.2f, 1f, 1f, 1f);
        myStats = new Stats(100, 1f, 1.2f, 1f, 1f, 1f);
        extraStats = new Stats(0f, 0f, 0f, 0f, 0f, 0f);
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

    /// <summary>
    /// 애니메이션 플레이
    /// </summary>
    /// <param name="state"></param>
    void PlayAnim(AnimState state)
    {
        animState = state;

        anim.SetInteger("animation", (int)animState);
    }

    #region 움직임
    // 슬라임의 움직임
    void Move()
    {
        if (isAttacking || isDash) return;

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
            rigid.position += direction * myStats.moveSpeed * Time.deltaTime;   // 이동
        }
        else
        {
            animState = AnimState.idle;
        }

        PlayAnim(animState);
    }


    // 스페이스바 누르면 앞으로 대시
    void SpaceBar()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isDash)
        {
            isDash = true;

            if (currentWeapon)
            {
                currentWeapon.SendMessage("Dash", this, SendMessageOptions.DontRequireReceiver);
            }
            else
            {
                Dash();
            }
        }
    }

    // 대시
    public void Dash()
    {
        if (!isCanDash)
        {
            isDash = false;
            return;
        }

        StartCoroutine(DoDash());
    }

    
    #endregion

    #region 공격

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

        currentWeapon.transform.parent = weaponPos;
        currentWeapon.transform.localPosition = Vector3.zero;

        ChangeStats(currentWeapon);            // 변경한 무기의 스탯으로 변경

        ChangeMaterial();               // 슬라임의 색 변경
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

    #region 스탯
    /// <summary>
    /// 무기 변경 시 해당 무기의 스탯으로 변경
    /// </summary>
    void ChangeStats(Weapon weapon)
    {
        myStats.HP = weapon.stats.HP + extraStats.HP;
        myStats.coolTime = weapon.stats.coolTime + extraStats.coolTime;
        myStats.moveSpeed = weapon.stats.moveSpeed + extraStats.moveSpeed;
        myStats.attackSpeed = weapon.stats.attackSpeed + extraStats.attackSpeed;
        myStats.attackPower = weapon.stats.attackPower + extraStats.attackPower;
        myStats.defensePower = weapon.stats.defensePower + extraStats.defensePower;
    }

    /// <summary>
    /// HP 스탯 추가
    /// </summary>
    /// <param name="amount">추가할 HP의 양</param>
    public void AddHP(float amount)
    {
        extraStats.HP += amount;
        if (currentWeapon)
        {
            myStats.HP = currentWeapon.stats.HP + extraStats.HP;
        }
        else
        {
            myStats.HP = originStats.HP + extraStats.HP;
        }
    }

    public void AddCoolTime(float amount)
    {
        extraStats.coolTime += amount;
        if (currentWeapon)
        {
            myStats.coolTime = currentWeapon.stats.coolTime + extraStats.coolTime;
        }
        else
        {
            myStats.coolTime = originStats.coolTime + extraStats.coolTime;
        }
    }

    public void AddMoveSpeed(float amount)
    {
        extraStats.moveSpeed += amount;
        if (currentWeapon)
        {
            myStats.moveSpeed = currentWeapon.stats.moveSpeed + extraStats.moveSpeed;
        }
        else
        {
            myStats.moveSpeed = originStats.moveSpeed + extraStats.moveSpeed;
        }
    }

    public void AddAttackSpeed(float amount)
    {
        extraStats.attackSpeed += amount;
        if (currentWeapon)
        {
            myStats.attackSpeed = currentWeapon.stats.attackSpeed + extraStats.attackSpeed;
        }
        else
        {
            myStats.attackSpeed = originStats.attackSpeed + extraStats.attackSpeed;
        }
    }

    public void AddAttackPower(float amount)
    {
        extraStats.attackPower += amount;
        if (currentWeapon)
        {
            myStats.attackPower = currentWeapon.stats.attackPower + extraStats.attackPower;
        }
        else
        {
            myStats.attackPower = originStats.attackPower + extraStats.attackPower;
        }
    }

    public void AddDefensePower(float amount)
    {
        extraStats.defensePower += amount;
        if (currentWeapon)
        {
            myStats.defensePower = currentWeapon.stats.defensePower + extraStats.defensePower;
        }
        else
        {
            myStats.defensePower = originStats.defensePower + extraStats.defensePower;
        }
    }
    #endregion

    #endregion
}