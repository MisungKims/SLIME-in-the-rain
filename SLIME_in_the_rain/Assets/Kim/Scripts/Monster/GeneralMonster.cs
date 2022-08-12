/**
 * @brief �Ϲ� ����
 * @author ��̼�
 * @date 22-07-10
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class GeneralMonster : Monster
{
    #region ����

    // �̵�
    private Vector3 randPos;
    private Vector3 offset;
    private float distance;
    private float randTime;
    private bool isStop = false;
    private int mapRange;

    // ����
    private bool takeDamage;            // �������� �Ծ�����?
    private bool isCounting;            // ���� ī������ �����ߴ���?

    private float originCountTime = 30f;    // �⺻ ī���� �ð�
    private float countTime;                // ī�����ؾ��ϴ� �ð�
    protected float addCountAmount;         // ī���� �ð� ������

    private WaitForSeconds waitFor1s = new WaitForSeconds(1f);

    // ü�¹�
    private GameObject hpBarObject;
    private Slider hpBar;
    private Vector3 hpBarPos = new Vector3(0, -0.65f, 0);

    private Camera mainCam;
    #endregion

    #region ����Ƽ �Լ�
    protected override void Awake()
    {
        base.Awake();

        addCountAmount = 10f;
        mainCam = Camera.main;

        if(DungeonManager.Instance) mapRange = DungeonManager.Instance.mapRange;
        else mapRange = 8;
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        StartCoroutine(Move());
    }
    #endregion

    #region �ڷ�ƾ

    // ���Ͱ� ������ ���ƴٴ�
    IEnumerator Move()
    {
        while (true)
        {
            if(!isChasing && !isStun && !isDie && !isHit)
            {
                nav.SetDestination(transform.position);
                PlayAnim(EMonsterAnim.idle);

                // ������ ��ġ�� �̵�
                if (RandomPosition.GetRandomNavPoint(Vector3.zero, mapRange, out randPos))
                {
                    nav.SetDestination(randPos);
                    PlayAnim(EMonsterAnim.walk);
                    
                    isStop = false;
                    while (!isStop)
                    {
                        offset = transform.position - randPos;
                        distance = offset.sqrMagnitude;         // ���Ϳ� ������ ��ġ ������ �Ÿ�

                        if (distance < 1f)
                        {
                            nav.SetDestination(transform.position);
                            PlayAnim(EMonsterAnim.idle);

                            randTime = Random.Range(2f, 6f);
                            yield return new WaitForSeconds(randTime);

                            isStop = true;
                        }

                        yield return null;
                    }
                    
                }
            }

            yield return null;
        }
    }



    // ������ ������ �����ϰ� �ð��� ������ ������ ���ϸ� ���� ����
    IEnumerator ChaseTimeCount()
    {
        isCounting = true;
        takeDamage = false;
        countTime = originCountTime;

        for (int i = 0; i < countTime; i++)
        {
            if (takeDamage)                      // ī��Ʈ ���� ���� �������� �Ծ��ٸ�, ī��Ʈ �ð��� ������Ŵ
            {
                countTime += addCountAmount;
                takeDamage = false;
            }

            yield return waitFor1s;
        }

        if (isChasing)
        {
            isCounting = false;
            StopChase();
        }
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
    #endregion

    #region �Լ�
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

    // ������ ���� �õ�
    protected override void TryStartChase()
    {
        takeDamage = true;

        base.TryStartChase();

        if (!isCounting)                // ���� Ÿ�� ī��Ʈ�� ���� ���� ���� ��
        {
            StartCoroutine(ChaseTimeCount());       // ���� Ÿ�� ī��Ʈ ����
        }
    }


    // ���� ����
    private void StopChase()
    {
        if (isChasing && !isCounting)
        {
            isChasing = false;
            if (isAttacking) IsAttacking = false;

            nav.SetDestination(this.transform.position);

            target = null;

            HideHPBar();

            PlayAnim(EMonsterAnim.idle);
        }
    }
    #endregion
}
