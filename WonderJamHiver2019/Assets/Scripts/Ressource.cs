using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Ressource : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger");
        if (!isServer)
            return;

        Debug.Log("server");

        if (other.tag.Equals("Player"))
        {
            Debug.Log("tag Player detect");
            RpcDestroyRessource();
        }
    }

    [ClientRpc]
    private void RpcDestroyRessource()
    {
        Debug.Log("RpcDestroyRessource");
        Destroy(this.gameObject);
    }

}
