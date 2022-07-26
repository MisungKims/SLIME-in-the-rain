/**
 * @brief Metalon�� ���� �Ź�
 * @author ��̼�
 * @date 22-07-14
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MetalonBaby : Monster
{
    #region ����
    // ü�¹�
    private Slider hpBar;
    private Vector3 hpBarPos;

    private Vector3 lookRot;

    [SerializeField]
    private Transform projectilePos;
    #endregion

    #region ����Ƽ �Լ�
    protected override void Awake()
    {
        base.Awake();

        projectileAtk = 2;
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        TryStartChase();
    }
    #endregion


    protected override IEnumerator DieCoroutine()
    {
        yield return waitFor3s;

        this.gameObject.SetActive(false);
    }

    // ü�¹��� ��ġ�� �����ϴ� �ڷ�ƾ
    IEnumerator SetHPBarPos()
    {
        while (hpBar)
        {
            hpBarPos = transform.position;
            hpBarPos.y += 1.5f;

            hpBar.transform.position = hpBarPos;

            yield return null;
        }
    }

    // ���� �ڷ�ƾ
    protected override IEnumerator Attack()
    {
        canAttack = false;

        nav.SetDestination(transform.position);
        transform.LookAt(target);

        IsAttacking = true;

        // ���� ����� �������� ���� (TODO : Ȯ��)
        randAttack = Random.Range(0, attackTypeCount);
        anim.SetInteger("attack", randAttack);

        if (randAttack == 2)
        {
            StartCoroutine(ProjectileAttack());        // ����ü �߻� ����
        }

        PlayAnim(EMonsterAnim.attack);

        // ������ �ð����� ���
        randAtkTime = Random.Range(minAtkTime, maxAtkTime);
        yield return new WaitForSeconds(randAtkTime);

        IsAttacking = false;
        noDamage = false;
    }

    private IEnumerator ProjectileAttack()
    {
        noDamage = true;        // �� ������ �������� ����ü�� �¾��� �� �������� �Ծ���ϹǷ� noDamage�� true�� ����

        yield return new WaitForSeconds(0.8f);

        GetProjectile();
    }

    // ����ü �߻� ����
    private void GetProjectile()
    {
        // ����ü �߻�
        MonsterProjectile projectile = ObjectPoolingManager.Instance.Get(EProjectileFlag.spider).GetComponent<MonsterProjectile>();
        projectile.monster = this;

        projectile.transform.position = projectilePos.position;
        projectile.transform.LookAt(target);

        lookRot = projectile.transform.eulerAngles;
        lookRot.x = 0;
        lookRot.z = 0;

        projectile.transform.eulerAngles = lookRot;
    }

    // ü�¹� Ȱ��ȭ
    public override void ShowHPBar()
    {
        if (!hpBar)
        {
            hpBar = uiPoolingManager.Get(EUIFlag.hpBar).GetComponent<Slider>();
            hpBar.maxValue = stats.maxHP;

            StartCoroutine(SetHPBarPos());
        }

        hpBar.value = stats.HP;
    }

    // ü�¹� ��Ȱ��ȭ
    public override void HideHPBar()
    {
        if (!hpBar) return;

        uiPoolingManager.Set(hpBar.gameObject, EUIFlag.hpBar);
        hpBar = null;
    }
}
