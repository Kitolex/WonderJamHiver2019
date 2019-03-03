using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Events;

public class Decollage : MonoBehaviour, EventListener<EndGameEvent>
{
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
        yield return new WaitForSeconds(2);
        GetComponent<Animator>().SetTrigger("Decollage");
    }
}
