using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Events;
using UnityEngine.UI;

public class JaugePressionBehaviour : MonoBehaviour, EventListener<LocalPlayerStartEvent>
{
    Player localPlayer;
    RectTransform rTransf;

    private float current;
    float smoothTime = 0.3f;
    float velocity = 0.0f;

    void Start()
    {
        rTransf = GetComponent<RectTransform>();
        if (!rTransf)
            Debug.Log("L'aguille n'a pas été placée");

        current = 0.5f;
    }


    void Update()
    {
        if (!localPlayer)
            return;

        current = Mathf.SmoothDamp(current, (float)localPlayer.ressourceCount / (float)localPlayer.maxRessourceCount, ref velocity, smoothTime);

        rTransf.rotation = Quaternion.Euler(0, 0, Mathf.Lerp(60, -60, current));
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
