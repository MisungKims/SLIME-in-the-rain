using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoveryHP : MonoBehaviour
{
    #region 변수
    private Slime slime;
    private Outline outline;

    private StatManager statManager;

    // 슬라임 감지에 필요한 변수
    private float distance;
    Vector3 offset;
    #endregion

    #region 유니티 함수
    private void Awake()
    {
        StartCoroutine(DetectSlime());
    }


    #endregion

    #region 코루틴
    /// <summary>
    /// 슬라임 탐지 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator DetectSlime()
    {
        outline = GetComponent<Outline>();
        slime = Slime.Instance;
        statManager = StatManager.Instance;

        // 슬라임과의 거리를 탐지
        while (true)
        {
            offset = slime.transform.position - transform.position;
            distance = offset.sqrMagnitude;                             // 젤리와 슬라임 사이의 거리

            if (distance < 2f)
            {
                if (!outline.enabled) outline.enabled = true;

                if (Input.GetKeyDown(KeyCode.G))
                {
                    StartCoroutine(Recovery());
                    Debug.Log("G");
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
        while (statManager.myStats.HP < statManager.myStats.maxHP)
        {
            statManager.AddHP(0.1f);

            yield return new WaitForSeconds(0.3f);
        }

        statManager.myStats.HP = statManager.myStats.maxHP;
    }
    #endregion
}
