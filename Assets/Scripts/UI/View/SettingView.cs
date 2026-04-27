using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingView : MonoBehaviour
{
    public Toggle totalToggel;
    public Toggle musicToggel;
    public Toggle soundToggel;

    public Slider totalSlider;
    public Slider musicSlider;
    public Slider soundSlider;

    public Button quitButton;
    private MusicData musicData;

    private void Start()
    {
        totalToggel.onValueChanged.AddListener((v) =>
        {
            SoundModel.Instance.HasTotal = v;
        });
        musicToggel.onValueChanged.AddListener((v) =>
        {
            SoundModel.Instance.HasMusic = v;
        });
        soundToggel.onValueChanged.AddListener((v) =>
        {
            SoundModel.Instance.HasSound = v;
        });
        totalSlider.onValueChanged.AddListener((v) =>
        {
            SoundModel.Instance.TotalVolume = v;
        });
        musicSlider.onValueChanged.AddListener((v) =>
        {
            SoundModel.Instance.MusicVolume = v;
        });
        soundSlider.onValueChanged.AddListener((v) =>
        {
            SoundModel.Instance.SoundVolume = v;
        });
        quitButton.onClick.AddListener(() =>
        {
            SoundModel.Instance.SaveData();
            ViewManager.Instance.Hide<SettingView>();
        });

        totalToggel.SetIsOnWithoutNotify(SoundModel.Instance.HasTotal);
        musicToggel.SetIsOnWithoutNotify(SoundModel.Instance.HasMusic);
        soundToggel.SetIsOnWithoutNotify(SoundModel.Instance.HasSound);
        totalSlider.SetValueWithoutNotify(SoundModel.Instance.TotalVolume);
        musicSlider.SetValueWithoutNotify(SoundModel.Instance.MusicVolume);
        soundSlider.SetValueWithoutNotify(SoundModel.Instance.SoundVolume);
    }
}
