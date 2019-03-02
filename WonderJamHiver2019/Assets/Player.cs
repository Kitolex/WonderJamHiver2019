using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour
{
    public int ID;

    // Start is called before the first frame update
    void Start()
    {
    }

    public override void OnStartLocalPlayer()
    {
        Camera.main.GetComponent<CameraBehaviour>().AssignPlayer(gameObject); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
