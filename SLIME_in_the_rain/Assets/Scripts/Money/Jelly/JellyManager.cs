using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyManager : MonoBehaviour
{
    #region º¯¼ö
    #region ½Ì±ÛÅæ
    private static JellyManager instance = null;
    public static JellyManager Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }
    #endregion

    private int jellyCount = 0;
    public int JellyCount { get { return jellyCount; } set { jellyCount = value; } }
    #endregion

    #region À¯´ÏÆ¼ ÇÔ¼ö
    void Awake()
    {
        if (null == instance)
        {
            instance = this;

            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    #endregion
}
