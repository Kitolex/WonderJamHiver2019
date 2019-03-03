using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Events;

public class Decollage : MonoBehaviour, EventListener<EndGameEvent>
{
    public GameObject Particle;

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
        if (GetComponent<Base>().team == eventType.winnerTeam)
        {           
            StartCoroutine(EndGameCoroutine());
        }
           
    }

    IEnumerator EndGameCoroutine()
    {
        Particle.SetActive(true);
        yield return new WaitForSeconds(2);
        GetComponent<Animator>().SetTrigger("Decollage");
    }
}
