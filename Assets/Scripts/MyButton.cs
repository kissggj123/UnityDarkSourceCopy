using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyButton{

    public bool IsPressing=false;//正在按压
    public bool OnPressed = false;//按下
    public bool OnReleased = false;//释放
    public bool IsExtending = false;//是否按压后延时
    public bool IsDelaying = false;//是否按压延时后触发
    public float extendingDuration = 0.5f;//在xxx时间内按压extending有效
    public float delayingDuration = 0.5f;//持续按压XXX后有效

    public bool curState = false;
    public bool lastState = false;

    private MyTimer exitTime = new MyTimer();
    private MyTimer delayingTime = new MyTimer();

    public void Tick(bool input) {

        exitTime.Tick();
        delayingTime.Tick();

        curState = input;
        IsPressing = curState;

        OnPressed = false;
        OnReleased = false;
        IsExtending = false;
        IsDelaying = false;

        if (curState!=lastState)
        {
            if (curState == true)//按下
            {
                OnPressed = true;
                StartTimer(delayingTime, delayingDuration);
            }
            else {               //抬起
                OnReleased = true;
                StartTimer(exitTime, extendingDuration);//抬起延时
            }
        }
        lastState = curState;
        if (exitTime.state == MyTimer.STATE.RUN)
        {
            IsExtending = true;
        }
        if (delayingTime.state == MyTimer.STATE.RUN)
        {
            IsDelaying = true;
        }

    }

    private void StartTimer(MyTimer timer, float duration) {
        timer.duration = duration;
        timer.Go();
    }


}
