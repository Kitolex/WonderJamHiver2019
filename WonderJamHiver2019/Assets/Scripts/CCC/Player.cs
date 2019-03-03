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

    [SyncVar]
    public int playerID;

    public bool isReady;

    public bool enterTeamZone;

    public bool isInBaseZone;

    [SyncVar]
    private bool isGivingRessource;

    [SyncVar]
    private bool isTakingRessource;

    private float timerInteractionBase;
    public float delaisInteractionBase = 0.1f;
    public int nbRessourcePerInteraction = 1;

    public SpriteRenderer spriteRenderer;
    CapsuleCollider capsuleCollider;

    // Start is called before the first frame update
    void Start()
    {
        team = 0;
        realTeam = 0;
        enterTeamZone = false;

        if(isLocalPlayer && PlayerState.singleton.inGame)
        {
            CmdSpawnMeInMyBase(PlayerState.singleton.myTeam);
            CmdSetTeam(PlayerState.singleton.myTeam);
            CmdSetPlayerID(PlayerState.singleton.playerID);
        }

        capsuleCollider = GetComponent<CapsuleCollider>();
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
                PlayerState.singleton.playerID = 0;
            }
        }

        if(isInBaseZone)
        {
            if (Input.GetButtonDown("Fire3") && !isGivingRessource && !isTakingRessource)
            {
                Debug.Log("Start Give Ressource to Base");
                CmdStartGiveRessourceToBase();
            }
            else if (Input.GetButtonUp("Fire3") && isGivingRessource && !isTakingRessource)
            {
                Debug.Log("Stop Give Ressource to Base");
                CmdStopGiveRessourceToBase();
            }

            if (Input.GetButtonDown("Fire4") && !isGivingRessource && !isTakingRessource)
            {
                Debug.Log("Start Take Ressource from Base");
                CmdStartTakeRessourceFromBase();
            }
            else if (Input.GetButtonUp("Fire4") && !isGivingRessource && isTakingRessource)
            {
                Debug.Log("Stop Take Ressource from Base");
                CmdStopTakeRessourceFromBase();
            }
        }

        if(!isServer)
            return;

        if(isGivingRessource)
        {
            if(Time.time > timerInteractionBase)
            {
                timerInteractionBase = Time.time + delaisInteractionBase;

                int ressourceInThisAction = Mathf.Min(this.ressourceCount, nbRessourcePerInteraction);

                this.ressourceCount -= ressourceInThisAction;

                if(this.team == 1)
                {
                    MainGameManager.singleton.teamBase1.currentPression += ressourceInThisAction;
                }
                if(this.team == 2)
                {
                    MainGameManager.singleton.teamBase2.currentPression += ressourceInThisAction;
                }
            }
        }

        if(isTakingRessource)
        {
            if(Time.time > timerInteractionBase)
            {
                timerInteractionBase = Time.time + delaisInteractionBase;

                int ressourceInThisAction = 0;

                if(this.team == 1)
                {
                    ressourceInThisAction = Mathf.Min(MainGameManager.singleton.teamBase1.currentPression, nbRessourcePerInteraction);
                    MainGameManager.singleton.teamBase1.currentPression -= ressourceInThisAction;
                }
                if(this.team == 2)
                {
                    ressourceInThisAction = Mathf.Min(MainGameManager.singleton.teamBase2.currentPression, nbRessourcePerInteraction);
                    MainGameManager.singleton.teamBase2.currentPression -= ressourceInThisAction;
                }

                this.ressourceCount += ressourceInThisAction;
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
        LobbyManager.singleton.AddReadyPlayer();
        if(team == 1)
        {
            this.playerID = LobbyManager.singleton.teamLobbyManager1.GetMyPlayerIdForThisGame();
            LobbyManager.singleton.teamLobbyManager1.nbInThisTeam++;
        }
        if(team == 2)
        {
            this.playerID = LobbyManager.singleton.teamLobbyManager2.GetMyPlayerIdForThisGame();
            LobbyManager.singleton.teamLobbyManager2.nbInThisTeam++;
        }
        //GetComponentInChildren<SpriteRenderer>().material.SetInt("_Team", team);
    }

    [Command]
    public void CmdResetRealTeam()
    {
        this.realTeam = 0;
        RpcSetActivePlayer(true);
        LobbyManager.singleton.RemoveReadyPlayer();
        if(team == 1)
            LobbyManager.singleton.teamLobbyManager1.nbInThisTeam--;
        if(team == 2)
            LobbyManager.singleton.teamLobbyManager2.nbInThisTeam--;
        this.playerID = 0;
        //GetComponentInChildren<SpriteRenderer>().material.SetInt("_Team", team);
    }

    [Command]
    public void CmdSetTeam(int team)
    {
        this.team = team;
        RpcUpdateTeamColorOnPlayer(team);
    }

    [ClientRpc]
    public void RpcUpdateTeamColorOnPlayer(int team)
    {
        GetComponentInChildren<SpriteRenderer>().material.SetInt("_Team", team);
    }

    [Command]
    public void CmdSetPlayerID(int id)
    {
        this.playerID = id;
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
        {
            PlayerState.singleton.playerID = this.playerID;
            CmdIsReady();
        }
    }

    [Command]
    private void CmdIsReady()
    {
        isReady = true;
    }

    [ClientRpc]
    public void RpcPrepareToEndGame(int scoreTeam1, int scoreTeam2)
    {
        PlayerState.singleton.inGame = false;
        PlayerState.singleton.inEndGame = true;
        if(isLocalPlayer)
            CmdIsReady();
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
        isReady = false;
    }

    [Command]
    private void CmdStartGiveRessourceToBase()
    {
        isGivingRessource = true;
    }

    [Command]
    private void CmdStopGiveRessourceToBase()
    {
        isGivingRessource = false;
    }

    [Command]
    private void CmdStartTakeRessourceFromBase()
    {
        isTakingRessource = true;
    }

    [Command]
    private void CmdStopTakeRessourceFromBase()
    {
        isTakingRessource = false;
    }

    [Command]
    public void CmdDropRessource()
    {
        if (ressourceCount <= ressourcePrefab.GetComponent<Ressource>().nbPressionGive)
            return;


        GameObject instance = Instantiate(ressourcePrefab);
        instance.transform.position = this.transform.position;
        if (capsuleCollider)
        {
            instance.transform.position += new Vector3(0, capsuleCollider.height, 0);
        }
        NetworkServer.Spawn(instance);
        
        this.ressourceCount -= ressourcePrefab.GetComponent<Ressource>().nbPressionGive;
    }
}
