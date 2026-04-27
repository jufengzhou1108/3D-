using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TipView : MonoBehaviour
{
    public Text contentText;
    public Button sureButton;
    private UnityAction action;

    private void Awake()
    {
        sureButton.onClick.AddListener(() =>
        {
            ViewManager.Instance.Hide<TipView>();
            action?.Invoke();
        });
    }

    public void Init(string content, UnityAction action)
    {
        contentText.text = content;
        this.action = action;
    }

    private void OnDestroy()
    {
        EventCenter.Instance.RemoveListener(ViewManager.GetShowEvent<TipView>(),ViewManager.Instance.ShowTipOver);
    }
}
