/**
 * @brief 오크 보스
 * @author 김미성
 * @date 22-07-12
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orc : Boss
{
    private float stunTime = 2f;        // 슬라임이 스턴할 시간

    // 슬라임에게 데미지를 입힘
    public override void DamageSlime(int atkType)
    {
        base.DamageSlime(atkType);
        Debug.Log(atkType);

       if(atkType == 1) slime.Stun(stunTime);       // 두번째 공격은 스턴
    }
}
