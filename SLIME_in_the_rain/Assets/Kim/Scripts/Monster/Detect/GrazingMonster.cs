/**
 * @details �������� �Ծ�� �����ϴ� ����� ����
 * @author ��̼�
 * @date 22-07-05
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;  // OnDrawGizmos

public class GrazingMonster : Monster
{
    #region ����
    // ����
    private bool takeDamage;            // �������� �Ծ�����?
    private bool isCounting;            // ���� ī������ �����ߴ���?

    private float originCountTime = 10f;    // �⺻ ī���� �ð�
    private float countTime;                // ī�����ؾ��ϴ� �ð�

    // ĳ��
    private WaitForSeconds waitFor1s = new WaitForSeconds(1f);
    private Slime slime;
    #endregion

    #region ����Ƽ �Լ�
    protected override void Start()
    {
        slime = Slime.Instance;

        base.Start();
    }
    #endregion

    #region �ڷ�ƾ
    // ������ ������ �����ϰ� �ð��� ������ ������ ���ϸ� ���� ����
    IEnumerator ChaseTimeCount()
    {
        isCounting = true;
        takeDamage = false;
        countTime = originCountTime;

        while (isChasing)
        {
            for (int i = 0; i < countTime; i++)
            {
                if(takeDamage)                      // ī��Ʈ ���� ���� �������� �Ծ��ٸ�, ī��Ʈ �ð��� ������Ŵ
                {
                    countTime += originCountTime;
                    takeDamage = false;
                }

                yield return waitFor1s;
            }
            
            if (isChasing && !isAttacking) StopChase();
        }

        isCounting = false;
    }

    IEnumerator DoStun(float time)
    {
        isStun = true;
        PlayAnim(EMonsterAnim.stun);

        yield return new WaitForSeconds(time);

        isStun = false;
        Chase();
    }
    #endregion

    #region �Լ�
    // �������� �ѱ� ����
    void Chase()
    {
        takeDamage = true;

        StartChase(slime.transform);

        if(!isCounting)
        {
            StartCoroutine(ChaseTimeCount());
        }
    }

    public override void AutoAtkDamaged()
    {
        base.AutoAtkDamaged();

        Chase();
    }

    public override void SkillDamaged()
    {
        base.SkillDamaged();

        Chase();
    }

    public override void Stun(float stunTime)
    {
        float damage = statManager.GetSkillDamage();
        stats.HP -= damage;
        ShowDamage(damage);

        StartCoroutine(DoStun(stunTime));
    }
    #endregion
}

