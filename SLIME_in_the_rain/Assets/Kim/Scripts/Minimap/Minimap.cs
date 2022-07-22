using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{

    // https://www.youtube.com/watch?v=oIt9ZAQ_lU0

    #region ΩÃ±€≈Ê
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

    public void Awake()
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

    public void RegisterMinimapWorldObject(MinimapWorldObject minimapWorldObject)
    {
        var miniMapIcon = Instantiate(miniMapIconPrefab);
        miniMapIcon.SetIcon(minimapWorldObject.Icon);
        miniMapIcon.SetColor(minimapWorldObject.IconColor);
        miniMapIcon.SetText(minimapWorldObject.text);
        miniMapIcon.SetTextSize(minimapWorldObject.textSize);
        miniMapWorldObjectLookup[minimapWorldObject] = miniMapIcon;
    }

    void CalcuateTransformationMatrix()
    {
        var miniMapDimensions = contentRectTransform.rect.size;
        var terrainDimensions = new Vector2(terrain.terrainData.size.x, terrain.terrainData.size.z);

        var scaleRatio = miniMapDimensions / terrainDimensions;
        var transition = -miniMapDimensions / 2;

        transformationMatrix = Matrix4x4.TRS(transition, Quaternion.identity, scaleRatio);

    }
}
