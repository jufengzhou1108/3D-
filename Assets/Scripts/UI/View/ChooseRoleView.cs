using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChooseRoleView : MonoBehaviour
{
    public Text moneyText;
    public Text nameText;
    public Text buyText;

    public Button leftButton;
    public Button rightButton;
    public Button returnButton;
    public Button buyButton;
    public Button beginButton;

    private int currentId=1;
    private GameObject currentObj;
    private string beforeObjName;//用来释放资源

    void Awake()
    {
        EventCenter.Instance.AddListener(PlayerModel.UPDATE_EVENT,UpdateView);
        leftButton.onClick.AddListener(() =>
        {
            currentId = Mathf.Clamp(currentId-1, 1, 5);
            UpdateView();
        });
        rightButton.onClick.AddListener(() =>
        {
            currentId = Mathf.Clamp(currentId +1, 1, 5);
            UpdateView();
        });
        returnButton.onClick.AddListener(() =>
        {
            ViewManager.Instance.Hide<ChooseRoleView>();
            Destroy(currentObj);
            Camera.main.GetComponent<CameraAnimator>().TrunRight(() =>
            {
                ViewManager.Instance.Show<BeginView>();
            });
        });
        buyButton.onClick.AddListener(() => 
        {
            RoleInfo role = RoleModel.Instance.GetRole(currentId);
            if (PlayerModel.Instance.Money >= role.lockMoney)
            {
                PlayerModel.Instance.SetRole(role.name, true);
                PlayerModel.Instance.Money -= role.lockMoney;
            }
            else
            {
                ViewManager.Instance.ShowTip("金钱不足");
            }
        });
        beginButton.onClick.AddListener(() =>
        {
            GameModel.Instance.Id = currentId;
            ViewManager.Instance.Clear();
            ObjectPool.Instance.Clear();
            SceneManager.LoadScene("GameScene");
        });

        UpdateView();
    }

    private void UpdateView()
    {
        RoleInfo role = RoleModel.Instance.GetRole(currentId);
        moneyText.text = PlayerModel.Instance.Money.ToString();
        nameText.text = role.tips;
        AddressableMgr.Instance.LoadResAsync<GameObject>(role.name, (obj) =>
        {
            if(currentObj != null)
            {
                Destroy(currentObj);
                AddressableMgr.Instance.Release<GameObject>(beforeObjName);
            }
            currentObj= Instantiate(obj);
            beforeObjName = role.name;
        });
        buyButton.gameObject.SetActive(!PlayerModel.Instance.HasRole(role.name));
        buyText.text = "￥"+role.lockMoney;
    }

    private void OnDestroy()
    {
        AddressableMgr.Instance.Release<GameObject>(beforeObjName);
        EventCenter.Instance.RemoveListener(PlayerModel.UPDATE_EVENT,UpdateView);
    }
}
