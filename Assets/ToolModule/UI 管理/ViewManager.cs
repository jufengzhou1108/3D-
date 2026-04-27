using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 视图管理器,用来显示和隐藏视图
/// </summary>
public class ViewManager:Singleton<ViewManager>
{
    private Dictionary<string,GameObject> viewDic=new Dictionary<string,GameObject>();
    private GameObject canvas;

    /// <summary>
    ///  初始化UI，加载Canvas，UIcamera，EventSystem
    /// </summary>
    private void Init()
    {
        GameObject canvasObj = AddressableMgr.Instance.LoadRes<GameObject>("Canvas");
        GameObject cameraObj = AddressableMgr.Instance.LoadRes<GameObject>("UICamera");
        GameObject eventObj = AddressableMgr.Instance.LoadRes<GameObject>("EventSystem");

        canvas=GameObject.Instantiate(canvasObj);
        GameObject camera= GameObject.Instantiate(cameraObj);
        GameObject eventSystem = GameObject.Instantiate(eventObj);

        canvas.GetComponent<Canvas>().worldCamera = camera.GetComponent<Camera>();
    }

    /// <summary>
    /// 显示视图
    /// </summary>
    /// <typeparam name="T">对应的视图类</typeparam>
    public void Show<T>() where T : class
    {
        if (canvas == null)
        {
            Init();
        }

        string name=typeof(T).Name;

        //如果已加载，则忽略
        if (viewDic.ContainsKey(name))
        {
            return;
        }

        AddressableMgr.Instance.LoadResAsync<GameObject>(name, (obj) =>
        {
            GameObject view = GameObject.Instantiate(obj);
            view.transform.SetParent(canvas.transform, false);
            viewDic[name] = view;

            //触发展示事件
            EventCenter.Instance.EventTrigger(GetShowEvent<T>());
        });
    }

    /// <summary>
    /// 隐藏指定视图
    /// </summary>
    /// <typeparam name="T">对应的视图类</typeparam>
    public void Hide<T>() where T : class
    {
        string name= typeof(T).Name;

        //没有显示，无需隐藏
        if (!viewDic.ContainsKey(name))
        {
            return ;
        }

        //销毁视图
        GameObject.Destroy(viewDic[name]);
        viewDic.Remove(name);
        EventCenter.Instance.EventTrigger(GetHideEvent<T>());
    }

    /// <summary>
    /// 获取显示视图的事件名 
    /// </summary>
    /// <typeparam name="T">视图名</typeparam>
    /// <returns></returns>
    public static string GetShowEvent<T>() where T : class
    {
        return typeof(T).Name+"_Show";
    }

    /// <summary>
    /// 获取隐藏视图的事件名 
    /// </summary>
    /// <typeparam name="T">视图名</typeparam>
    /// <returns></returns>
    public static string GetHideEvent<T>() where T : class
    {
        return typeof(T).Name + "_Hide";
    }



    private string content;
    private UnityAction action;
    //显示提示
    public void ShowTip(string content,UnityAction action=null)
    {
        this.content = content;
        this.action = action;
        EventCenter.Instance.AddListener(GetShowEvent<TipView>(),ShowTipOver);
        Show<TipView>();
    }

    public void ShowTipOver()
    {
        viewDic[typeof(TipView).Name].GetComponent<TipView>().Init(content,action);
    }

    //清除所有视图
    public void Clear()
    {
        foreach(string viewName in viewDic.Keys)
        {
            GameObject.Destroy(viewDic[viewName]);
            AddressableMgr.Instance.Release<GameObject>(viewName);
        }
        viewDic.Clear();
    }
}
