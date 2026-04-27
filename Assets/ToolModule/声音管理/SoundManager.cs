using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    private string SoundPreferbName = "Sound";
    private LinkedList<AudioSource> playingList = new();

    public SoundManager()
    {
        DetectPlaying();
    }

    //将播放完了的音效放回对象池
    private async void DetectPlaying()
    {
        while (true)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(1));

            if (playingList.Count == 0)
            {
                continue;
            }

            LinkedListNode<AudioSource> node = playingList.First;
            while (node != null)
            {
                if (!node.Value.isPlaying)
                {
                    LinkedListNode<AudioSource> deleNode = node;
                    ObjectPool.Instance.Push("Sound",deleNode.Value.gameObject);
                    playingList.Remove(deleNode);
                }

                node = node.Next;
            }
        }
    }

    public void PlaySound(string name,Vector3 position=default(Vector3))
    {
        return;

        GameObject obj = ObjectPool.Instance.Pop(SoundPreferbName);
        AudioSource audioSource=obj.GetComponent<AudioSource>();
        AddressableMgr.Instance.LoadResAsync<AudioClip>(name, (clip) =>
        {
            obj.transform.position= position;
            audioSource.clip = clip;
            audioSource.volume = SoundModel.Instance.GetSoundVolume();
            audioSource.Play();
            playingList.AddLast(audioSource);
        });
    }
}
