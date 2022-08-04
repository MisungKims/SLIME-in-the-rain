using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoveryHP : MonoBehaviour
{
    #region 변수
    private Slime slime;
    private Outline outline;

    [SerializeField]
    private float speed = 0.08f;        // HP 회복 속도

    private bool isUsed = false;

    // 슬라임 감지에 필요한 변수
    private float distance;
    Vector3 offset;

    // 캐싱
    private StatManager statManager;
    private WaitForSeconds waitForSeconds;
    #endregion

    #region 유니티 함수
    private void Awake()
    {
        waitForSeconds = new WaitForSeconds(speed);
    }

    private void OnEnable()
    {
        isUsed = false;

        StartCoroutine(DetectSlime());
    }


    #endregion

    #region 코루틴
    // 슬라임 탐지 코루틴
    IEnumerator DetectSlime()
    {
        outline = GetComponent<Outline>();
        slime = Slime.Instance;
        statManager = StatManager.Instance;

        // 슬라임과의 거리를 탐지
        while (!isUsed)
        {
            offset = slime.transform.position - transform.position;
            distance = offset.sqrMagnitude;                             // 젤리와 슬라임 사이의 거리

            if (distance < 2f)
            {
                if (!outline.enabled) outline.enabled = true;           // 거리가 가까울 때 외곽선 표시

                if (Input.GetKeyDown(KeyCode.G))                // G키를 누르면 HP 회복
                {
                    StartCoroutine(Recovery());
                }
            }
            else
            {
                if (outline.enabled) outline.enabled = false;
            }

            yield return null;
        }
    }

    // 최대 체력까지 회복
    IEnumerator Recovery()
    {
        isUsed = true;
        outline.enabled = false;

        while (statManager.myStats.HP < statManager.myStats.maxHP)
        {
            statManager.AddHP(1f);

            yield return waitForSeconds;
        }

        statManager.myStats.HP = statManager.myStats.maxHP;
    }
    #endregion
}
