using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Events;
using TMPro;

public class VictoryText : MonoBehaviour, EventListener<EndGameEvent>
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
        
        StartCoroutine(EndGameCoroutine(eventType.winnerTeam));


    }

    IEnumerator EndGameCoroutine(int winner)
    {
        yield return new WaitForSeconds(3);
        GetComponent<TextMeshProUGUI>().SetText("TEAM " + winner + " \n WINS!");
    }
}
