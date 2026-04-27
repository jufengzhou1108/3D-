using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    //目标位置
    public Transform targetTrans;
    //相对偏移
    public Vector3 offsetPos;
    //看向位置的Y偏移值
    public float bodyHeight;
    //移动速度
    public float moveSpeed;
    //旋转速度
    public float rotateSpeed;

    private void Update()
    {
        if(targetTrans == null)
        {
            return;
        }

        Vector3 targetPos= targetTrans.position+targetTrans.forward*offsetPos.z;
        targetPos += targetTrans.up * offsetPos.y;
        targetPos += targetTrans.right * offsetPos.x;
        this.transform.position = Vector3.Lerp(this.transform.position,targetPos,moveSpeed*Time.deltaTime);

        Quaternion targetRotation = Quaternion.LookRotation(targetTrans.position + Vector3.up * bodyHeight - this.transform.position);
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation,targetRotation,Time.deltaTime*rotateSpeed);
    }

    public void SetTarget(Transform target)
    {
        targetTrans= target;
    }
}
