using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Events;

public class BaseIndicatorBehaviour : MonoBehaviour, EventListener<LocalPlayerStartEvent>, EventListener<EndGameEvent>
{
    RectTransform rTransf;
    Image img;
    GameObject player;
    public Base teamBase1; // A renseigner dans la scene
    public Base teamBase2; // A renseigner dans la scene
    public float DistanceAffichage;

    bool isVisible = false;
    bool stopped = false;



    // Start is called before the first frame update
    void Start()
    {
        rTransf = GetComponent<RectTransform>();
        if (!rTransf)
            Debug.LogWarning("Pas de rect transform trouvé sur le gameobject");

        img = GetComponent<Image>();
        if (!img)
            Debug.LogWarning("Pas d'image trouvé sur le gameobject");

        img.CrossFadeAlpha(0, 0, false);

        if(PlayerState.singleton.myTeam == 1)
        {
            PlayerState.singleton.myBase = teamBase1;
        }

        if(PlayerState.singleton.myTeam == 2)
        {
            PlayerState.singleton.myBase = teamBase2;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!player || !PlayerState.singleton.myBase || stopped)
            return;

        Vector2 orientation = new Vector2(player.transform.position.x - PlayerState.singleton.myBase.transform.position.x, player.transform.position.z - PlayerState.singleton.myBase.transform.position.z);
        float angle = Vector2.SignedAngle(Vector2.right, -orientation.normalized);

        if (orientation.magnitude > DistanceAffichage && !isVisible)
            ShowIndicator();
        if (orientation.magnitude < DistanceAffichage && isVisible)
            HideIndicator();

        if (!isVisible)
            return;        
        
        rTransf.rotation = Quaternion.Euler(0, 0, angle);

    }

    public void ShowIndicator()
    {
        img.CrossFadeAlpha(0.7f, 1, false);
        isVisible = true;
    }

    public void HideIndicator()
    {
        img.CrossFadeAlpha(0, 1, false);
        isVisible = false;
    }

  


    public void OnEvent(LocalPlayerStartEvent eventType)
    {
        player = eventType.localPlayer;
    }

    public void OnEvent(EndGameEvent eventType)
    {
        HideIndicator();
        stopped = true;
    }

    void OnEnable()
    {
        this.EventStartListening<LocalPlayerStartEvent>();
        this.EventStartListening<EndGameEvent>();
    }

    void OnDisable()
    {
    	this.EventStopListening<LocalPlayerStartEvent>();
        this.EventStopListening<EndGameEvent>();
    }
}
