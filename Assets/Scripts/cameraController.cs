using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cameraController : MonoBehaviour {
    //public playerInput pi;//键盘输入
    public JoyStickInput pi;//手柄输入
    public float rotateSpeed;

    private GameObject playerHandle;
    private GameObject cameraHandle;
    private Camera cam;
    private float tempEulerx;//x轴临时欧拉角

    private GameObject model;
    public GameObject lockTarget;
    public Image lockDot;

	// Use this for initialization
	void Awake () {
        cameraHandle = transform.parent.gameObject;
        playerHandle = cameraHandle.transform.parent.gameObject;
        tempEulerx = 20;
        model = playerHandle.GetComponent<ActorController>().model;
        lockDot.enabled = false;
        cam = GetComponent<Camera>();
    }
	
	// Update is called once per frame
	void Update () {

        if (lockTarget == null)
        {
            Vector3 tempModelEuler = model.transform.eulerAngles;

            playerHandle.transform.Rotate(Vector3.up, pi.JRight * rotateSpeed * Time.deltaTime);
            tempEulerx -= pi.Jup * -rotateSpeed * Time.deltaTime;
            tempEulerx = Mathf.Clamp(tempEulerx, -40, 30);//限制转动角度
            cameraHandle.transform.localEulerAngles = new Vector3(tempEulerx, 0, 0);
            model.transform.eulerAngles = tempModelEuler;
        }
        else
        {
            //锁定视角
            Vector3 tempForward = lockTarget.transform.position - model.transform.position;
            tempForward.y = 0;
            playerHandle.transform.forward = tempForward;
            cameraHandle.transform.LookAt(lockTarget.gameObject.transform);

            //调整圆圈位置
            float halfHigh = lockTarget.transform.localScale.y / 2.0f;
            print("目标高度： " + halfHigh + "目标位置： "+ lockTarget.gameObject.transform.position);
            lockDot.rectTransform.position = cam.WorldToScreenPoint(lockTarget.gameObject.transform.position);
            if (Vector3.Distance(model.transform.position,lockTarget.transform.position)>10.0f)
            {
                lockTarget = null;
                lockDot.enabled = false;
            }
        }


        //Cursor.lockState = CursorLockMode.Locked;//鼠标控制方向，但影藏其图标
    }

    public void LockOrUnLock() {
        //人物前置一长5米方形盒子，用于与物体碰撞
        Vector3 modelOrigin1 = model.transform.position;
        Vector3 modelOrigin2 = modelOrigin1 + new Vector3(0,1,0);
        Vector3 boxCenter = modelOrigin2 + model.transform.forward * 5.0f;//盒子中心点

        Collider[] cols = Physics.OverlapBox(boxCenter,new Vector3(0.5f,0.5f,5.0f),model.transform.rotation,LayerMask.GetMask("Enemy"));
        if (cols.Length == 0)
        {
            lockTarget = null;
            lockDot.enabled = false;
            print("未锁定目标");
        }
        else
        {
            foreach (var col in cols)
            {
                if (lockTarget==col.gameObject)
                {
                    print("同一目标连续锁定为解除");
                    lockTarget = null;
                    lockDot.enabled = false;
                    break;
                }
                print("已锁定目标");
                lockTarget = col.gameObject;
                lockDot.enabled = true;
                break;
            }
        }
    }
}
