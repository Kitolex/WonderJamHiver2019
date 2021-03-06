﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Events;
using Random = UnityEngine.Random;

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

    public AudioSource audioSourceRessourceCollect;
    public AudioSource audioSourceRessourceGive;
    public AudioSource audioSourceRessourceTake;
    public AudioSource audioSourceHackDuration;
    public float stundDurationLightning = 1.0f;
    public int nbRessourceCapsuleDropWhenLightning = 3;
    public GameObject lightningPrefabFX;
    public AudioClip lightningSound;

    [Header("Sounds")]
    public float volumePick = 0.15f;
    public AudioClip pick1;
    public AudioClip pick2;
    public float volumeGiveToBase = 0.2f;
    public float volumeTakeFromBase = 0.3f;
    private float velocityVolumeGiveToBase = 0.0f;
    private float velocityVolumeTakeFromBase = 0.0f;
    private float smoothTime = 0.3f;

    // Start is called before the first frame update
    void Start()
    {
        enterTeamZone = false;

        if(isLocalPlayer && PlayerState.singleton.inGame)
        {
            CmdSpawnMeInMyBase(PlayerState.singleton.myTeam);
            CmdSetTeam(PlayerState.singleton.myTeam);
            CmdSetPlayerID(PlayerState.singleton.playerID);
        }

        capsuleCollider = GetComponent<CapsuleCollider>();
        
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    public override void OnStartLocalPlayer()
    {
        Camera.main.GetComponent<CameraBehaviour>().AssignPlayer(gameObject);
        EventManager.TriggerEvent<LocalPlayerStartEvent>(new LocalPlayerStartEvent(gameObject));
        GetComponent<AudioListener>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        audioSourceRessourceGive.volume = Mathf.SmoothDamp(audioSourceRessourceGive.volume, isGivingRessource ? volumeGiveToBase : 0.0f, ref velocityVolumeGiveToBase, smoothTime);
        audioSourceRessourceTake.volume = Mathf.SmoothDamp(audioSourceRessourceTake.volume, isTakingRessource ? volumeTakeFromBase : 0.0f, ref velocityVolumeTakeFromBase, smoothTime);
        
        if (enterTeamZone)
        {
            if ((Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.E)) && this.realTeam == 0)
            {
                this.realTeam = -1; // set dirty pour éviter le spam, sera ecrasé par le serveur
                CmdSetRealTeam(team);
                PlayerState.singleton.myTeam = team;
            }

            if ((Input.GetButtonDown("Fire2") || Input.GetKeyDown(KeyCode.R)) && this.realTeam != 0)
            {
                CmdResetRealTeam();
                PlayerState.singleton.myTeam = 0;
                PlayerState.singleton.playerID = 0;
            }
        }

        if(isLocalPlayer)
        {
            if(isInBaseZone)
            {
                if (Input.GetButtonDown("Fire3") && !isGivingRessource && !isTakingRessource)
                {
                    Debug.Log("zqsdefhujklmp");
                    CmdStartGiveRessourceToBase();
                }
                else if (Input.GetButtonUp("Fire3") && isGivingRessource && !isTakingRessource)
                {
                    CmdStopGiveRessourceToBase();
                }

                if (Input.GetButtonDown("Fire4") && !isGivingRessource && !isTakingRessource)
                {
                    CmdStartTakeRessourceFromBase();
                }
                else if (Input.GetButtonUp("Fire4") && !isGivingRessource && isTakingRessource)
                {
                    CmdStopTakeRessourceFromBase();
                }

                if(ressourceCount == 0 && isGivingRessource)
                {
                    CmdStopGiveRessourceToBase();
                }

                if(PlayerState.singleton.myBase.currentPression == 0 && isTakingRessource)
                {
                    CmdStopTakeRessourceFromBase();
                }
            }
            else
            {
                if(isGivingRessource)
                {
                    isGivingRessource = false;
                    CmdStopGiveRessourceToBase();
                }
                if(isTakingRessource)
                {
                    isTakingRessource = false;
                    CmdStopTakeRessourceFromBase();
                }
            }
        }

        spriteRenderer.material.SetInt("_Team", team);

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
        //EFFET SONOR ET VISUEL
        audioSourceRessourceCollect.PlayOneShot(UnityEngine.Random.Range(0.0f, 1.0f) > 0.5f ? pick1 : pick2, volumePick * UnityEngine.Random.Range(0.7f, 0.9f));
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
        spriteRenderer.material.SetInt("_Team", team);
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
        {
            PlayerState.singleton.scoreTeam1 = scoreTeam1;
            PlayerState.singleton.scoreTeam2 = scoreTeam2;
            CmdIsReady();
        }
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
        DropRessource(1);
    }

    [Command]
    public void CmdDropMultipleRessource(int nb)
    {
        DropRessource(nb);
    }

    public void DropRessource(int nb)
    {
        for(int i=0; i<nb; i++)
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

    [ClientRpc]
    public void RpcStartBeingHacked()
    {
        Movement movement = GetComponent<Movement>();
        movement.isHackedFor(6.5f);

        if(!isLocalPlayer)
            return;

        audioSourceHackDuration.Play();
    }

    [ClientRpc]
    public void RpcReceivedLightning()
    {
        Movement movement = GetComponent<Movement>();
        movement.isStunedFor(stundDurationLightning);
        CmdDropMultipleRessource(nbRessourceCapsuleDropWhenLightning);
        Instantiate(lightningPrefabFX, this.transform.position, Quaternion.identity);
        audioSourceRessourceCollect.PlayOneShot(lightningSound);

        if(isLocalPlayer)
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.velocity = Vector3.zero;
            rb.AddForce(Vector3.left * Random.Range(-50.0f, 50.0f) + Vector3.forward * Random.Range(-50.0f, 50.0f), ForceMode.Impulse);
        }
    }
}
