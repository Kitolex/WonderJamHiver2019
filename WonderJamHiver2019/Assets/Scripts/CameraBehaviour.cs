using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CameraBehaviour : MonoBehaviour
{
    GameObject player;
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
}
