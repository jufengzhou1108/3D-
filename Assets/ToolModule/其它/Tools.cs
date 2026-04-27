using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Tools 
{
    //批量执行DontDestroyOnLoad函数
    public static void DontDestoryObjects(Object[] objects)
    {
        foreach (Object obj in objects)
        {
            GameObject.DontDestroyOnLoad(obj);
        }
    }
}
