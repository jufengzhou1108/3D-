using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieModel : Singleton<ZombieModel>
{
    private ZombieData data;
    private string jsonName = "zombies";

    public ZombieModel()
    {
        data = JsonTool.LoadJson<ZombieData>(jsonName);
    }

    public ZombieInfo GetZombie(int id)
    {
        return data.zombies[id - 1];
    }
}
