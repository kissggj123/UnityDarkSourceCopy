using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnGroundSensor : MonoBehaviour {

    public CapsuleCollider capcol;
    public float offset = 0.1f;

    private Vector3 point1;
    private Vector3 point2;
    private float radiu;//半径
   
    void Awake()
    {
        radiu = capcol.radius-0.05f;
    }

    void FixedUpdate()
    {
        point1 = transform.position + transform.up * (radiu-offset);
        point2 = transform.position + transform.up * (capcol.height-offset) - transform.up * radiu;

        Collider[] ouputCols = Physics.OverlapCapsule(point1, point2, radiu, LayerMask.GetMask("Ground"));//仅限于地面碰撞判断
        //Collider[] ouputCols = Physics.OverlapCapsule(point1, point2, radiu);//所有物体都碰撞
        if (ouputCols.Length != 0)
        {
            //foreach (var col in ouputCols){
            //    print("collision: "+ col.name);
            //}
            SendMessageUpwards("IsGround");
        }
        else
        {
            SendMessageUpwards("IsNotGround");
        }
    }
}
