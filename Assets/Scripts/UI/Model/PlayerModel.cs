using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 用户数据管理类
/// </summary>
public class PlayerModel:Singleton<PlayerModel>
{
    private PlayerData playerData;
    private string DAtA_NAME = "player";

    public const string UPDATE_EVENT = "PlayerModel_Update";

    private void GetData()
    {
        playerData = JsonTool.LoadJson<PlayerData>(DAtA_NAME);
    }

    public void SaveData()
    {
        JsonTool.SaveData(playerData, DAtA_NAME);
    } 

    //完成数据更新时的处理
    private void Update()
    {
        EventCenter.Instance.EventTrigger(UPDATE_EVENT);
        JsonTool.SaveData(playerData, DAtA_NAME);
    }

    public int Money
    {
        get
        {
            if(playerData == null)
            {
                GetData();
            }
            return playerData.money;
        }
        set
        {
            playerData.money = value;
            Update();
        }
    }

    public bool HasRole(string name)
    {
        switch(name)
        {
            case "engineer":
                return playerData.roles["engineer"];
            case "gunner":
                return playerData.roles["gunner"];
            case "infantry":
                return playerData.roles["infantry"];
            case "officer":
                return playerData.roles["officer"];
            case "sniper":
                return playerData.roles["sniper"];
            default:
                Debug.Log("未找到对应角色");
                return false;
        }
    }

    public void SetRole(string name, bool value)
    {
        switch (name)
        {
            case "engineer":
                playerData.roles["engineer"]=value;
                break;
            case "gunner":
                playerData.roles["gunner"] = value;
                break;
            case "infantry":
                playerData.roles["infantry"] = value;
                break;
            case "officer":
                playerData.roles["officer"] = value;
                break;
            case "sniper":
                playerData.roles["sniper"] = value;
                break;
            default:
                Debug.Log("未找到对应角色");
                return;
        }
        Update();
    }
}
