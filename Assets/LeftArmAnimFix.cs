using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftArmAnimFix : MonoBehaviour {

    private Animator anim;
    public Vector3 a;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    //void Start () {}
	
	//void Update () {}

    private void OnAnimatorIK(int layerIndex)
    {
        if (anim.GetBool("Defense") == false)
        {
            Transform leftLowerArm = anim.GetBoneTransform(HumanBodyBones.LeftLowerArm);//或取左下臂的骨骼
            leftLowerArm.localEulerAngles += a;
            anim.SetBoneLocalRotation(HumanBodyBones.LeftLowerArm, Quaternion.Euler(leftLowerArm.localEulerAngles));
        }
        else
        {
            Transform leftLowerArm = anim.GetBoneTransform(HumanBodyBones.LeftLowerArm);//或取左下臂的骨骼
            leftLowerArm.localEulerAngles += new Vector3(-50,25,0);
            anim.SetBoneLocalRotation(HumanBodyBones.LeftLowerArm, Quaternion.Euler(leftLowerArm.localEulerAngles));
        }
        
    }
}
