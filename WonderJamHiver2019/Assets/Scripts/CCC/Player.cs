using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Events;

public class Player : NetworkBehaviour
{
    public int maxRessources = 5;
    public int nbRessourceLostByHit = 1;

    public GameObject ressourcePrefab;

    [SyncVar]
    public int ressourceCount;

    public int maxRessourceCount = 100;

    [SyncVar]
    public int team;

    [SyncVar]
    public int realTeam;

//    [SyncVar]
    public bool isReady;

    public bool enterTeamZone;

    public SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        team = 0;
        realTeam = 0;
        enterTeamZone = false;

        if(isLocalPlayer && PlayerState.singleton.inGame)
        {
            CmdSpawnMeInMyBase(PlayerState.singleton.myTeam);
        }
    }

    public override void OnStartLocalPlayer()
    {
        Camera.main.GetComponent<CameraBehaviour>().AssignPlayer(gameObject);
        EventManager.TriggerEvent<LocalPlayerStartEvent>(new LocalPlayerStartEvent(gameObject));
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
                PlayerState.singleton.myTeam = team;
            }

            if ((Input.GetButtonDown("Fire2") || Input.GetKeyDown(KeyCode.R)) && this.realTeam != 0)
            {
                Debug.Log("Suprresion Equipe");
                CmdResetRealTeam();
                PlayerState.singleton.myTeam = 0;
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
        this.gameObject.GetComponentInChildren<SpriteRenderer>().enabled = active;
        this.gameObject.GetComponent<Movement>().enabled = active;
        this.gameObject.GetComponent<Dash>().enabled = active;
        this.gameObject.GetComponent<Rigidbody>().useGravity = active;
        this.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        this.gameObject.GetComponent<Collider>().enabled = active;
    }

    [ClientRpc]
    public void RpcPrepareToStartGame()
    {
        PlayerState.singleton.inGame = true;
        if(isLocalPlayer)
            CmdIsReady();
    }

    [Command]
    private void CmdIsReady()
    {
        isReady = true;
    }

    [Command]
    private void CmdSpawnMeInMyBase(int team)
    {
        RpcSpawnAtPosition(MainGameManager.singleton.GetSpawnPosition(team));
    }

    [ClientRpc]
    private void RpcSpawnAtPosition(Vector3 position)
    {
        this.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        this.transform.position = position;
    }

    [Command]
    public void CmdDropRessource()
    {
        if (ressourceCount <= 0)
            return;

        GameObject instance = Instantiate(ressourcePrefab);
        instance.transform.position = this.transform.position;
        NetworkServer.Spawn(instance);
    }
}
