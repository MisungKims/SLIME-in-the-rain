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
    private GameObject hpBarObject;
    private Slider hpBar;
    private Vector3 hpBarPos;

    private Vector3 lookRot;

    [SerializeField]
    private Transform projectilePos;

    private Camera mainCam;
    #endregion

    #region ����Ƽ �Լ�
    protected override void Awake()
    {
        base.Awake();

        mainCam = Camera.main;
        projectileAtk = 2;

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
        while (hpBarObject)
        {
            hpBarObject.transform.position = mainCam.WorldToScreenPoint(transform.position + hpBarPos);

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

        PlayAnim(EMonsterAnim.attack);

        // ������ �ð����� ���
        randAtkTime = Random.Range(minAtkTime, maxAtkTime);
        yield return new WaitForSeconds(randAtkTime);

        IsAttacking = false;
        noDamage = false;
    }

    // ü�¹� Ȱ��ȭ
    public override void ShowHPBar()
    {
        if (!hpBar)
        {
            hpBarObject = uiPoolingManager.Get(EUIFlag.hpBar);
            hpBar = hpBarObject.transform.GetChild(0).GetComponent<Slider>();
            hpBar.maxValue = stats.maxHP;

            StartCoroutine(SetHPBarPos());
        }

        hpBar.value = stats.HP;
    }

    // ü�¹� ��Ȱ��ȭ
    public override void HideHPBar()
    {
        if (!hpBar) return;

        uiPoolingManager.Set(hpBarObject, EUIFlag.hpBar);
        hpBar = null;
    }
}
