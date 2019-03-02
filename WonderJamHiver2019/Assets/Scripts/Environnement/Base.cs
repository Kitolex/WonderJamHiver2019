using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Base : NetworkBehaviour
{
    public int team;

    [SyncVar]
    public int currentPression;

    public int neededPressionToWin = 1000;

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
        if (other.tag.Equals("Player"))
        {
            Player player = other.gameObject.GetComponent<Player>();
            
            if(!player.isLocalPlayer || PlayerState.singleton.myTeam != this.team)
                return;

            ShowUICanInteractWithBase();
            player.isInBaseZone = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            Player player = other.gameObject.GetComponent<Player>();
            
            if(!player.isLocalPlayer || PlayerState.singleton.myTeam != this.team)
                return;

            HideUICanInteractWithBase();
            player.isInBaseZone = false;
        }
    }

    private void ShowUICanInteractWithBase()
    {

    }

    private void HideUICanInteractWithBase()
    {

    }
}
