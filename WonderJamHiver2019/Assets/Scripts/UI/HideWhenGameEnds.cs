using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Events;

public class HideWhenGameEnds : MonoBehaviour, EventListener<EndGameEvent>
{
    public void OnEvent(EndGameEvent eventType)
    {
        gameObject.SetActive(false);
    }

    void OnEnable()
    {
        this.EventStartListening<EndGameEvent>();
    }

    void OnDisable()
    {
        this.EventStopListening<EndGameEvent>();
    }
}
