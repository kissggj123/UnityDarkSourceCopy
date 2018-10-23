using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerInput : MonoBehaviour {

    public bool isEnable = true;

    [Header("========= 按键设置 ========")]
    public string keyUp="w";
    public string keyDown="s";
    public string keyLeft="a";
    public string keyRight="d";

    public string keyA;
    public string keyB;
    public string keyC;
    public string keyD;

    public string keyJUp;
    public string keyJDown;
    public string keyJLeft;
    public string keyJRight;

    [Header("========= 输出信号设置 ========")]
    public float Dup;//目标上下移动
    public float Dright;//目标左右移动

    public float Dmag;//目标移动距离
    public Vector3 Dvac;//目标方向

    public float Jup;//右摇杆上下
    public float JRight;//右摇杆左右

    public bool defense;
    //按压型触发
    public bool run;//是否跑步
    //一次性触发
    public bool jump;
    private bool lastJump;

    public bool attack;
    private bool lastAttack;

    [Header("========= 鼠标 ========")]
    public bool mauseEnable=false;

    [Header("========= 其它 ========")]
    private float targetDup;
    private float targetDright;
    private float velocityDup;
    private float velocityDright;

    void Start () {
		
	}
	
	
	void Update () {
        //控制相机
        if (mauseEnable == true){
            Jup = Input.GetAxis("Mouse Y");
            JRight = Input.GetAxis("Mouse X");
        }
        else {
            Jup = (Input.GetKey(keyJUp) ? 1.0f : 0) - (Input.GetKey(keyJDown) ? 1.0f : 0);
            JRight = (Input.GetKey(keyJRight) ? 1.0f : 0) - (Input.GetKey(keyJLeft) ? 1.0f : 0);
        }
        
        targetDup = (Input.GetKey(keyUp)?1.0f:0) - (Input.GetKey(keyDown)?1.0f:0);
        targetDright = (Input.GetKey(keyRight) ?1.0f:0) - (Input.GetKey(keyLeft) ?1.0f:0);

        if (isEnable == false)
        {
            targetDup = 0;
            targetDright = 0;
        }

        Dup = Mathf.SmoothDamp(Dup,targetDup,ref velocityDup,0.1f);
        Dright = Mathf.SmoothDamp(Dright, targetDright,ref velocityDright,0.1f);

        Vector2 tempDAxis = SquareToCircle(new Vector2(Dright,Dup));
        float transDright = tempDAxis.x;
        float transDup = tempDAxis.y;

        Dmag = Mathf.Sqrt((transDup * transDup) + (transDright * transDright));
        Dvac = transDright * transform.right + transDup * transform.forward;

        run = Input.GetKey(keyA);

        //跳跃
        bool newJump = Input.GetKey(keyB);
        if (newJump != lastJump && newJump==true)
        { jump = true; }
        else
        { jump = false; }
        lastJump = newJump;

        //防御
        defense = Input.GetKey(keyD);

        //攻击
        bool newAttack = Input.GetKey(keyC);
        if (newAttack != lastAttack && newAttack == true)
        { attack = true; }
        else
        { attack = false; }
        lastAttack = newAttack;

        ////鼠标攻击
        //bool newAttack = Input.GetKey("mouse 0");
        //if (newAttack != lastAttack && newAttack == true)
        //{ attack = true; }
        //else
        //{ attack = false; }
        //lastAttack = newAttack;

        ////鼠标防御
        //defense = Input.GetKey("mouse 1");
    }

    public Vector2 SquareToCircle(Vector2 input)
    {
        Vector2 output = Vector2.zero;
        output.x= input.x * Mathf.Sqrt(1 - (input.y * input.y) / 2.0f);
        output.y= input.y * Mathf.Sqrt(1 - (input.x * input.x) / 2.0f);
        return output;
    }
}
