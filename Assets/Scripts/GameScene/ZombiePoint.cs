using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombiePoint : MonoBehaviour
{
    public const string EndEvent = "ZombiePoint_EndEvent";

    public int zombieNumOneWave;
    public int[] zombieIDs;
    private int nowId;
    private int nowNum;

    public float createOffsetTime;
    public float delayTime;
    public float firstDelayTime;

    void Start()
    {
        Invoke("CreateWave", firstDelayTime);
    }

    private void CreateWave()
    {
        nowId=zombieIDs[Random.Range(0,zombieIDs.Length)];
        nowNum = zombieNumOneWave;

        CreateZombie();

        GameModel.Instance.NowWaveNum++;
    }

    private void CreateZombie()
    {
        ZombieInfo info = ZombieModel.Instance.GetZombie(nowId);
        AddressableMgr.Instance.LoadResAsync<GameObject>(info.res,(obj) =>
        {
            GameObject newObj=Instantiate(obj,this.transform.position,Quaternion.identity);
            newObj.GetComponent<ZombieObject>().InitInfo(info);
        });

        nowNum--;
        if (nowNum == 0)
        {
            if (GameModel.Instance.NowWaveNum == GameModel.Instance.AllWaveNum)
            {
                EventCenter.Instance.EventTrigger(EndEvent);
                return;
            }

            Invoke("CreateWave", delayTime);

            return;
        }

        Invoke("CreateZombie", createOffsetTime);
    }
}
