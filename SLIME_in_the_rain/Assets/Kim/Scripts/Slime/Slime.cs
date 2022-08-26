/**
 * @brief 슬라임 오브젝트
 * @author 김미성
 * @date 22-07-24
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public GameObject shootPlane;
    public int killCount = 0;
    public bool isDungeonStart = false;

    [SerializeField]
    private LifePanel lifePanel;
    private int life = 1;
    public int Life
    {
        get { return life; }
        set { life = value; }
    }

    public Rigidbody rigid;
    public RigidbodyConstraints rigidbodyConstraints;

    private Animator anim;

    [SerializeField]
    private SkinnedMeshRenderer skinnedMesh;            // 슬라임의 Material
    public SkinnedMeshRenderer SkinnedMesh { get { return skinnedMesh; } }
    [SerializeField]
    private Material baseMat;

    private Stats stat;
    public Stats Stat { get { return stat; } }

    public bool isDie;

    //////// 무기
    [Header("------------ 무기")]
    public Transform weaponPos;     // 무기 장착 시 무기의 parent

    public Weapon currentWeapon;    // 장착 중인 무기

    [SerializeField]
    private LayerMask weaponLayer;

    private float detectRadius = 1f;      // 무기를 감지할 범위

    Collider[] colliders;
    Outline outline;

    //////// 대시
    [Header("------------ 대시")]
    // 대시 거리
    public float originDashDistance = 5.5f;
    private float dashDistance;
    public float DashDistance { set { dashDistance = value; } }

    // 대시 지속 시간
    public float originDashTime = 0.4f;
    private float dashTime;
    public float DashTime { get { return dashTime; } set { dashTime = value; } }
    private float currentDashTime;


    public bool isDash { get; set; }                // 대시 중인지?
    public bool isCanDash;     // 대시 가능한지?

    public GameObject shield;

    //////// 공격
    public bool canAttack;
    public Transform target;

    public bool isAttacking;   // 평타 중인지?

    public bool isStealth;      // 은신 중인지?

    //////// 데미지
    private bool isStun;
    private Color red = new Color(255, 83, 83, 255);

    //////// 이동
    enum AnimState { idle, move, dash, damaged, die }     // 애니메이션의 상태
    AnimState animState = AnimState.idle;

    private Vector3 direction;                  // 이동 방향

    public bool canMove = true;

    private bool isInWater = false;
    public bool IsInWater { get { return isInWater; } }

    private float decreaseHPAmount = 0.5f;  // 물 안에서 감소될 체력의 양

    [SerializeField]
    private MinimapWorldObject minimapWorldObject;

    //////// 캐싱
    private WaitForSeconds waitForAttack = new WaitForSeconds(0.2f);       // 공격을 기다리는
    private WaitForSeconds waitFor2s = new WaitForSeconds(2f);
    private WaitForSeconds waitForDash;

    public StatManager statManager;
    private MainCanvas mainCanvas;

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
        rigidbodyConstraints = rigid.constraints;
        anim = GetComponent<Animator>();
        shield.SetActive(false);

        
    }

    private void OnEnable()
    {
        dashDistance = originDashDistance;
        dashTime = originDashTime;
        isCanDash = true;

        isInWater = false;
        SkinnedMesh.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;

        SetCanAttack();

        stat = statManager.myStats;
        StartCoroutine(AutoAttack());
        StartCoroutine(Skill());
        StartCoroutine(DecreaseHPInWater());
        StartCoroutine(DetectWater());
        StartCoroutine(DetectWall());
    }

    //private void Start()
    //{
    //    stat = statManager.myStats;
    //    StartCoroutine(AutoAttack());
    //    StartCoroutine(Skill());
    //    StartCoroutine(DecreaseHPInWater());
    //    StartCoroutine(DetectWater());
    //    StartCoroutine(DetectWall());
    //}

    public bool isFrontWall = false;

    private void Update()
    {
        SpaceBar();
        DetectWeapon();
    }


    void FixedUpdate()
    {
        Move();
    }
    #endregion

    // 공격을 할 수 있도록
    public void SetCanAttack()
    {
        canAttack = true;
        isDie = false;
        canMove = true;
        isAttacking = false;
        isStun = false;
    }

    #region 코루틴
    // 무기를 들고 있을 때 좌클릭하면 평타
    IEnumerator AutoAttack()
    {
        while (true)
        {
            if (canAttack && !isDie && canMove && !isAttacking && currentWeapon && !isStun && Input.GetMouseButtonDown(0))
            {
                isAttacking = true;

                if(currentWeapon) currentWeapon.SendMessage("AutoAttack", SendMessageOptions.DontRequireReceiver);

                yield return new WaitForSeconds(0.01f);           // 각 무기의 공속 스탯에 따라 대기

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

                currentWeapon.SendMessage("Skill", SendMessageOptions.DontRequireReceiver);

                yield return waitForAttack;         // 0.2초 대기

                isAttacking = false;
            }

            yield return null;
        }
    }

    // 스페이스바 누르면 앞으로 대시
    void SpaceBar()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isDash && canMove)
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

    // 대시 코루틴
    IEnumerator DoDash()
    {
        isCanDash = false;

        PlayAnim(AnimState.dash);       // 대시 애니메이션 실행

        currentDashTime = dashTime;
        while (currentDashTime > 0 && !isFrontWall)
        {
            currentDashTime -= Time.deltaTime;
            transform.position += transform.forward * dashDistance * Time.deltaTime;
            yield return null;
        }

        PlayAnim(AnimState.idle);       // 대시 애니메이션 실행

        dashDistance = originDashDistance;
        dashTime = originDashTime;

        isDash = false;
        isCanDash = true;
    }

    // 앞에 벽이 있는지 감지
    IEnumerator DetectWall()
    {
        while (true)
        {
            if (Physics.Raycast(transform.position + Vector3.up * 0.1f, transform.forward, out RaycastHit hit, 0.7f))
            {
                if (hit.transform.gameObject.layer == 11)
                {
                    isFrontWall = true;
                    transform.position = transform.position;
                }
                else isFrontWall = false;
            }
            else isFrontWall = false;

            yield return null;
        }
    }

    // 스턴 코루틴
    IEnumerator DoStun(float stunTime)
    {
        isStun = true;
        PlayAnim(AnimState.damaged);

        yield return new WaitForSeconds(stunTime);

        isStun = false;
    }

    // 물 위에 있는지 감지
    private IEnumerator DetectWater()
    {
        while (true)
        {
            // 슬라임의 위치에서 공격 거리만큼 ray를 쏨
            RaycastHit hit;
            if (Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, out hit, 2f))
            {
                if (hit.transform.gameObject.layer == 4)
                {
                    isInWater = true;       // water 레이어일 때

                    // 물 위에서는 그림자를 없앰
                    if (SkinnedMesh.shadowCastingMode.Equals(UnityEngine.Rendering.ShadowCastingMode.On))
                        SkinnedMesh.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                }
                else
                {
                    isInWater = false;

                    // 땅 위에서는 그림자 있음
                    if (SkinnedMesh.shadowCastingMode.Equals(UnityEngine.Rendering.ShadowCastingMode.Off))
                        SkinnedMesh.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                }
            }

            yield return null;
        }
    }

    // 물 위에 있으면 체력 감소
    private IEnumerator DecreaseHPInWater()
    {
        UIObjectPoolingManager uIObjectPoolingManager = UIObjectPoolingManager.Instance;
        while (true)
        {
            if (isInWater)
            {
                statManager.AddHP(-decreaseHPAmount);

                uIObjectPoolingManager.ShowInWaterText();

                yield return waitFor2s;
            }

            yield return null;
        }
    }


    #endregion

    #region 함수
    // 슬라임과 오브젝트 사이의 거리를 구함
    public float GetDistance(Transform target)
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
        if (isDie || !canMove || isAttacking || isDash || isStun) return;

        float dirX = Input.GetAxis("Horizontal");
        float dirZ = Input.GetAxis("Vertical");

        if (dirX != 0 || dirZ != 0)
        {
            animState = AnimState.move;

            direction = new Vector3(dirX, 0, dirZ);

            if (direction != Vector3.zero)
            {
                float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

                transform.rotation = Quaternion.Euler(0, angle, 0);         // 회전
            }

           if (!isFrontWall)  transform.position += direction * 3*statManager.myStats.moveSpeed * Time.deltaTime;   // 이동
        }
        else
        {
            animState = AnimState.idle;
        }

        PlayAnim(animState);
    }


    // 대시
    public void Dash()
    {
        // 대시를 할 수 없을 때 return
        if (!canMove || !isCanDash || isStun || isDie)
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
        if (canAttack && !isDie && canMove && !isAttacking && currentWeapon && currentWeapon.isCanSkill && !isStun && Input.GetMouseButtonDown(1))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    #endregion

    #region 무기
    Collider lastCollider;

    // 주변에 있는 무기 감지
    void DetectWeapon()
    {
        if (isDie) return;
        
        colliders = Physics.OverlapSphere(transform.position, detectRadius, weaponLayer);

        if (colliders.Length == 1)      // 감지한 무기가 한 개일 때
        {
            if (lastCollider) DisableOutline(lastCollider);
            lastCollider = colliders[0];

            EquipWeapon(0);
        }
        else if (colliders.Length > 1)
        {
            // 감지한 무기들 중 제일 가까운 거리에 있는 무기를 장착
            int minIndex = -1;
            float minDis = Mathf.Infinity;

            for (int i = 0; i < colliders.Length; i++)          // 가까운 거리에 있는 무기 찾기
            {
                float distance = GetDistance(colliders[i].transform);

                if (minDis > distance)
                {
                    minDis = distance;
                    minIndex = i;
                }
            }

            if(lastCollider) DisableOutline(lastCollider);              // 이전의 무기는 아웃라인을 끔

            lastCollider = colliders[minIndex];
            EquipWeapon(minIndex);
        }
        else
        {
            if(lastCollider) DisableOutline(lastCollider);           // 아무것도 감지하지 않을 때 오브젝트의 아웃라인 끄기

            lastCollider = null;
        }
    }

    void DisableOutline(Collider collider)
    {
        outline = collider.GetComponent<Outline>();
        outline.enabled = false;
    }

    // 감지한 무기 G 키를 눌러 장착
    void EquipWeapon(int index)
    {
        if (lastCollider)
        {
            outline = lastCollider.GetComponent<Outline>();
            outline.enabled = true;

            if(lastCollider.GetComponent<Weapon>())
                UIObjectPoolingManager.Instance.ShowNoWeaponText(lastCollider.GetComponent<Weapon>().wName);
        }

        

        if (Input.GetKeyDown(KeyCode.G))
        {
            if(colliders[index].transform.parent)
            {
                FieldItems fieldItems = colliders[index].transform.parent.GetComponent<FieldItems>();
                if (fieldItems) fieldItems.canDetect = false;
            }
            
            RemoveCurrentWeapon();

            outline.enabled = false;
            colliders[index].SendMessage("DoAttach", SendMessageOptions.DontRequireReceiver);
        }
    }

    // 인벤토리에서 클릭 시 무기 장착
    public void EquipWeapon(Weapon weapon)
    {
        RemoveCurrentWeapon();

        weapon.ChangeWeapon();
    }

    // 현재 무기를 없앰
    public void RemoveCurrentWeapon()
    {
        if (currentWeapon)
        {
            currentWeapon.gameObject.layer = 6;
            Destroy(currentWeapon.gameObject);
           // ObjectPoolingManager.Instance.Set(currentWeapon);
            currentWeapon = null;
        }
    }

    // 무기 변경
    public void ChangeWeapon(Weapon weapon)
    {
        currentWeapon = weapon;
        currentWeapon.gameObject.layer = 7;
        currentWeapon.GetComponent<Outline>().enabled = false;

        // 무기의 위치 설정
        ObjectPoolingManager.Instance.Set(currentWeapon.transform.parent.gameObject, EObjectFlag.weapon);
        currentWeapon.transform.parent = weaponPos;
       
        currentWeapon.transform.localPosition = Vector3.zero;

        // 변경한 무기의 스탯으로 변경
        statManager.ChangeWeapon(currentWeapon);

        //변경된 스탯 적용
        if (MainCanvas.Instance) MainCanvas.Instance.changeWeapon();

        // 슬라임의 색 변경
        ChangeMaterial();              
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

    public void Die()
    {
        if (isDie) return;
        isDie = true;
        statManager.myStats.HP = 0;
        canMove = false;

        PlayAnim(AnimState.die);

        if (DungeonManager.Instance) DungeonManager.Instance.SetMonsterHPBar();
        else if (BossMapManager.Instance) BossMapManager.Instance.SetMonsterHPBar();

        UIObjectPoolingManager.Instance.InitUI();

        life--;
        if (life <= 0) StartCoroutine(DieCoru());
        else StartCoroutine(Restart());
    }

    IEnumerator DieCoru()
    {
        yield return new WaitForSeconds(2f);

        SceneManager.LoadScene(SceneDesign.Instance.s_result);
    }
    
    IEnumerator Restart()
    {
        yield return new WaitForSeconds(1f);

        yield return StartCoroutine(lifePanel.SetUI(life + 1));

        isDie = false;
        statManager.myStats.HP = statManager.myStats.maxHP * 0.5f;
        canMove = true;

        if (BossMapManager.Instance) BossMapManager.Instance.ShowBossHPBar();

        UIObjectPoolingManager.Instance.slimeHpBarParent.SetActive(true);
    }

    //// 데미지를 입음
    //public void Damaged(float amount)
    //{
    //    // 대미지 = 몬스터 공격력 * (1 - 방어율)
    //    // 방어율 = 방어력 / (1 + 방어력)

    //    float damageReduction = stat.defensePower / (1 + stat.defensePower);
    //    stat.HP -= amount * (1 - damageReduction);

    //    PlayAnim(AnimState.damaged);
    //}


    // 데미지를 입음
    public void Damaged(Stats monsterStats, int atkType)
    {
        float damageReduction = stat.defensePower / (1 + stat.defensePower);
        float damage = monsterStats.attackPower * (1 - damageReduction) * -1;

        TakeDamage(damage);
    }

    public void Damaged(float damageAmount)
    {
        TakeDamage(-2);
    }

    private void TakeDamage(float damageAmount)
    {
        StartCoroutine(CameraShake.StartShake(0.1f, 0.05f));

        damageAmount = -2;
        statManager.AddHP(damageAmount);
        if(statManager.myStats.HP <= 0) Die();
        else PlayAnim(AnimState.damaged);
    }

    // 스턴
    public void Stun(float stunTime)
    {
        UIObjectPoolingManager.Instance.ShowStunText();

        StartCoroutine(DoStun(stunTime));

        Debug.Log("Stun");
    }

    public void RegisterMinimap()
    {
        if(Minimap.Instance) Minimap.Instance.RegisterMinimapWorldObject(minimapWorldObject);
    }

    // 슬라임 초기화
    public void InitSlime()
    {
        skinnedMesh.material = baseMat;
        RemoveCurrentWeapon();
    }

    #endregion
}