using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTimer{

    public enum STATE
    {
        IDLE,
        RUN,
        FINISHED
    }
    public STATE state;
    public float duration = 1.0f;//预算截断的时间
    private float elapsedTime = 0;//流逝的时间
    public void Tick() {
        if (state == STATE.IDLE)
        {

        }
        else if (state == STATE.RUN)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime>=duration)
            {
                state = STATE.FINISHED;
            }
        }
        else if (state == STATE.FINISHED)
        {

        }
        else
        { Debug.Log("MyTimer Error"); }
    }

    public void Go() {
        elapsedTime = 0;
        state = STATE.RUN;
    }

}
