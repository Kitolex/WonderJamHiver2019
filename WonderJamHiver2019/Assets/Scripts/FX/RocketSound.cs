using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Events;

public class RocketSound : MonoBehaviour,EventListener<EndGameEvent>
{
    public AudioClip ClipDecollage;

    private AudioSource source;
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
        StartCoroutine(EndGameCoroutine());
    }

    IEnumerator EndGameCoroutine()
    {
        yield return new WaitForSeconds(1);
        source = GetComponent<AudioSource>();
        source.PlayOneShot(ClipDecollage,1.0F);
    }
}