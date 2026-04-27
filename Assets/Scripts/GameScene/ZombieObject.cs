using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieObject : MonoBehaviour
{
    private Animator animator;
    private NavMeshAgent agent;
    private ZombieInfo info;

    private int hp;
    private bool isDead;
    private float lastAtkTime;
    private bool isAtk;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    public void InitInfo(ZombieInfo info)
    {
        this.info = info;
        this.animator.runtimeAnimatorController = AddressableMgr.Instance.LoadRes<RuntimeAnimatorController>(info.animator);
        hp= info.hp;
        agent.speed=info.moveSpeed;
        agent.angularSpeed = info.roundSpeed;
        agent.acceleration = info.moveSpeed;
    }

    public void Wound(int damage)
    {
        hp-= damage;
        animator.SetTrigger("wound");

        if(hp < 1)
        {
            Dead();
            return;
        }

        //播放受伤音效
        SoundManager.Instance.PlaySound("Wound",this.transform.position);
    }

    private void Dead()
    {
        agent.isStopped = true;
        animator.SetBool("isDead", true);
        isDead = true;

        //播放死亡音效
        SoundManager.Instance.PlaySound("dead", this.transform.position);

        //加钱
        PlayerModel.Instance.Money += 10;
    }

    public void Clear()
    {
        Destroy(this.gameObject);
    }

    public void BornOver()
    {
        agent.SetDestination(MainTowerObject.Instance.transform.position);
        animator.SetBool("isRun", true);
    }

    private void Update()
    {
        if (isDead)
        {
            return;
        }

        animator.SetBool("isRun",agent.velocity!=Vector3.zero);

        lastAtkTime += Time.deltaTime;
        if (Vector3.Distance(this.transform.position, MainTowerObject.Instance.transform.position) <= 5&&lastAtkTime>info.atkOffst&&!isAtk)
        {
            animator.SetTrigger("attack");
            isAtk = true;
        }
    }

    public void AtkEvent()
    {
        SoundManager.Instance.PlaySound("Eat", this.transform.position);

        MainTowerObject.Instance.Attacked(info.atk);
        lastAtkTime=0;
        isAtk=false;
    }

    private void OnDestroy()
    {
        AddressableMgr.Instance.Release<RuntimeAnimatorController>(info.animator);
    }
}
