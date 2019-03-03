using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SpriteOrientationBehavior : MonoBehaviour
{

    void Start()
    {
        SpriteRenderer sr = this.GetComponentInChildren<SpriteRenderer>();
		
		sr.shadowCastingMode = ShadowCastingMode.On;
		sr.receiveShadows = true;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.forward = Camera.main.transform.forward;
    }
}
