using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainTowerObject : MonoBehaviour
{
    public const string DeadEvent = "MainTowerObject_Dead";
    private static MainTowerObject instance;
    public static MainTowerObject Instance => instance;

    private void Awake()
    {
        instance = this;
    }

    public void Attacked(int damage)
    {
        GameModel.Instance.HP-=damage;
        if(GameModel.Instance.HP <= 0)
        {
            TimeManager.Pause();
            Dead();
        }
    }

    private void Dead()
    {
        EventCenter.Instance.EventTrigger(DeadEvent);
    }
}
