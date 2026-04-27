using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public int money=0;
    public Dictionary<string,bool> roles=new Dictionary<string, bool> { { "engineer",false}, { "gunner", false }, { "infantry", false }, { "officer", false }, { "sniper", false } }; 
}
