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

        if (!isServer)
            return;



        if (other.tag.Equals("Player"))
        {

            other.GetComponent<Player>().ressourceCount++;
            other.GetComponent<Player>().RpcCollectRessource();
            RpcDestroyRessource();
        }
    }

    [ClientRpc]
    private void RpcDestroyRessource()
    {
        Destroy(this.gameObject);
    }

}
