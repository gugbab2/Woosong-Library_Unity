using UnityEngine.Events;
using UnityEngine.EventSystems;

public static class FlexibleExtensions
{
    //리사이즈 시키는 이벤트 트리커
    public static void AddEventTrigger(this EventTrigger eventTrigger, UnityAction<BaseEventData> action,
        EventTriggerType triggerType)
    {
        EventTrigger.TriggerEvent trigger = new EventTrigger.TriggerEvent();
        trigger.AddListener(action);

        EventTrigger.Entry entry = new EventTrigger.Entry { callback = trigger, eventID = triggerType };
        eventTrigger.triggers.Add(entry);
    }
}