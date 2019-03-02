using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Ressource : NetworkBehaviour
{
    public int nbPressionGive = 10;
    
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
            Player player = other.GetComponent<Player>();
            player.ressourceCount = Mathf.Min(player.ressourceCount + nbPressionGive, player.maxRessourceCount);
            player.RpcCollectRessource();
            RpcDestroyRessource();
        }
    }

    [ClientRpc]
    private void RpcDestroyRessource()
    {
        Destroy(this.gameObject);
    }

}
