using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Events;

public class BaseIndicatorBehaviour : MonoBehaviour, EventListener<LocalPlayerStartEvent>
{
    RectTransform rTransf;
    Image img;
    GameObject player;
    public GameObject teamBase; // A renseigner dans la scene
    public float DistanceAffichage;

    bool isVisible = false;

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
    }

    // Update is called once per frame
    void Update()
    {
        if (!player || !teamBase)
            return;

        Vector2 orientation = new Vector2(player.transform.position.x - teamBase.transform.position.x, player.transform.position.z - teamBase.transform.position.z);
        float angle = Vector2.SignedAngle(Vector2.right, -orientation.normalized);

        if (orientation.magnitude > DistanceAffichage && !isVisible)
            ShowIndicator();
        if (orientation.magnitude < DistanceAffichage && isVisible)
            HideIndicator();



        if (!isVisible)
            return;
        
        
        rTransf.rotation = Quaternion.Euler(0, 0, angle);


        Vector3 iconPosition = Camera.main.WorldToScreenPoint(teamBase.transform.position) - new Vector3((Screen.width / 2f), (Screen.height / 2f),0);
        iconPosition.x = Mathf.Clamp(iconPosition.x, -(Screen.width/2f) + 35, (Screen.width/2f) - 35);
        iconPosition.y = Mathf.Clamp(iconPosition.y, -(Screen.height / 2f) + 35, (Screen.height / 2f) - 35);

        rTransf.localPosition = iconPosition;
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

    void OnEnable()
    {
        this.EventStartListening<LocalPlayerStartEvent>();
    }

    void OnDisable()
    {
    	this.EventStopListening<LocalPlayerStartEvent>();
    }
}
