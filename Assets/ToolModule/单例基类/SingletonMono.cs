using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 有mono单例,用来实现管理器
/// </summary>
public class SingletonMono<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Instance 
    {
        get
        {
            if(instance == null)
            {
                GameObject obj = new GameObject(typeof(T).Name);
                instance = obj.AddComponent<T>();
                GameObject.DontDestroyOnLoad(obj);
            }

            return instance;
        }
    }
}
