using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneWeapon : Rune
{
    #region 변수
    protected EWeaponType weaponType1;
    protected EWeaponType weaponType2;

    private Slime slime;
    #endregion

    #region 유니티 함수
    private void Awake()
    {
        slime = Slime.Instance;
    }
    #endregion

    #region 함수
    public override void Use()
    {
        UseWeaponRune();

        throw new System.NotImplementedException();
    }

    protected virtual bool UseWeaponRune()
    {
        // 현재 무기가 type1 혹은 type2 와 같다면 사용할 수 있도록
        EWeaponType currentWeapon = slime.currentWeapon.weaponType;
        if (currentWeapon.Equals(weaponType1) || currentWeapon.Equals(weaponType2))
        {
            return true;
        }
        else return false;
    }
    #endregion

}
