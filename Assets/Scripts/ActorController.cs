using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorController : MonoBehaviour {

    public GameObject model;
    //public playerInput pi;//键盘控制
    public JoyStickInput pi;//手柄控制
    public cameraController camcon;
    public Animator anim;
    public float walkSpeed = 1.4f;
    public float runSpeed = 2.8f;
    public float jumpVelocity=3.0f;
    public float rollVelocity = 3.0f;
    public float jabVelocity = 2.0f;

    private Rigidbody rigid;
    private Vector3 planarVec;
    private Vector3 thrusVec;//向上跳跃的冲量
    private bool lockPlanar=false;
    private bool canAttack = false;
    private float lerpTarget;//用于动画层级之间的缓动
    private Vector3 deltaPos;//用于记录root motion方式下模型的移动量

	void Awake () {
        //pi = GetComponent<playerInput>();//键盘控制
        pi = GetComponent<JoyStickInput>();//手柄控制
        anim = model.GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
	}
    //60FPS/s
    void Update () {
        float targetRunMulti = (pi.run ? 2.0f : 1.0f);
        anim.SetFloat("forward", pi.Dmag * Mathf.Lerp(anim.GetFloat("forward"),targetRunMulti,0.3f));

        if (pi.defense == true){
            anim.SetLayerWeight(anim.GetLayerIndex("Defense"), 1);
            anim.SetBool("Defense", pi.defense);
        }
        else {
            anim.SetLayerWeight(anim.GetLayerIndex("Defense"), 0);
            anim.SetBool("Defense", pi.defense);
        }

        if (pi.jump){
            anim.SetTrigger("jump");
            canAttack = false;
        }

        if (pi.attack && canAttack)
        { anim.SetTrigger("Attack"); }

        if (pi.Dmag > 0.1f)
        { model.transform.forward = Vector3.Slerp(model.transform.forward,pi.Dvac,0.2f); }

        if(lockPlanar == false)
        { planarVec = pi.Dmag * model.transform.forward * (pi.run ? runSpeed : 1.0f); }

        if (pi.roll||rigid.velocity.magnitude>7f)//是否翻滚
        {
            anim.SetTrigger("Roll");
        }

        if (pi.lockOn)
        {
            camcon.LockOrUnLock();
        }
	}

    private bool CheckState(string stateName, string layerName = "Base Layer") {
        return anim.GetCurrentAnimatorStateInfo(anim.GetLayerIndex(layerName)).IsName(stateName);
    }

    //50FPS/s
    private void FixedUpdate()
    {
        rigid.position += deltaPos;
        rigid.velocity = new Vector3(planarVec.x * walkSpeed, rigid.velocity.y , planarVec.z * walkSpeed) + thrusVec;
        thrusVec = Vector3.zero;
        deltaPos = Vector3.zero;
    }

    /// <summary>
    /// 
    /// </summary>
    public void OnJumpEnter()
    {
        pi.isEnable = false;
        lockPlanar = true;
        thrusVec = new Vector3(0, jumpVelocity, 0);     
    }

    public void IsGround(){
        anim.SetBool("isGround", true);
    }

    public void IsNotGround(){
        anim.SetBool("isGround", false);
    }

    public void OnGroundEnter(){
        pi.isEnable = true;
        lockPlanar = false;
        canAttack = true;
    }

    public void OnFallEnter(){
        pi.isEnable = false;
        lockPlanar = true;
        canAttack = false;
    }

    public void OnRollEnter() {
        pi.isEnable = false;
        lockPlanar = true;
        thrusVec = new Vector3(0, rollVelocity, 0);
    }

    public void OnJabEnter() {
        pi.isEnable = false;
        lockPlanar = true;
    }

    public void OnJabUpdate() {
        thrusVec = model.transform.forward * (-jabVelocity);
    }

    public void OnAttack1hA() {
        pi.isEnable = false;
        lerpTarget = 1.0f;
    }

    public void OnAttack1hAUpdate() {
        thrusVec = model.transform.forward * anim.GetFloat("Attack1hAVelocity");
        float currentWeight = anim.GetLayerWeight(anim.GetLayerIndex("Attack"));
        currentWeight = Mathf.Lerp(currentWeight,lerpTarget,0.1f);
        anim.SetLayerWeight(anim.GetLayerIndex("Attack"),currentWeight);
    }

    public void OnAttack1hC()
    {
       // pi.isEnable = false;
    }

    public void OnAttack1hCUpdate()
    {
       // thrusVec = model.transform.forward * anim.GetFloat("Attack1hCVelocity");
    }

    public void OnAttackIdleEnter() {
        pi.isEnable = true;
        lerpTarget = 0;
    }

    public void OnAttackIdleUpdate() {
        float currentWeight = anim.GetLayerWeight(anim.GetLayerIndex("Attack"));
        currentWeight = Mathf.Lerp(currentWeight, lerpTarget, 0.1f);
        anim.SetLayerWeight(anim.GetLayerIndex("Attack"), currentWeight);
    }

    public void OnUpdateRM(object _deltaPos) {
        if (CheckState("Attack1HC", "Attack"))//另外一种动画移动方式
        {
            deltaPos += (Vector3)_deltaPos;//累加偏移量，等待下一次累加
        }
    }

}