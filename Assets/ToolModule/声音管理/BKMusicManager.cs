using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 背景音乐管理器
/// </summary>
public class BKMusicManager : SingletonMono<BKMusicManager>
{
    private AudioSource musicSource;
    private string beforeMusicName;//用来释放资源

    private void Start()
    {
        EventCenter.Instance.AddListener(SoundModel.SOUND_UPDATE_EVENT,SoundChange);
    }

    private void SoundChange()
    {
        if (musicSource == null)
        {
            return;
        }

        musicSource.volume=SoundModel.Instance.GetMusicVolume();
    }

    /// <summary>
    /// 播放音乐
    /// </summary>
    public void PlayMusic(string name)
    {
        if (musicSource == null)
        {
            musicSource = gameObject.AddComponent<AudioSource>();
        }

        AddressableMgr.Instance.LoadResAsync<AudioClip>(name, (c) =>
        {
            if(beforeMusicName!=null)
            {
                AddressableMgr.Instance.Release<AudioClip>(beforeMusicName);
            }

            musicSource.clip = c;
            musicSource.loop = true;
            musicSource.volume =SoundModel.Instance.GetMusicVolume();
            beforeMusicName = name;
            musicSource.Play();
        });
    }
}
