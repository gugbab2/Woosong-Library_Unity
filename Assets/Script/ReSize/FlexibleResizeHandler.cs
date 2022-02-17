using System;
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

//리사이즈 리사이즈 지정 이넘 타입
public enum HandlerType
{
    TopRight,
    Right,
    BottomRight,
    Bottom,
    BottomLeft,
    Left,
    TopLeft,
    Top
}

[RequireComponent(typeof(EventTrigger))]
public class FlexibleResizeHandler : MonoBehaviour
{
    //이넘 타입 저장 변수
    public HandlerType Type;
    //지정할 타켓
    public RectTransform Target;
    //사이즈 변경 최솟값
    public Vector2 MinimumDimmensions = new Vector2(50, 50);
    //사이즈 변경 최댓값
    public Vector2 MaximumDimmensions = new Vector2(800, 800);
    //이벤트 트리거
    private EventTrigger _eventTrigger;
    
	void Start ()
	{
	    _eventTrigger = GetComponent<EventTrigger>();
        _eventTrigger.AddEventTrigger(OnDrag, EventTriggerType.Drag);
	}
    //컴퍼넌트에서 컨트롤및 기능을 수행 할 수 있게하는 메서드
    void OnDrag(BaseEventData data)
    {
        PointerEventData ped = (PointerEventData) data;
        //Target.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Target.rect.width + ped.delta.x);
        //Target.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Target.rect.height + ped.delta.y);
        RectTransform.Edge? horizontalEdge = null;
        RectTransform.Edge? verticalEdge = null;

        switch (Type)
        {
            case HandlerType.TopRight:
                horizontalEdge = RectTransform.Edge.Left;
                verticalEdge = RectTransform.Edge.Bottom;
                break;
            case HandlerType.Right:
                horizontalEdge = RectTransform.Edge.Left;
                break;
            case HandlerType.BottomRight:
                horizontalEdge = RectTransform.Edge.Left;
                verticalEdge = RectTransform.Edge.Top;
                break;
            case HandlerType.Bottom:
                verticalEdge = RectTransform.Edge.Top;
                break;
            case HandlerType.BottomLeft:
                horizontalEdge = RectTransform.Edge.Right;
                verticalEdge = RectTransform.Edge.Top;
                break;
            case HandlerType.Left:
                horizontalEdge = RectTransform.Edge.Right;
                break;
            case HandlerType.TopLeft:
                horizontalEdge = RectTransform.Edge.Right;
                verticalEdge = RectTransform.Edge.Bottom;
                break;
            case HandlerType.Top:
                verticalEdge = RectTransform.Edge.Bottom;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
      if (horizontalEdge != null)
        {
            if (horizontalEdge == RectTransform.Edge.Right)
            {
                float newWidth = Mathf.Clamp(Target.sizeDelta.x - ped.delta.x, MinimumDimmensions.x, MaximumDimmensions.x);
                float deltaPosX = -(newWidth - Target.sizeDelta.x) * Target.pivot.x;

                Target.sizeDelta = new Vector2(newWidth, Target.sizeDelta.y);
                Target.anchoredPosition = Target.anchoredPosition + new Vector2(deltaPosX, 0);
            }
            else
            {
                float newWidth = Mathf.Clamp(Target.sizeDelta.x + ped.delta.x, MinimumDimmensions.x, MaximumDimmensions.x);
                float deltaPosX = (newWidth - Target.sizeDelta.x) * Target.pivot.x;

                Target.sizeDelta = new Vector2(newWidth, Target.sizeDelta.y);
                Target.anchoredPosition = Target.anchoredPosition + new Vector2(deltaPosX, 0);
            }
        }
        if (verticalEdge != null)
        {
            if (verticalEdge == RectTransform.Edge.Top)
            {
                float newHeight = Mathf.Clamp(Target.sizeDelta.y - ped.delta.y, MinimumDimmensions.y, MaximumDimmensions.y);
                float deltaPosY = -(newHeight - Target.sizeDelta.y) * Target.pivot.y;

                Target.sizeDelta = new Vector2(Target.sizeDelta.x, newHeight);
                Target.anchoredPosition = Target.anchoredPosition + new Vector2(0, deltaPosY);
            }
            else
            {
                float newHeight = Mathf.Clamp(Target.sizeDelta.y + ped.delta.y, MinimumDimmensions.y, MaximumDimmensions.y);
                float deltaPosY = (newHeight - Target.sizeDelta.y) * Target.pivot.y;

                Target.sizeDelta = new Vector2(Target.sizeDelta.x, newHeight);
                Target.anchoredPosition = Target.anchoredPosition + new Vector2(0, deltaPosY);
            }
        }
    }
}
