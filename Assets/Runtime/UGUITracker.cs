using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UGUITracker : MonoBehaviour
{
    private GamePerfSDK gamePerfSDK;
    private IEnumerator ClickTrackEnumerator = null;

    private int curTouchCount = 0;
    private GameObject curPressGameObject = null;      // 当前点击的GameObject

    private void Awake()
    {
        gamePerfSDK = GetComponent<GamePerfSDK>();

        ClickTrackEnumerator = ClickTrack();
        StartCoroutine(ClickTrackEnumerator);
    }

    private void OnDestroy()
    {
        if (ClickTrackEnumerator != null)
        {
            StopCoroutine(ClickTrackEnumerator);
            ClickTrackEnumerator = null;
        }
    }

    private System.Collections.IEnumerator ClickTrack()
    {
        while (true)
        {
            if (IsPressDown())
            {
                Vector2 pos = GetPressPos();
                Touch touch = new Touch { position = pos };
                PointerEventData pointerEventData = MockUpPointerInputModule.GetPointerEventData(touch);
                if (pointerEventData.pointerPress != null)
                {
                    curPressGameObject = pointerEventData.pointerPress;
                    Selectable selectable = curPressGameObject.GetComponent<Selectable>();
                    Debug.Log("GamePerf Debug:" + GetGameObjectPath(curPressGameObject));
#if UNITY_ANDROID
                    gamePerfSDK.UserClickTrack(GetGameObjectPath(curPressGameObject));
#endif
                }
            }
            else
            {
                curPressGameObject = null;
            }

            curTouchCount = Input.touchCount;
            yield return null;
        }
    }

    private bool IsPressDown()
    {
        if (Input.GetMouseButtonDown(0))
            return true;
        if (Input.touchCount == 1 && curTouchCount == 0)
            return true;
        return false;
    }

    private Vector2 GetPressPos()
    {
        // 经过测试发现无论是否开启触碰模拟鼠标事件，触摸将会导致mousePosition被改变
        return Input.mousePosition;
    }

    private string GetGameObjectPath(GameObject obj)
    {
        if (obj == null) return "null";
        string path = "/" + obj.name;
        Transform parentTransform = obj.transform.parent;
        while (parentTransform != null)
        {
            path = "/" + parentTransform.name + path;
            parentTransform = parentTransform.parent;
        }
        return path;
    }

}
