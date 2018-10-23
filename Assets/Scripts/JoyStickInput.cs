using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoyStickInput : MonoBehaviour {

    public bool isEnable = true;
    [Header("========= 手柄控制设置 ========")]
    public string lAxisX = "Horizontal";
    public string lAxisY = "Vertical";
    public string rAxisX = "Horizontal3th";
    public string rAxisY = "Vertical4th";
    public string btn0 = "btn0";//A
    public string btn1 = "btn1";//B
    public string btn2 = "btn2";//Y
    public string btn3 = "btn3";//X
    public string btn4 = "btn4";//L1
    public string btn5 = "btn5";//R1
    public string btnR = "btnR";//右摇杆按键


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
    public bool attack;
    public bool roll;
    public bool lockOn;

    [Header("========= 其它 ========")]
    private float targetDup;
    private float targetDright;
    private float velocityDup;
    private float velocityDright;


    [Header("========= 按键输入转换后 ========")]
    public MyButton buttonA = new MyButton();
    public MyButton buttonB = new MyButton();
    public MyButton buttonY = new MyButton();
    public MyButton buttonX = new MyButton();
    public MyButton buttonL1 = new MyButton();
    public MyButton buttonR1 = new MyButton();
    public MyButton buttonR = new MyButton();

	
	// Update is called once per frame
	void Update () {

        buttonA.Tick(Input.GetButton(btn0));
        buttonB.Tick(Input.GetButton(btn1));
        buttonY.Tick(Input.GetButton(btn2));
        buttonX.Tick(Input.GetButton(btn3));
        buttonL1.Tick(Input.GetButton(btn4));
        buttonR1.Tick(Input.GetButton(btn5));
        buttonR.Tick(Input.GetButton(btnR));

        //控制相机
        Jup = Input.GetAxis(rAxisY);
        JRight = Input.GetAxis(rAxisX);
        //Jup = Input.GetAxis("Mouse Y");
        //JRight = Input.GetAxis("Mouse X");

        //移动控制
        targetDup = Input.GetAxis(lAxisY);
        targetDright = Input.GetAxis(lAxisX);

        if (isEnable == false)
        {
            targetDup = 0;
            targetDright = 0;
        }

        Dup = Mathf.SmoothDamp(Dup, targetDup, ref velocityDup, 0.1f);
        Dright = Mathf.SmoothDamp(Dright, targetDright, ref velocityDright, 0.1f);

        Vector2 tempDAxis = SquareToCircle(new Vector2(Dright, Dup));
        float transDright = tempDAxis.x;
        float transDup = tempDAxis.y;

        Dmag = Mathf.Sqrt((transDup * transDup) + (transDright * transDright));
        Dvac = transDright * transform.right + transDup * transform.forward;

        //跑
        run = (buttonL1.IsPressing && !buttonL1.IsDelaying)||buttonL1.IsExtending;
        //跳跃
        jump = buttonL1.OnPressed && buttonL1.IsExtending;//跑跳整合
        ////防御
        defense = buttonR1.IsPressing;
        //手柄B键攻击
        attack = buttonB.OnPressed;
        //翻滚
        roll = buttonL1.IsDelaying && buttonL1.OnReleased;
        //锁定
        lockOn = buttonR.OnPressed;

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
        output.x = input.x * Mathf.Sqrt(1 - (input.y * input.y) / 2.0f);
        output.y = input.y * Mathf.Sqrt(1 - (input.x * input.x) / 2.0f);
        return output;
    }
}
