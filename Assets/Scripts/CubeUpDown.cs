using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeUpDown : MonoBehaviour {
    private Animation ani;
    // Use this for initialization
    void Start () {
        ani = transform.GetComponent<Animation>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        ani.Play("Up");
    }

    private void OnTriggerExit(Collider other)
    {
        ani.Play("Down");
    }
}
