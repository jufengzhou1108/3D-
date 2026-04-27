using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameView : MonoBehaviour
{
    private float HP_WIDTH = 580f;

    public Image bloodImage;
    public Text bloodText;
    public Text remainText;
    public Text moneyText;
    public GameObject bottom;
    public Button returnButton;

    public Image towerImage1;
    public Image towerImage2;
    public Image towerImage3;

    public Text towerMoneyText1;
    public Text towerMoneyText2;
    public Text towerMoneyText3;

    private void Awake()
    {
        returnButton.onClick.AddListener(() =>
        {
            ViewManager.Instance.Clear();
            SceneManager.LoadScene("BeginScene");
        });
        //InitTowers();
        UpDataView();
        EventCenter.Instance.AddListener(PlayerModel.UPDATE_EVENT, UpDataView);
        EventCenter.Instance.AddListener(GameModel.UPDATE_EVENT, UpDataView);
    }

    private void InitTowers()
    {
        TowerData[] towers = GameModel.Instance.Towers;
        AddressableMgr.Instance.LoadResAsync<Sprite>(towers[0].url, (sprite) =>
        {
            towerImage1.sprite = sprite;
        });
        towerMoneyText1.text=towers[0].money.ToString();
        AddressableMgr.Instance.LoadResAsync<Sprite>(towers[1].url, (sprite) =>
        {
            towerImage2.sprite = sprite;
        });
        towerMoneyText2.text = towers[1].money.ToString();
        AddressableMgr.Instance.LoadResAsync<Sprite>(towers[2].url, (sprite) =>
        {
            towerImage3.sprite = sprite;
        });
        towerMoneyText3.text = towers[2].money.ToString();
    }

    private void UpDataView()
    {
        GameModel data=GameModel.Instance;
        float hpRate =data.HP / 100f;
        bloodImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, HP_WIDTH * hpRate);
        bloodText.text=(int)data.HP + "/100";

        remainText.text = data.NowWaveNum + "/" + data.AllWaveNum;
        moneyText.text = PlayerModel.Instance.Money.ToString();
    }

    private void OnDestroy()
    {
        EventCenter.Instance.RemoveListener(PlayerModel.UPDATE_EVENT, UpDataView);
        EventCenter.Instance.RemoveListener(GameModel.UPDATE_EVENT, UpDataView);
    }
}
