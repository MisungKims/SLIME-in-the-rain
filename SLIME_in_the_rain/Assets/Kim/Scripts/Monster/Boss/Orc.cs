/**
 * @brief ��ũ ����
 * @author ��̼�
 * @date 22-07-12
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orc : Boss
{
    private float chaseCount;
    private float maxCount = 2f;

    private float stunTime = 2f;        // �������� ������ �ð�

    protected override void Awake()
    {
        base.Awake();

        bossName = "��ũ";
        SetHPBar();
    }

    // �����ӿ��� �������� ����
    public override void DamageSlime(int atkType)
    {
        base.DamageSlime(atkType);

       if(atkType == 1) slime.Stun(stunTime);       // �ι�° ������ ����
    }

    // �������� ����
    protected override IEnumerator Chase()
    {
        while (CanChase())
        {
            // ������ ���� ���� �ȿ� �������� �ִٸ� �ܰŸ� ���� ����
            atkRangeColliders = Physics.OverlapSphere(transform.position, stats.attackRange, slimeLayer);
            if (atkRangeColliders.Length > 0)
            {
                isInRange = true;
                if (!isAttacking && canAttack) StartCoroutine(ShortAttack());

                chaseCount = 0;
            }
            else if (atkRangeColliders.Length <= 0)         // ���� ������ �������� ���ٸ� 3��~5�� �Ŀ� ���Ÿ� ����
            {
                isInRange = false;
                if (!isAttacking)
                {
                    PlayAnim(EMonsterAnim.run);

                    chaseCount += Time.deltaTime;

                    if (chaseCount >= maxCount)
                    {
                        yield return StartCoroutine(LongAttack());
                    }
                }
            }

            if (!isAttacking) nav.SetDestination(target.position);

            yield return null;
        }

        isChasing = false;
    }

    // �ܰŸ� ���� �ڷ�ƾ
    private IEnumerator ShortAttack()
    {
        canAttack = false;

        nav.SetDestination(target.position);
        transform.LookAt(target);

        chaseCount = 0;
        IsAttacking = true;

        randAttack = Random.Range(0, 2);
        anim.SetInteger("attack", randAttack);

        PlayAnim(EMonsterAnim.attack);
        yield return new WaitForSeconds(0.15f);
        Debug.Log(anim.GetCurrentAnimatorStateInfo(0).fullPathHash);
        if (anim.GetCurrentAnimatorStateInfo(0).fullPathHash == -270903256) soundManager.Play("Boss1/Attack", SoundType.SFX);

        // ���� �ִϸ��̼��� ���� �� ���� ���
        while (!canAttack)
        {
            yield return null;
        }

        // ������ �ð����� ���
        // ��� �� ���� ������ ����� �ٷ� �Ѿư�
        randAtkTime = Random.Range(minAtkTime, maxAtkTime);
        while (randAtkTime > 0 && isInRange)
        {
            randAtkTime -= Time.deltaTime;

            yield return null;
        }

        IsAttacking = false;
    }

    // ���Ÿ� ���� �ڷ�ƾ (���鼭 �����ӿ��� ������ ��)
    private IEnumerator LongAttack()
    {
        canAttack = false;

        nav.SetDestination(target.position);
        chaseCount = 0;
        IsAttacking = true;
        nav.speed *= 4;

        // �ִϸ��̼� ����
        randAttack = 2;
        anim.SetInteger("attack", 1);
        PlayAnim(EMonsterAnim.attack);
        soundManager.Play("Boss1/Attack", SoundType.SFX);

        while (!canAttack)      // �ִϸ��̼��� ���� �� ����
        {
            nav.SetDestination(target.position);

            yield return null;
        }

        nav.speed *= 0.25f;
        IsAttacking = false;

        maxCount = Random.Range(3f, 6f);
    }
}
