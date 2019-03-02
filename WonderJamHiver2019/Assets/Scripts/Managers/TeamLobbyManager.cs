using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TeamLobbyManager : NetworkBehaviour
{

    public int teamLobby;

    private bool enterTeamZone;

    // Start is called before the first frame update
    void Start()
    {
        enterTeamZone = false;
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
            other.GetComponent<Player>().enterTeamZone = true;
            other.GetComponent<Player>().CmdSetTeam( this.teamLobby);
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

    /*private void OnTriggerStay(Collider other)
    {
        Debug.Log("trigger");


        
        if (other.tag.Equals("Player"))
        {
            Debug.Log("player");
            if (!other.GetComponent<Player>().isLocalPlayer)
                return;

            Debug.Log("local");
            if (Input.GetKey(KeyCode.E))
            {
                Debug.Log("Ajout Equipe");
                other.GetComponent<Player>().CmdSetTeam(this.teamLobby);

            }
        }
    }*/



}
