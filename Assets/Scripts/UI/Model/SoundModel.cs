using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundModel : Singleton<SoundModel>
{
    private MusicData musicData;

    private const string JSON_NAME = "musicData";
    public const string SOUND_UPDATE_EVENT = "BKMusicManager_SoundUpdate";

    public SoundModel()
    {
        GetData();
    }

    /// <summary>
    /// 音乐音量数据
    /// </summary>
    public float MusicVolume
    {
        get
        {
            return musicData.musicVolume;
        }
        set
        {
            musicData.musicVolume = value;
            UpdateMusic();
        }
    }

    /// <summary>
    /// 是否开启音乐
    /// </summary>
    public bool HasMusic
    {
        get
        {
            return musicData.hasMusic;
        }
        set
        {
            musicData.hasMusic = value;
            UpdateMusic();
        }
    }

    /// <summary>
    /// 音效音量
    /// </summary>
    public float SoundVolume
    {
        get
        {
            return musicData.soundVolume;
        }
        set
        {
            musicData.soundVolume = value;
            UpdateSound();
        }
    }

    /// <summary>
    /// 是否开启音效
    /// </summary>
    public bool HasSound
    {
        get
        {
            return musicData.hasSound;
        }
        set
        {
            musicData.hasSound = value;
            UpdateSound();
        }
    }

    /// <summary>
    /// 总音量大小
    /// </summary>
    public float TotalVolume
    {
        get
        {
            return musicData.totalVolume;
        }
        set
        {
            musicData.totalVolume = value;
            UpdateMusic();
            UpdateSound();
        }
    }

    /// <summary>
    /// 是否开启声音
    /// </summary>
    public bool HasTotal
    {
        get
        {
            return musicData.hasTotal;
        }
        set
        {
            musicData.hasTotal = value;
            UpdateSound();
            UpdateMusic();
        }
    }

    /// <summary>
    /// 更新音乐播放数据
    /// </summary>
    private void UpdateMusic()
    {
        EventCenter.Instance.EventTrigger(SOUND_UPDATE_EVENT);
    }

    /// <summary>
    /// 触发音效更新事件，更新音效
    /// </summary>
    public void UpdateSound()
    {
        EventCenter.Instance.EventTrigger(SOUND_UPDATE_EVENT);
    }

    /// <summary>
    /// 获取声音数据
    /// </summary>
    /// <returns></returns>
    private void GetData()
    {
        musicData = JsonTool.LoadJson<MusicData>(JSON_NAME);
    }

    /// <summary>
    /// 保存声音数据
    /// </summary>
    public void SaveData()
    {
        JsonTool.SaveData(musicData, JSON_NAME);
    }

    /// <summary>
    /// 获取音乐播放音量
    /// </summary>
    /// <returns></returns>
    public float GetMusicVolume()
    {
        if (!musicData.hasTotal || !musicData.hasMusic)
        {
            return 0f;
        }
        return musicData.musicVolume * musicData.totalVolume;
    }

    /// <summary>
    /// 获取音效播放音量
    /// </summary>
    /// <returns></returns>
    public float GetSoundVolume()
    {
        if (!musicData.hasTotal || !musicData.hasSound)
        {
            return 0f;
        }
        return musicData.soundVolume * musicData.totalVolume;
    }
}
