using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

//对象池
public class ObjectPool : SingletonMono<ObjectPool>
{
    private Dictionary<string, Drawer> drawerDic = new();

    /// <summary>
    /// 取出对应对象资源
    /// </summary>
    /// <param name="key">addressable名</param>
    /// <returns></returns>
    public GameObject Pop(string key,int ceiling=10)
    {
        if (!drawerDic.ContainsKey(key))
        {
            InitDrawer(key, ceiling);
        }

        GameObject ans = drawerDic[key].Pop();
        ans.SetActive(true);
        return ans;
    }

    /// <summary>
    /// 放入对应的对象资源
    /// </summary>
    /// <param name="obj">对象实例</param>
    /// <param name="ceiling">抽屉上限</param>
    public void Push(string key,GameObject obj,int ceiling=10)
    {
        if(!drawerDic.ContainsKey(key))
        {
            InitDrawer(key, ceiling);
        }

        drawerDic[key].Push(obj);
    }

    /// <summary>
    /// 清空对象池
    /// </summary>
    public void Clear()
    {
        foreach(Drawer drawer in drawerDic.Values)
        {
            drawer.Clear();
            Destroy(drawer.gameObject);
            AddressableMgr.Instance.Release<GameObject>(drawer.resName);
        }
        drawerDic.Clear();
    }

    public void InitDrawer(string key,int ceiling)
    {
        GameObject obj = new();
        obj.transform.SetParent(this.transform);
        obj.name = key;
        Drawer drawer= obj.AddComponent<Drawer>();
        drawer.ceiling = ceiling;
        drawer.obj= AddressableMgr.Instance.LoadRes<GameObject>(key);
        drawer.resName = key;
        drawerDic.Add(key, drawer);
    }
}

//抽屉类
public class Drawer : MonoBehaviour 
{
    private Stack<GameObject> objStack = new();

    public string resName;
    public int ceiling;//容纳上限
    public GameObject obj;//addressable加载的资源

    //取出一个失活的对象
    public GameObject Pop()
    {
        if (objStack.Count < 1)
        {
            return Instantiate(obj);
        }

        return objStack.Pop();
    }

    //放入一个对象
    public void Push(GameObject obj)
    {
        if (objStack.Count >= ceiling)
        {
            Destroy(obj);
            return;
        }

        obj.SetActive(false);
        obj.transform.SetParent(this.transform);
        objStack.Push(obj);
    }

    public void Clear()
    {
        foreach(GameObject obj in objStack)
        {
            Destroy(obj);
        }

        objStack.Clear();
    }
}

