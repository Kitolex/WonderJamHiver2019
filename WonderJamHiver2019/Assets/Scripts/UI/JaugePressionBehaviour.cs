using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Events;
using UnityEngine.UI;

public class JaugePressionBehaviour : MonoBehaviour, EventListener<LocalPlayerStartEvent>
{
    Player localPlayer;
    RectTransform rTransf;

   
    void Start()
    {
        rTransf = GetComponent<RectTransform>();
        if (!rTransf)
            Debug.Log("L'aguille n'a pas été placée");
    }


    void Update()
    {
        if (!localPlayer)
            return;

        

        rTransf.rotation = Quaternion.Euler(0, 0, Mathf.Lerp(60, -60, (float)localPlayer.ressourceCount / (float)localPlayer.maxRessourceCount));
    }

    public void OnEvent(LocalPlayerStartEvent eventType)
    {
        localPlayer = eventType.localPlayer.GetComponent<Player>();

    }


    void OnEnable()
    {
        this.EventStartListening<LocalPlayerStartEvent>();
    }

    void OnDisable()
    {
        this.EventStopListening<LocalPlayerStartEvent>();
    }

}
