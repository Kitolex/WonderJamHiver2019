﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour
{

    [SyncVar]
    public int ressourceCount;

    [SyncVar]
    public int team;

    [SyncVar]
    public int realTeam;

    public bool enterTeamZone;

    

    // Start is called before the first frame update
    void Start()
    {
        team = 0;
        realTeam = 0;
        enterTeamZone = false;
    }

    public override void OnStartLocalPlayer()
    {
        Camera.main.GetComponent<CameraBehaviour>().AssignPlayer(gameObject); 
    }

    // Update is called once per frame
    void Update()
    {
        if (enterTeamZone)
        {
            if ((Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.E)) && this.realTeam == 0)
            {
                Debug.Log("Ajout Equipe");
                this.realTeam = -1; // set dirty pour éviter le spam, sera ecrasé par le serveur
                CmdSetRealTeam(team);
            }

            if ((Input.GetButtonDown("Fire2") || Input.GetKeyDown(KeyCode.R)) && this.realTeam != 0)
            {
                Debug.Log("Suprresion Equipe");
                CmdResetRealTeam();
            }
        }
    }

    [ClientRpc]
    public void RpcCollectRessource()
    {
        //EFFET VISUEL
    }

    [Command]
    public void CmdSetRealTeam(int team)
    {
        this.realTeam = team;
        RpcSetActivePlayer(false);
        GameObject.FindGameObjectWithTag("LobbyManager").GetComponent<LobbyManager>().AddReadyPlayer();
    }

    [Command]
    public void CmdResetRealTeam()
    {
        this.realTeam = 0;
        RpcSetActivePlayer(true);
        GameObject.FindGameObjectWithTag("LobbyManager").GetComponent<LobbyManager>().RemoveReadyPlayer();
    }

    [Command]
    public void CmdSetTeam(int team)
    {
        this.team = team;
    }

    [ClientRpc]
    public void RpcSetActivePlayer(bool active)
    {
        this.gameObject.GetComponent<SpriteRenderer>().enabled = active;
        this.gameObject.GetComponent<Movement>().enabled = active;
        this.gameObject.GetComponent<Dash>().enabled = active;
        this.gameObject.GetComponent<Rigidbody>().useGravity = active;
        this.gameObject.GetComponent<Collider>().enabled = active;
    }


}
