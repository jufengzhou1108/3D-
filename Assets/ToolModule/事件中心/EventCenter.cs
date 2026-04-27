using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventCenter : Singleton<EventCenter>
{
    public Dictionary<string,UnityAction> eventDic=new Dictionary<string, UnityAction>();

    /// <summary>
    /// 添加监听
    /// </summary>
    /// <param name="eventName">监听事件名</param>
    /// <param name="action">回调函数</param>
    public void AddListener(string eventName, UnityAction action)
    {
        if (!eventDic.ContainsKey(eventName))
        {
            eventDic.Add(eventName, action);
            return;
        }

        eventDic[eventName] += action;
    }

    /// <summary>
    /// 移除监听
    /// </summary>
    /// <param name="eventName">监听事件名</param>
    /// <param name="action">回调函数</param>
    public void RemoveListener(string eventName, UnityAction action)
    {
        if(!eventDic.ContainsKey(eventName))
        {
            return;
        }

        eventDic[eventName] -= action;

        if (eventDic[eventName] == null)
        {
            eventDic.Remove(eventName);
        }
    }

    /// <summary>
    /// 触发事件
    /// </summary>
    /// <param name="eventName">事件名</param>
    public void EventTrigger(string eventName)
    {
        if (!eventDic.ContainsKey(eventName))
        {
            return;
        }

        eventDic[eventName]?.Invoke();
    }
}
