using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Events;

public class CameraBehaviour : MonoBehaviour, EventListener<EndGameEvent>
{
    GameObject player;
    public GameObject Base1;
    public GameObject Base2;
    Camera camera;

    public float Distance;
    public float Angle;
    public Vector3 CameraOffset;
    public float SmoothSpeed = 0.1f;

    public bool IsFollowingPlayer = true;

    // Start is called before the first frame update
    void Start()
    {
        camera = GetComponent<Camera>();
        if (!camera)
            Debug.LogWarning("Pas de camera détecté");
    }

    // Update is called once per frame
    void Update()
    {
        if (!player)
            return;

        if (!IsFollowingPlayer)
            return;

        Vector3 desiredPosition = new Vector3(0, 0, -Distance);
        desiredPosition = Quaternion.AngleAxis(Angle, Vector3.right) * desiredPosition;
        desiredPosition = player.transform.position + desiredPosition + CameraOffset;

        Vector3 smoothPostion = Vector3.Lerp(transform.position, desiredPosition, SmoothSpeed);

        transform.position = smoothPostion;
        transform.rotation = Quaternion.Euler(new Vector3(Angle, 0, 0));
    }


    public void AssignPlayer(GameObject playerGameObject)
    {
        player = playerGameObject;
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
        IsFollowingPlayer = false;
        StartCoroutine(EndGameCoroutine(eventType.winnerTeam));
    }

    IEnumerator EndGameCoroutine(int winnerTeam)
    {
        float timeStart = Time.time;
        Vector3 initialPosition = transform.position;
        Vector3 destination;

        if(winnerTeam == 1)
        {
            destination = new Vector3(0, 0, -Distance);
            destination = Quaternion.AngleAxis(Angle, Vector3.right) * destination;
            destination = Base1.transform.position + destination + CameraOffset;
            transform.rotation = Quaternion.Euler(new Vector3(Angle, 0, 0));
        }
        else
        {
            destination = new Vector3(0, 0, -Distance);
            destination = Quaternion.AngleAxis(Angle, Vector3.right) * destination;
            destination = Base2.transform.position + destination + CameraOffset;
            transform.rotation = Quaternion.Euler(new Vector3(Angle, 0, 0));
        }
         

        while ((Time.time - timeStart) < 2)
        {
            transform.position = Vector3.Lerp(initialPosition, destination, (Time.time - timeStart) / 2);
            yield return null;
        }
    }
}
