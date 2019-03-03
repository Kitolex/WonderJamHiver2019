using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyMeAfter : MonoBehaviour {

	public float delayUntilDestroy;
	private float timer;

	// Use this for initialization
	void Start () {
		timer = Time.time + delayUntilDestroy;
	}
	
	// Update is called once per frame
	void Update () {
		if(Time.time >= timer){
			Destroy(this.gameObject);
		}
	}
}
