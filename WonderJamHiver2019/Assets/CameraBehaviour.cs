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

        Vector3 test = new Vector3(0, 0, -Distance);
        test = Quaternion.AngleAxis(Angle, Vector3.right) * test;

        transform.position = player.transform.position + test + CameraOffset;
        transform.rotation = Quaternion.Euler(new Vector3(Angle, 0, 0));
    }


    public void AssignPlayer(GameObject playerGameObject)
    {
        player = playerGameObject;
    }
}
