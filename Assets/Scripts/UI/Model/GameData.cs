using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData 
{
    public int id=1;
    public float hp=100f;
    public int allWaveNum=12;
    public int nowWaveNum=0;
    public TowerData[] towers;
}

public class TowerData
{
    public string url;
    public int money;
}
