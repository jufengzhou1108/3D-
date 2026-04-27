using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieData 
{
    public List<ZombieInfo> zombies;
}

public class ZombieInfo
{
    public int id;
    public string res;
    public string animator;
    public int atk;
    public float moveSpeed;
    public float roundSpeed;
    public int hp;
    public float atkOffst;
}
