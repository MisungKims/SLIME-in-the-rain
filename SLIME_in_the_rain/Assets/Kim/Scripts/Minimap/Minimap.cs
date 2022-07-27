using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{

    // https://www.youtube.com/watch?v=oIt9ZAQ_lU0

    #region 싱글톤
    private static Minimap instance = null;
    public static Minimap Instance
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

    [SerializeField]
    Terrain terrain;

    [SerializeField]
    RectTransform scrollViewRectTransform;

    [SerializeField]
    RectTransform contentRectTransform;

    [SerializeField]
    MinimapIcon miniMapIconPrefab;

    Matrix4x4 transformationMatrix;

    Dictionary<MinimapWorldObject, MinimapIcon> miniMapWorldObjectLookup = new Dictionary<MinimapWorldObject, MinimapIcon>();

    [SerializeField]
    private RectTransform range;

    [SerializeField]
    private float mul;

    [SerializeField]
    private float zoom;


    public void Awake()
    {
        if (null == instance)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Update()
    {
        MoveMinimap();
        //UpdateMinimapIcons();
    }

    void MoveMinimap()
    {
        this.transform.localScale = Vector3.one * zoom;

        foreach (var kvp in miniMapWorldObjectLookup)
        {
            var minimapWorldObject = kvp.Key;
            var minimapIcon = kvp.Value;

            Vector3 pos = minimapWorldObject.transform.position;
            pos.y = pos.z;
            pos.z = 0;

            this.transform.localPosition = -pos * mul * zoom;
        }
    }

    // 아이콘의 위치 바꿈
    void UpdateMinimapIcons()
    {
        foreach (var kvp in miniMapWorldObjectLookup)
        {
            var minimapWorldObject = kvp.Key;
            var minimapIcon = kvp.Value;

            var iconPosition = WorldPositionTomapPostion(minimapWorldObject.transform.position);
            minimapIcon.rectTransform.anchoredPosition = iconPosition * mul;
        }
    }

    // 오브젝트의 위치를 가져옴
    Vector2 WorldPositionTomapPostion(Vector3 worldPos)
    {
        var pos = new Vector2(worldPos.x, worldPos.z);

        return pos;
    }

    // 미니맵 아이콘 등록
    public void RegisterMinimapWorldObject(MinimapWorldObject minimapWorldObject)
    {
        var miniMapIcon = Instantiate(miniMapIconPrefab);
        miniMapIcon.transform.SetParent(this.transform);
        miniMapIcon.SetIcon(minimapWorldObject.Icon);
        miniMapIcon.SetColor(minimapWorldObject.IconColor);
        miniMapWorldObjectLookup[minimapWorldObject] = miniMapIcon;
    }
}
