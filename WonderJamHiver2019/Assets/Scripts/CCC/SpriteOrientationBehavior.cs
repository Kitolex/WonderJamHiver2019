using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteOrientationBehavior : MonoBehaviour
{

    Camera camera;

    void Start()
    {
        this.camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.forward = camera.transform.forward;
    }
}
