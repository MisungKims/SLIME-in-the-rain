/**
 * @brief ¡©∏Æ ø¿∫Í¡ß∆Æ
 * @author ±ËπÃº∫
 * @date 22-07-02
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUp : PickUp
{
  
    public override void Get()
    {
        GetMoneyMap.Instance.sumSpeed += 1.5f;

        StatManager.Instance.AddMoveSpeed(1.5f);

        gameObject.SetActive(false);
    }
}
