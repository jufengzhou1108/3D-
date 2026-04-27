using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 摄像机动画控制器脚本
/// </summary>
public class CameraAnimator : MonoBehaviour
{
    private Animator animator;
    private UnityAction action;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    //左转
    public void TrunLeft(UnityAction action)
    {
        animator.SetTrigger("left");
        this.action = action;
    }

    //右转
    public void TrunRight(UnityAction action)
    {
        animator.SetTrigger("right");
        this.action = action;
    }

    public void PlayOver()
    {
        action?.Invoke();
    }
}
