using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RorateFX : MonoBehaviour {

	public float speed;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		this.transform.Rotate(new Vector3(0.0f, speed, 0.0f));
	}
}
