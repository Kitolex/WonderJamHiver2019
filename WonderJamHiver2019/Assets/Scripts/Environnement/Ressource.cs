using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Random = UnityEngine.Random;

public class Ressource : NetworkBehaviour
{
    public int nbPressionGive = 10;

    public float disableTimeOnCreation = 1f;
    public bool collectable = false;

    public bool jumpAtSpawn;
    public int aleaLatteralSpawnForce;
    public int upForce;

    private float enableAt;
    private Rigidbody rb;
    
    // Start is called before the first frame update
    void Start()
    {
        enableAt = Time.time + disableTimeOnCreation;
        if (jumpAtSpawn)
        {
            rb = this.GetComponent<Rigidbody>();
            if (rb)
            {
                rb.AddForce(new Vector3(Random.Range(0f, 1f) * aleaLatteralSpawnForce, 1 * upForce, Random.Range(0f, 1f) * aleaLatteralSpawnForce));
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (!collectable)
        {
            if(Time.time >= enableAt)
            {
                collectable = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (!isServer)
            return;

        if (!collectable)
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
