using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Events;

public class Shake : MonoBehaviour, EventListener<EndGameEvent>
{
    public float Amplitude = 0.5f;

    public bool isShaking = false;

    Vector3 initialPosition;

    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isShaking)
            return;

        transform.position = new Vector3(initialPosition.x + Random.Range(-Amplitude, Amplitude), transform.position.y, initialPosition.z + Random.Range(-Amplitude, Amplitude));
    }


    void OnEnable()
    {
        this.EventStartListening<EndGameEvent>();
    }
    void OnDisable()
    {
        this.EventStopListening<EndGameEvent>();
    }

    public void OnEvent(EndGameEvent eventType)
    {
        isShaking = true;
    }
}
