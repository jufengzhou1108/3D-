using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;


/// <summary>
/// Addressable相关的管理类
/// </summary>
public class AddressableMgr :Singleton<AddressableMgr>
{ 
    //资源权柄字典
    private Dictionary<string,AsyncOperationHandle> handleDic=new Dictionary<string, AsyncOperationHandle>();
    //资源计数字典
    private Dictionary<string, int> numDic = new Dictionary<string, int>();
    //资源加载任务字典（用来处理竞态）
    private Dictionary<string, LoadTask> taskDic = new();

    /// <summary>
    /// 异步加载可寻址资源
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name">资源名</param>
    /// <param name="action">资源加载的回调函数</param>
    public async void LoadResAsync<T>(string name, UnityAction<T> action) where T : UnityEngine.Object
    { 
        string key=name+"_"+typeof(T).Name;
        if(handleDic.ContainsKey(key))
        {
            numDic[key]++;
            action?.Invoke(handleDic[key].Result as T);
            return;
        }

        //如果已经创建了任务则只需要添加回调
        if (taskDic.ContainsKey(key))
        {
            (taskDic[key] as LoadTask<T>).num++;
            (taskDic[key] as LoadTask<T>).action+=action;

            return;
        }

        taskDic.Add(key,new LoadTask<T>());
        LoadTask<T> task = taskDic[key] as LoadTask<T>;
        task.num++;
        task.action += action;
        task.handle = Addressables.LoadAssetAsync<T>(name);
        await task.handle;

        if (task.handle.Status != AsyncOperationStatus.Succeeded)
        {
            Debug.Log("资源加载失败: "+name);

            //加载失败删除加载任务
            taskDic.Remove(key);
            
            return;
        }

        handleDic.Add(key, task.handle);
        numDic.Add(key, task.num);
        task.action?.Invoke(task.handle.Result);
        taskDic.Remove(key);
    }

    /// <summary>
    /// 同步加载可寻址资源
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name">资源名</param>
    /// <returns></returns>
    public T LoadRes<T>(string name) where T : UnityEngine.Object
    {
        string key = name + "_" + typeof(T).Name;
        if (handleDic.ContainsKey(key))
        {
            numDic[key]++;
            return handleDic[key].Result as T;
        }

        AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(name);

        handle.WaitForCompletion();

        if (handle.Status != AsyncOperationStatus.Succeeded)
        {
            Debug.Log("资源加载失败: " + name);
            return null;
        }
        
        handleDic.Add(key, handle);
        numDic.Add(key, 1);
        return handle.Result;
    }

    /// <summary>
    /// 为lua开发提供的同步加载可寻址资源函数
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="type">资源类型</param>
    /// <param name="name">资源名</param>
    /// <returns></returns>
    public T LoadRes<T>(T type,string name) where T : UnityEngine.Object
    {
        return LoadRes<T>(name);
    }

    //释放资源
    public void Release<T>(string name) where T : UnityEngine.Object
    {
        string key= name + "_" + typeof(T).Name;
        if (!handleDic.ContainsKey(key))
        {
            if (!taskDic.ContainsKey(key))
            {
                return;
            }

            (taskDic[key] as LoadTask<T>).num--;
            (taskDic[key] as LoadTask<T>).action += (t) =>
            {
                if((taskDic[key] as LoadTask<T>).num <= 0)
                {
                    handleDic[key].Release();
                    handleDic.Remove(key);
                    numDic.Remove(key);
                }
            };
            return;
        }

        numDic[key]--;
        if (numDic[key] == 0)
        {
            handleDic[key].Release();
            handleDic.Remove(key);
            numDic.Remove(key);
        }
    }
}

//用于管理加载任务的基类
public abstract class LoadTask 
{
}

//加载任务类
public class LoadTask<T>:LoadTask where T : UnityEngine.Object
{
    public int num;
    public UnityAction<T> action;
    public AsyncOperationHandle<T> handle;
}
