using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager :MonoBehaviour 
{
    public Transform birthPoint;
    private bool isPause=false;

    private static GameManager instance;
    public static GameManager Instance => instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        AddressableMgr.Instance.LoadResAsync<GameObject>(RoleModel.Instance.GetRole(GameModel.Instance.Id).name, (role) =>
        {
            GameObject player=Instantiate(role);

            //排除角色控制器的影响
            player.GetComponent<CharacterController>().enabled = false;
            player.transform.position=birthPoint.position;
            player.GetComponent<CharacterController>().enabled = true;

            Camera.main.GetComponent<CameraFollow>().SetTarget(player.transform);
        });
        ViewManager.Instance.Show<GameView>();

        EventCenter.Instance.AddListener(ZombiePoint.EndEvent, GameEnd);
        EventCenter.Instance.AddListener(MainTowerObject.DeadEvent,GameFail);
    }
    public void GameEnd()
    {
        if (GameModel.Instance.HP > 0)
        {
            ViewManager.Instance.ShowTip("游戏胜利\n获得￥100", () =>
            {
                ViewManager.Instance.Clear();
                ObjectPool.Instance.Clear();   
                SceneManager.LoadScene("BeginScene");
                PlayerModel.Instance.Money += 50;
                TimeManager.Play();
            });
        }
        GameFail();
    }

    public void GameFail()
    {
        ViewManager.Instance.ShowTip("游戏失败\n获得￥50", () =>
        {
            ViewManager.Instance.Clear();
            ObjectPool.Instance.Clear();
            SceneManager.LoadScene("BeginScene");
            PlayerModel.Instance.Money += 50;
            TimeManager.Play();
        });
    }


    private void OnDestroy()
    {
        AddressableMgr.Instance.Release<GameObject>(RoleModel.Instance.GetRole(GameModel.Instance.Id).name);
        EventCenter.Instance.RemoveListener(ZombiePoint.EndEvent, GameEnd);
        EventCenter.Instance.RemoveListener(MainTowerObject.DeadEvent, GameFail);
    }
}
