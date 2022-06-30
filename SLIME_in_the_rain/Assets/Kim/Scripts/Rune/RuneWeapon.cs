using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneWeapon : Rune, IWeaponRune
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
    public virtual bool Use(Weapon weapon)
    {
        if (UseWeaponRune(weapon))          // 룬이 발동할 수 있는지 판별
        {
            weapon.IsHaveRune = true;       // 무기가 룬의 특성을 사용할 수 있도록
            return true;
        }
        else return false;
    }

    // 이 룬이 발동하는 무기인지?
    protected bool UseWeaponRune(Weapon weapon)
    {
        // 무기가 type1 혹은 type2 와 같다면 사용할 수 있도록
        EWeaponType currentWeapon = weapon.weaponType;
        if (currentWeapon.Equals(weaponType1) || currentWeapon.Equals(weaponType2))
        {
            Use(slime.currentWeapon);
            return true;
        }
        else return false;
    }
    #endregion

}
