using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModel : Singleton<GameModel>
{
    private GameData gameData;
    public const string UPDATE_EVENT = "GameModel_Update";
    public GameModel()
    {
        gameData = JsonTool.LoadJson<GameData>("gameData");
    }

    public float HP
    {
        get => gameData.hp;
        set
        {
            gameData.hp=Mathf.Clamp(value,0,100);
            UpDate();
        }
    }

    public int Id
    {
        get=>gameData.id;
        set=>gameData.id=value;
    }

    public int AllWaveNum
    {
        get => gameData.allWaveNum;
    }

    public int NowWaveNum
    {
        get=>gameData.nowWaveNum;
        set
        {
            gameData.nowWaveNum=value;
            UpDate();
        }
    }

    public TowerData[] Towers=> gameData.towers;

    private void UpDate()
    {
        EventCenter.Instance.EventTrigger(UPDATE_EVENT);
    }

    public void Init()
    {
        HP = 100;
        NowWaveNum = 0;
    }
}
