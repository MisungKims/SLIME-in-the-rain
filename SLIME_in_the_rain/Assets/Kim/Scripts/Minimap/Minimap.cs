using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Minimap : MonoBehaviour
{
    #region 변수
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

    private bool isZoomIn = true;

    [SerializeField]
    private GameObject slimeIconZoomIn;    // 축소 상태에서 맵 중간에 놓일 슬라임의 아이콘

    [SerializeField]
    private MinimapIcon miniMapIconPrefab;         // 생성할 아이콘 프리팹

    private Dictionary<MinimapWorldObject, MinimapIcon> miniMapWorldObjectLookup = new Dictionary<MinimapWorldObject, MinimapIcon>();


    [SerializeField]
    private float mul = 8f;

   [SerializeField]
    private float zoom = 2.3f;     // 줌인 할 배율

    private MinimapWorldObject slimeObj;
    private Vector3 slimePos;

    [SerializeField]
    private Mask mask;
   [SerializeField]
    private RectTransform maskTransform;
    [SerializeField]
    private RectTransform mapTransform;
    [SerializeField]
    private RectTransform zoomInTransform;      // 줌인 했을 때의 Mask 크기
    [SerializeField]
    private RectTransform zoomOutTransform;      // 줌아웃 했을 때의 Mask 크기


    // 캐싱
    private MinimapWorldObject minimapWorldObject;
    private MinimapIcon minimapIcon;
    private Vector2 iconPosition;

    private MinimapIcon newIcon;
    #endregion

    #region 유니티 함수
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

        ZoomIn();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            if (isZoomIn) ZoomOut();
            else ZoomIn();
        }

        MoveMinimap();
        UpdateMinimapIcons();
    }
    #endregion

    #region 함수
    // 축소
    void ZoomIn()
    {
        isZoomIn = true;
        slimeIconZoomIn.SetActive(true);
        mask.enabled = true;

        maskTransform.anchoredPosition = zoomInTransform.anchoredPosition;
        maskTransform.sizeDelta = zoomInTransform.sizeDelta;
        mapTransform.anchoredPosition = Vector2.zero;
    }

    // 확대
    void ZoomOut()
    {
        isZoomIn = false;
        slimeIconZoomIn.SetActive(false);
        mask.enabled = false;

        maskTransform.anchoredPosition = zoomOutTransform.anchoredPosition;
        maskTransform.sizeDelta = zoomOutTransform.sizeDelta;
        mapTransform.anchoredPosition = Vector2.zero;
    }

    // 축소 상태일 때 미니맵 자체를 움직임
    void MoveMinimap()
    {
        if (isZoomIn)
        {
            this.transform.localScale = Vector3.one * zoom;

            slimePos = slimeObj.transform.position;
            slimePos.y = slimePos.z;
            slimePos.z = 0;

            this.transform.localPosition = -slimePos * mul * zoom;
        }
        else
        {
            this.transform.localScale = Vector3.one;
        } 
    }

    // 아이콘의 위치 바꿈
    void UpdateMinimapIcons()
    {
        foreach (var kvp in miniMapWorldObjectLookup)
        {
            minimapWorldObject = kvp.Key;

            if (isZoomIn)       // 축소상태 일때에는 슬라임의 아이콘이 아닌 것만 위치 변경
            {
                if (!minimapWorldObject.Equals(slimeObj))
                {
                    minimapIcon = kvp.Value;

                    iconPosition = WorldPositionTomapPostion(minimapWorldObject.transform.position);
                    minimapIcon.rectTransform.anchoredPosition = iconPosition * mul;
                }
            }
            else
            {
                minimapIcon = kvp.Value;

                iconPosition = WorldPositionTomapPostion(minimapWorldObject.transform.position);
                minimapIcon.rectTransform.anchoredPosition = iconPosition * mul;
            }
        }
    }

    // 오브젝트의 위치를 가져옴
    Vector2 WorldPositionTomapPostion(Vector3 worldPos)
    {
        var pos = new Vector2(worldPos.x, worldPos.z);

        return pos;
    }

    // 미니맵 아이콘 등록
    public void RegisterMinimapWorldObject(MinimapWorldObject obj)
    {
        //MinimapIcon miniMapIcon = Instantiate(miniMapIconPrefab);
        //newIcon = ObjectPoolingManager.Instance.Get(EObjectFlag.minimapIcon).GetComponent<MinimapIcon>();
        newIcon = Instantiate(miniMapIconPrefab);
        newIcon.transform.SetParent(this.transform);
        newIcon.SetIcon(obj.Icon);
        newIcon.SetColor(obj.IconColor);
        miniMapWorldObjectLookup[obj] = newIcon;

        if (obj.CompareTag("Slime")) slimeObj = obj;
    }
    #endregion
}
