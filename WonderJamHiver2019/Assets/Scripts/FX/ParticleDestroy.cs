using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDestroy : MonoBehaviour {

	private ParticleSystem ps;

	// Use this for initialization
	void Awake () {
		ps = GetComponent<ParticleSystem> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!ps.IsAlive ()) {
			Destroy (this.gameObject);
		}
	}
}
