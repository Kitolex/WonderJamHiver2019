using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour
{

    [SyncVar]
    public int ressourceCount;


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

    [ClientRpc]
    public void RpcCollectRessource()
    {
        //EFFET VISUEL
    }
}
