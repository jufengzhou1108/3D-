using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{ 
    private int atk;
    private float rotateSpeed = 50;
    private Animator animator;
    private CharacterController characterController;
    public Transform gunPoint;

    private void Awake()
    {
        //如果不在游戏场景则直接失活
        if (SceneManager.GetActiveScene().name != "GameScene")
        {
            this.enabled = false;
            return;
        }

        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        atk = RoleModel.Instance.GetRole(GameModel.Instance.Id).atk;
    }

    void Update()
    {
        //操控动画机完成移动
        animator.SetFloat("HSpeed", Input.GetAxis("Vertical"));
        animator.SetFloat("WSpeed", Input.GetAxis("Horizontal"));
        //旋转
        this.transform.Rotate(Vector3.up,Input.GetAxis("Mouse X")*rotateSpeed*Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            animator.SetLayerWeight(1, 1);
        }
        else if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            animator.SetLayerWeight(1, 0);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            animator.SetTrigger("Roll");
        }

        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("Attack");
        }
    }

    //刀攻击的处理
    public void KnifeEvent()
    {
        //播放刀音效
        SoundManager.Instance.PlaySound("Knife",this.transform.position);

        Collider[] colliders= Physics.OverlapSphere(this.transform.position + this.transform.forward + this.transform.up,1,1<<LayerMask.NameToLayer("Master"),QueryTriggerInteraction.UseGlobal);
        foreach(Collider collider in colliders)
        {
            collider.GetComponent<ZombieObject>().Wound(atk);
        }
    }

    //枪攻击的处理
    public void ShootEvent()
    {
        //播放枪音效
        SoundManager.Instance.PlaySound("Gun", this.transform.position);

        RaycastHit[] hits= Physics.RaycastAll(new Ray(gunPoint.position, gunPoint.forward),1000,1<<LayerMask.NameToLayer("Master"));

        foreach(RaycastHit hit in hits)
        {
            //射击处理
            hit.collider.GetComponent<ZombieObject>().Wound(atk);
        }
    }
}
