/**
 * @brief ·é ¸Å´ÏÀú
 * @author ±è¹Ì¼º
 * @date 22-06-29
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneManager : MonoBehaviour
{
    #region º¯¼ö
    [SerializeField]
    private List<Rune> runes = new List<Rune>();        // ÀüÃ¼ ·éÀÇ ¸®½ºÆ®

    private Rune[] myRunes = new Rune[3];       // ³» ·é
    int runeCount = 0;

    int rand;
    #endregion

    #region ÇÔ¼ö
    // ·£´ýÀ¸·Î ·éÀ» ¹ÝÈ¯
    public Rune GetRandomRune()
    {
        rand = Random.Range(0, runes.Count);

        return runes[rand];
    }

    // ·éÀ» Ãß°¡
    public void AddMyRune(Rune rune)
    {
        if (runeCount > 2) return;

        myRunes[runeCount] = rune;
        runeCount++;
    }
    #endregion
}
