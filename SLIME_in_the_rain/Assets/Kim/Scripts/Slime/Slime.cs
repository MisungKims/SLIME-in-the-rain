/**
 * @brief ������ ������Ʈ
 * @author ��̼�
 * @date 22-07-24
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Slime : MonoBehaviour
{
    #region ����
    #region �̱���
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
    private SkinnedMeshRenderer skinnedMesh;            // �������� Material
    public SkinnedMeshRenderer SkinnedMesh { get { return skinnedMesh; } }
    [SerializeField]
    private Material baseMat;

    private Stats stat;
    public Stats Stat { get { return stat; } }

    public bool isDie;

    //////// ����
    [Header("------------ ����")]
    public Transform weaponPos;     // ���� ���� �� ������ parent

    public Weapon currentWeapon;    // ���� ���� ����

    [SerializeField]
    private LayerMask weaponLayer;

    private float detectRadius = 1f;      // ���⸦ ������ ����

    Collider[] colliders;
    Outline outline;

    //////// ���
    [Header("------------ ���")]
    // ��� �Ÿ�
    public float originDashDistance = 5.5f;
    private float dashDistance;
    public float DashDistance { set { dashDistance = value; } }

    // ��� ���� �ð�
    public float originDashTime = 0.4f;
    private float dashTime;
    public float DashTime { get { return dashTime; } set { dashTime = value; } }
    private float currentDashTime;


    public bool isDash { get; set; }                // ��� ������?
    public bool isCanDash;     // ��� ��������?

    public GameObject shield;

    //////// ����
    public bool canAttack;
    public Transform target;

    public bool isAttacking;   // ��Ÿ ������?

    public bool isStealth;      // ���� ������?

    //////// ������
    private bool isStun;
    private Color red = new Color(255, 83, 83, 255);

    //////// �̵�
    enum AnimState { idle, move, dash, damaged, die }     // �ִϸ��̼��� ����
    AnimState animState = AnimState.idle;

    private Vector3 direction;                  // �̵� ����

    public bool canMove = true;

    private bool isInWater = false;
    public bool IsInWater { get { return isInWater; } }

    private float decreaseHPAmount = 0.5f;  // �� �ȿ��� ���ҵ� ü���� ��

    [SerializeField]
    private MinimapWorldObject minimapWorldObject;

    //////// ĳ��
    private WaitForSeconds waitForAttack = new WaitForSeconds(0.2f);       // ������ ��ٸ���
    private WaitForSeconds waitFor2s = new WaitForSeconds(2f);
    private WaitForSeconds waitForDash;

    public StatManager statManager;
    private MainCanvas mainCanvas;

    #endregion

    #region ����Ƽ �Լ�
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

    // ������ �� �� �ֵ���
    public void SetCanAttack()
    {
        canAttack = true;
        isDie = false;
        canMove = true;
        isAttacking = false;
        isStun = false;
    }

    #region �ڷ�ƾ
    // ���⸦ ��� ���� �� ��Ŭ���ϸ� ��Ÿ
    IEnumerator AutoAttack()
    {
        while (true)
        {
            if (canAttack && !isDie && canMove && !isAttacking && currentWeapon && !isStun && Input.GetMouseButtonDown(0))
            {
                isAttacking = true;

                if(currentWeapon) currentWeapon.SendMessage("AutoAttack", SendMessageOptions.DontRequireReceiver);

                yield return new WaitForSeconds(0.01f);           // �� ������ ���� ���ȿ� ���� ���

                isAttacking = false;
            }

            yield return null;
        }
    }

    // ���⸦ ��� ���� �� ��Ŭ���ϸ� ��ų
    IEnumerator Skill()
    {
        while (true)
        {
            if (IsCanSkill())
            {
                isAttacking = true;

                currentWeapon.SendMessage("Skill", SendMessageOptions.DontRequireReceiver);

                yield return waitForAttack;         // 0.2�� ���

                isAttacking = false;
            }

            yield return null;
        }
    }

    // �����̽��� ������ ������ ���
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

    // ��� �ڷ�ƾ
    IEnumerator DoDash()
    {
        isCanDash = false;

        PlayAnim(AnimState.dash);       // ��� �ִϸ��̼� ����

        currentDashTime = dashTime;
        while (currentDashTime > 0 && !isFrontWall)
        {
            currentDashTime -= Time.deltaTime;
            transform.position += transform.forward * dashDistance * Time.deltaTime;
            yield return null;
        }

        PlayAnim(AnimState.idle);       // ��� �ִϸ��̼� ����

        dashDistance = originDashDistance;
        dashTime = originDashTime;

        isDash = false;
        isCanDash = true;
    }

    // �տ� ���� �ִ��� ����
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

    // ���� �ڷ�ƾ
    IEnumerator DoStun(float stunTime)
    {
        isStun = true;
        PlayAnim(AnimState.damaged);

        yield return new WaitForSeconds(stunTime);

        isStun = false;
    }

    // �� ���� �ִ��� ����
    private IEnumerator DetectWater()
    {
        while (true)
        {
            // �������� ��ġ���� ���� �Ÿ���ŭ ray�� ��
            RaycastHit hit;
            if (Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, out hit, 2f))
            {
                if (hit.transform.gameObject.layer == 4)
                {
                    isInWater = true;       // water ���̾��� ��

                    // �� �������� �׸��ڸ� ����
                    if (SkinnedMesh.shadowCastingMode.Equals(UnityEngine.Rendering.ShadowCastingMode.On))
                        SkinnedMesh.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                }
                else
                {
                    isInWater = false;

                    // �� �������� �׸��� ����
                    if (SkinnedMesh.shadowCastingMode.Equals(UnityEngine.Rendering.ShadowCastingMode.Off))
                        SkinnedMesh.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                }
            }

            yield return null;
        }
    }

    // �� ���� ������ ü�� ����
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

    #region �Լ�
    // �����Ӱ� ������Ʈ ������ �Ÿ��� ����
    public float GetDistance(Transform target)
    {
        Vector3 offset = transform.position - target.position;

        return offset.sqrMagnitude;
    }

    // �ִϸ��̼� �÷���
    void PlayAnim(AnimState state)
    {
        animState = state;

        anim.SetInteger("animation", (int)animState);
    }

    #region ������
    // �������� ������
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

                transform.rotation = Quaternion.Euler(0, angle, 0);         // ȸ��
            }

           if (!isFrontWall)  transform.position += direction * 3*statManager.myStats.moveSpeed * Time.deltaTime;   // �̵�
        }
        else
        {
            animState = AnimState.idle;
        }

        PlayAnim(animState);
    }


    // ���
    public void Dash()
    {
        // ��ø� �� �� ���� �� return
        if (!canMove || !isCanDash || isStun || isDie)
        {
            isDash = false;
            return;
        }

        StartCoroutine(DoDash());
    }
    #endregion

    #region ����
    // ��ų�� ����� �� �ִ���?
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

    #region ����
    Collider lastCollider;

    // �ֺ��� �ִ� ���� ����
    void DetectWeapon()
    {
        if (isDie) return;
        
        colliders = Physics.OverlapSphere(transform.position, detectRadius, weaponLayer);

        if (colliders.Length == 1)      // ������ ���Ⱑ �� ���� ��
        {
            if (lastCollider) DisableOutline(lastCollider);
            lastCollider = colliders[0];

            EquipWeapon(0);
        }
        else if (colliders.Length > 1)
        {
            // ������ ����� �� ���� ����� �Ÿ��� �ִ� ���⸦ ����
            int minIndex = -1;
            float minDis = Mathf.Infinity;

            for (int i = 0; i < colliders.Length; i++)          // ����� �Ÿ��� �ִ� ���� ã��
            {
                float distance = GetDistance(colliders[i].transform);

                if (minDis > distance)
                {
                    minDis = distance;
                    minIndex = i;
                }
            }

            if(lastCollider) DisableOutline(lastCollider);              // ������ ����� �ƿ������� ��

            lastCollider = colliders[minIndex];
            EquipWeapon(minIndex);
        }
        else
        {
            if(lastCollider) DisableOutline(lastCollider);           // �ƹ��͵� �������� ���� �� ������Ʈ�� �ƿ����� ����

            lastCollider = null;
        }
    }

    void DisableOutline(Collider collider)
    {
        outline = collider.GetComponent<Outline>();
        outline.enabled = false;
    }

    // ������ ���� G Ű�� ���� ����
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

    // �κ��丮���� Ŭ�� �� ���� ����
    public void EquipWeapon(Weapon weapon)
    {
        RemoveCurrentWeapon();

        weapon.ChangeWeapon();
    }

    // ���� ���⸦ ����
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

    // ���� ����
    public void ChangeWeapon(Weapon weapon)
    {
        currentWeapon = weapon;
        currentWeapon.gameObject.layer = 7;
        currentWeapon.GetComponent<Outline>().enabled = false;

        // ������ ��ġ ����
        ObjectPoolingManager.Instance.Set(currentWeapon.transform.parent.gameObject, EObjectFlag.weapon);
        currentWeapon.transform.parent = weaponPos;
       
        currentWeapon.transform.localPosition = Vector3.zero;

        // ������ ������ �������� ����
        statManager.ChangeWeapon(currentWeapon);

        //����� ���� ����
        if (MainCanvas.Instance) MainCanvas.Instance.changeWeapon();

        // �������� �� ����
        ChangeMaterial();              
    }

    // �������� ��(���͸���) ����
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

    //// �������� ����
    //public void Damaged(float amount)
    //{
    //    // ����� = ���� ���ݷ� * (1 - �����)
    //    // ����� = ���� / (1 + ����)

    //    float damageReduction = stat.defensePower / (1 + stat.defensePower);
    //    stat.HP -= amount * (1 - damageReduction);

    //    PlayAnim(AnimState.damaged);
    //}


    // �������� ����
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

    // ����
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

    // ������ �ʱ�ȭ
    public void InitSlime()
    {
        skinnedMesh.material = baseMat;
        RemoveCurrentWeapon();
    }

    #endregion
}