/**
 * @brief 슬라임 오브젝트
 * @author 김미성
 * @date 22-06-29
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Rune : MonoBehaviour
{
    #region 변수
    // 룬의 이름
    [SerializeField]
    private string runeName;
    public string RuneName { get { return runeName; } }

    // 룬의 설명
    [SerializeField]
    private string runeDescription;
    public string RuneDescription { get { return runeDescription; } }

    // 룬의 이미지
    [SerializeField]
    private Sprite runeSprite;
    public Sprite RuneSprite { get { return runeSprite; } }
    #endregion

    #region 함수
    public abstract void Use();     // 룬을 사용
    #endregion
}
