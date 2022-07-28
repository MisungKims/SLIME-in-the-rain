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
    private RectTransform slimeIconRect;

    [SerializeField]
    private MinimapIcon miniMapIconPrefab;         // 생성할 아이콘 프리팹

    private Dictionary<MinimapWorldObject, MinimapIcon> miniMapWorldObjectLookup = new Dictionary<MinimapWorldObject, MinimapIcon>();


    [SerializeField]
    private float mul = 8f;

   [SerializeField]
    private float zoom = 2.3f;     // 줌인 할 배율

    [SerializeField]
    private float zoomInRange;
    [SerializeField]
    private float zoomOutRange;

    // 슬라임이 미니맵의 범위를 벗어났는지?
    private bool isOutRangeX;
    private bool isOutRangeY;

    private MinimapWorldObject slimeObj;
    private GameObject slimeIconZoomOut;
    private Vector2 slimePos;

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

        slimeIconRect = slimeIconZoomIn.GetComponent<RectTransform>();
        slimeIconRect.anchoredPosition = Vector2.zero;
        slimeIconRect.localScale *= zoom;

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
        if(slimeIconZoomOut) slimeIconZoomOut.SetActive(false);

        maskTransform.anchoredPosition = zoomInTransform.anchoredPosition;
        maskTransform.sizeDelta = zoomInTransform.sizeDelta;
        mapTransform.anchoredPosition = Vector2.zero;
    }

    // 확대
    void ZoomOut()
    {
        isZoomIn = false;
        slimeIconZoomIn.SetActive(false);
        if(slimeIconZoomOut) slimeIconZoomOut.SetActive(true);

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

           slimePos = IsOutRange(WorldPositionTomapPostion(slimeObj.transform.position));

            this.transform.localPosition = -slimePos * mul * zoom;

            if (!isOutRangeX && !isOutRangeY)
            {
                slimeIconZoomIn.SetActive(true);
                slimeIconZoomOut.SetActive(false);

                slimeIconRect.anchoredPosition = Vector2.zero;
            } 
        }
        else
        {
            this.transform.localScale = Vector3.one;
        } 
    }

    Vector2 IsOutRange(Vector2 pos)
    {
        // 아이콘이 범위를 벗어났을 경우
        if (pos.x < zoomInRange * -1)
        {
            pos.x = zoomInRange * -1.01f;
            isOutRangeX = true;
        }
        else if (pos.x > zoomInRange)
        {
            pos.x = zoomInRange * 1.01f;
            isOutRangeX = true;
        }
        else isOutRangeX = false;

        if (pos.y < zoomInRange * -1)
        {
            pos.y = zoomInRange * -1.01f;
            isOutRangeY = true;
        }
        else if (pos.y > zoomInRange)
        {
            pos.y = zoomInRange * 1.01f;
            isOutRangeY = true;
        }
        else isOutRangeY = false;

        return pos;
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
                else
                {
                    if (isOutRangeX || isOutRangeY)
                    {
                        slimeIconZoomIn.SetActive(false);
                        slimeIconZoomOut.SetActive(true);

                        minimapIcon = kvp.Value;

                        iconPosition = WorldPositionTomapPostion(minimapWorldObject.transform.position);
                        minimapIcon.rectTransform.anchoredPosition = iconPosition * mul;
                    }
                }
            }
            else
            {
                minimapIcon = kvp.Value;
                iconPosition = WorldPositionTomapPostion(minimapWorldObject.transform.position);

                // 아이콘이 범위를 벗어나지 못하도록
                if (iconPosition.x < zoomOutRange * -1) iconPosition.x = zoomOutRange * -1;
                else if (iconPosition.x > zoomOutRange) iconPosition.x = zoomOutRange;

                if (iconPosition.y < zoomOutRange * -1) iconPosition.y = zoomOutRange * -1;
                else if (iconPosition.y > zoomOutRange) iconPosition.y = zoomOutRange;

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
        newIcon = ObjectPoolingManager.Instance.Get(EObjectFlag.minimapIcon).GetComponent<MinimapIcon>();
        //newIcon = Instantiate(miniMapIconPrefab);
        newIcon.transform.SetParent(this.transform);
        newIcon.SetIcon(obj.Icon);
        newIcon.SetColor(obj.IconColor);
        miniMapWorldObjectLookup[obj] = newIcon;

        if (obj.CompareTag("Slime"))
        {
            slimeObj = obj;
            slimeIconZoomOut = newIcon.gameObject;
        }
    }
    #endregion
}
