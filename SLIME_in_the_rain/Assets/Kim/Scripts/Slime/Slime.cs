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
    public SkinnedMeshRenderer SkinnedMesh { get { return skinnedMesh; } }

    private Stats stat;
    public Stats Stat { get { return stat; } }

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
    private float originDashDistance = 1.4f;          // 대시할 거리
    private float dashDistance = 1.4f;
    public float DashDistance { set { dashDistance = value; } }
    public float dashTime = 1f;        // 대시 지속 시간
    public float currentDashTime;
    public bool isDash { get; set; }                // 대시 중인지?
    bool isCanDash;     // 대시 가능한지?


    //////// 공격
    Vector3 mousePos;

    Vector3 targetPos;
    public Transform target;

    public bool isAttacking;   // 평타 중인지?

    public bool isStealth;      // 은신 중인지?

    // 데미지
    private bool isStun;


    //////// 이동
    enum AnimState { idle, move, dash, damaged, die }     // 애니메이션의 상태
    AnimState animState = AnimState.idle;

    Vector3 direction;                  // 이동 방향


    //////// 캐싱
    private WaitForSeconds waitForRotate = new WaitForSeconds(0.01f);       // 슬라임의 회전을 기다리는
    private WaitForSeconds waitForAttack = new WaitForSeconds(0.2f);       // 공격을 기다리는

    [SerializeField]
    public StatManager statManager;

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

        isCanDash = true;
    }

    private void Start()
    {
        stat = statManager.myStats;
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

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(transform.position, detectRadius);
    //}
    #endregion

    #region 코루틴
    // 무기를 들고 있을 때 좌클릭하면 평타
    IEnumerator AutoAttack()
    {
        while (true)
        {
            if (!isAttacking && currentWeapon && Input.GetMouseButtonDown(0))
            {
                isAttacking = true;

                LookAtMousePos();

                yield return waitForRotate;         // 0.01초 대기

                currentWeapon.SendMessage("AutoAttack", targetPos, SendMessageOptions.DontRequireReceiver);

                yield return new WaitForSeconds(stat.attackSpeed);           // 각 무기의 공속 스탯에 따라 대기

                isAttacking = false;
            }

            yield return null;
        }
    }

    // 무기를 들고 있을 때 우클릭하면 스킬
    IEnumerator Skill()
    {
        while (true)
        {
            if (IsCanSkill())
            {
                isAttacking = true;

                LookAtMousePos();

                yield return waitForRotate;         // 0.01초 대기

                currentWeapon.SendMessage("Skill", targetPos, SendMessageOptions.DontRequireReceiver);

                yield return waitForAttack;         // 0.2초 대기

                isAttacking = false;
            }

            yield return null;
        }
    }

    // 대시 코루틴
    IEnumerator DoDash()
    {
        isCanDash = false;

        PlayAnim(AnimState.dash);       // 대시 애니메이션 실행

        currentDashTime = dashTime;
        while (currentDashTime >= 0)
        {
            transform.position += transform.forward * dashDistance * Time.deltaTime;

            currentDashTime -= Time.deltaTime;
        }

        isDash = false;

        yield return new WaitForSeconds(0.5f);

        dashDistance = originDashDistance;
        isCanDash = true;
    }

    // 스턴 코루틴
    IEnumerator DoStun(float stunTime)
    {
        isStun = true;
        PlayAnim(AnimState.damaged);

        yield return new WaitForSeconds(stunTime);

        isStun = false;
    }
    #endregion

    #region 함수
    // 슬라임과 오브젝트 사이의 거리를 구함
    float GetDistance(Transform target)
    {
        Vector3 offset = transform.position - target.position;

        return offset.sqrMagnitude;
    }

    // 애니메이션 플레이
    void PlayAnim(AnimState state)
    {
        animState = state;

        anim.SetInteger("animation", (int)animState);
    }

    #region 움직임
    // 슬라임의 움직임
    void Move()
    {
        if (isAttacking || isDash || isStun) return;

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

            transform.position += direction * stat.moveSpeed * Time.deltaTime;   // 이동
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
        // 대시를 할 수 없을 때 return
        if (!isCanDash || isStun)
        {
            isDash = false;
            return;
        }

        StartCoroutine(DoDash());
    }

    
    #endregion

    #region 공격
    // 스킬을 사용할 수 있는지?
    bool IsCanSkill()
    {
        if (!isAttacking && currentWeapon && currentWeapon.isCanSkill && Input.GetMouseButtonDown(1))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // 마우스로 클릭한 위치를 바라봄
    void LookAtMousePos()
    {
        mousePos = Input.mousePosition;
        mousePos.z = 10f;    // 마우스와 슬라임 사이의 간격

        // 오브젝트를 클릭하지 않았을 때는 마우스 위치를 바라보고,
        // 오브젝트를 클릭했을 때는 오브젝트 위치를 바라봄
        if (!IsHitObject())        
            targetPos = Camera.main.ScreenToWorldPoint(mousePos);

        targetPos.y = transform.position.y;
        transform.LookAt(targetPos);            // 마우스의 위치를 바라봄
    }

    // 오브젝트를 클릭했는지?
    bool IsHitObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.CompareTag("DamagedObject") || hit.transform.CompareTag("Land"))
            {
                targetPos = hit.transform.position;         // 슬라임이 바라볼 위치

                if (hit.transform.gameObject.layer == 8)        // 몬스터 레이어면 target을 설정 (유도를 위해)
                {
                    target = hit.transform;
                }

                return true;
            }
        }

        return false;
    }

    #endregion

    #region 무기
    // 주변에 있는 무기 감지
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

    // 감지한 무기 장착
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

    // 무기 변경
    public void ChangeWeapon(Weapon weapon)
    {
        currentWeapon = weapon;

        currentWeapon.transform.parent = weaponPos;
        currentWeapon.transform.localPosition = Vector3.zero;

        statManager.ChangeStats(currentWeapon);            // 변경한 무기의 스탯으로 변경

        ChangeMaterial();               // 슬라임의 색 변경
    }

    // 슬라임의 색(머터리얼) 변경
    void ChangeMaterial()
    {
        if (currentWeapon)
        {
            skinnedMesh.material = currentWeapon.slimeMat;
        }
    }

    #endregion

    //// 데미지를 입음
    //public void Damaged(float amount)
    //{
    //    // 대미지 = 몬스터 공격력 * (1 - 방어율)
    //    // 방어율 = 방어력 / (1 + 방어력)

    //    float damageReduction = stat.defensePower / (1 + stat.defensePower);
    //    stat.HP -= amount * (1 - damageReduction);

    //    PlayAnim(AnimState.damaged);
    //}


    //// 슬라임의 평타에 데미지를 입음
    //public void AutoAtkDamaged()
    //{
    //    PlayAnim(AnimState.damaged);
    //    Debug.Log("AutoAtkDamaged");
    //    // 대미지 = 몬스터 공격력 * (1 - 방어율)
    //    // 방어율 = 방어력 / (1 + 방어력)

    //    //float damageReduction = stat.defensePower / (1 + stat.defensePower);
    //    //stat.HP -= amount * (1 - damageReduction);

    //    //
    //}

    //// 슬라임의스킬에 데미지를 입음
    //public void SkillDamaged()
    //{
    //    PlayAnim(AnimState.damaged);
    //    Debug.Log("SkillDamaged");
    //}

    public void Damaged(Stats monsterStats, int atkType)
    {
        PlayAnim(AnimState.damaged);
    }

    // 스턴
    public void Stun(float stunTime)
    {
        StartCoroutine(DoStun(stunTime));

        Debug.Log("Stun");
    }

   
#endregion
}