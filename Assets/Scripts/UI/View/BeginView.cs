using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeginView : MonoBehaviour
{
    [SerializeField]
    private Button beginButton;
    [SerializeField]
    private Button settingButton;
    [SerializeField]
    private Button quitButton;

    private void Awake()
    {
        beginButton.onClick.AddListener(() =>
        {
            Camera.main.GetComponent<CameraAnimator>().TrunLeft(() =>
            {
                ViewManager.Instance.Show<ChooseRoleView>();
            });

            ViewManager.Instance.Hide<BeginView>();
        });
        settingButton.onClick.AddListener(() =>
        {
            ViewManager.Instance.Show<SettingView>();
        });
        quitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }
}
