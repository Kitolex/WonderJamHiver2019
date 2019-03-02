using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TeamLobbyManager : NetworkBehaviour
{

    public int teamLobby;

    [SyncVar]
    public int nbInThisTeam;

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
            if (!other.GetComponent<Player>().isLocalPlayer)
                return;
            other.GetComponent<Player>().CmdSetTeam(this.teamLobby);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            if (!other.GetComponent<Player>().isLocalPlayer)
                return;
            other.GetComponent<Player>().enterTeamZone = nbInThisTeam < 3;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            if (!other.GetComponent<Player>().isLocalPlayer)
                return;
            other.GetComponent<Player>().enterTeamZone = false;
            other.GetComponent<Player>().CmdSetTeam(0);
        }
    }

}
