﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Events;

public class MainGameManager : NetworkBehaviour
{
    public static MainGameManager singleton;

    public List<Transform> team1StartPositions;
    public List<Transform> team2StartPositions;

    
    public Base teamBase1; // A renseigner dans la scene
    public Base teamBase2; // A renseigner dans la scene

    private int nextSpawnTeam1;
    private int nextSpawnTeam2;

    public int countNeededPlayer = 2;
    private NetworkManager networkManager;

    [SyncVar]
    public int timeLeft;

    private int timeLeftTmp;
    private float startTime;
    public int matchDuration = 300;

    bool endReached = false;

    void Awake()
    {
        if(null == MainGameManager.singleton){
            MainGameManager.singleton = this;
        }

        if (!isServer)
            return;

        this.timeLeft = matchDuration;
    }

    // Start is called before the first frame update
    void Start()
    {
        networkManager = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<NetworkManager>();
        nextSpawnTeam1 = 0;
        nextSpawnTeam2 = 0;
        if (PublicNetworkManager.singleton.active  && isServer)
        {
            PublicNetworkManager.singleton.ActiveLoop();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!isServer)
            return;

        this.timeLeftTmp = (int)Mathf.Floor(matchDuration + (startTime - Time.time));
        updateTimeLeft();

        if (timeLeft <= 0)
        {
            EndGameEvent endGameEvent = new EndGameEvent(0);
            if (teamBase1.currentPression > teamBase2.currentPression)
            {
                endGameEvent = new EndGameEvent(1);
            }
            else if (teamBase2.currentPression > teamBase1.currentPression)
            {
                endGameEvent = new EndGameEvent(2);
            }

            EventManager.TriggerEvent<EndGameEvent>(endGameEvent);
            foreach (GameObject go in GameObject.FindGameObjectsWithTag("Player"))
            {
                go.GetComponent<Player>().RpcPrepareToEndGame(teamBase1.currentPression, teamBase2.currentPression);
            }

            endReached = true;
        }

        if (!endReached)
        {
            if (teamBase1.currentPression >= teamBase1.neededPressionToWin)
            {
                EventManager.TriggerEvent<EndGameEvent>(new EndGameEvent(1));
                endReached = true;
                foreach (GameObject go in GameObject.FindGameObjectsWithTag("Player"))
                {
                    go.GetComponent<Player>().RpcPrepareToEndGame(teamBase1.currentPression, teamBase2.currentPression);
                }
                StartCoroutine(EndGameCoroutine());
            }

            else if (teamBase2.currentPression >= teamBase2.neededPressionToWin)
            {
                EventManager.TriggerEvent<EndGameEvent>(new EndGameEvent(2));
                endReached = true;
                foreach (GameObject go in GameObject.FindGameObjectsWithTag("Player"))
                {
                    go.GetComponent<Player>().RpcPrepareToEndGame(teamBase1.currentPression, teamBase2.currentPression);
                }
                StartCoroutine(EndGameCoroutine());
            }

            int countReadyPlayerForChangeScene = 0;

            foreach (GameObject go in GameObject.FindGameObjectsWithTag("Player"))
            {
                if (go.GetComponent<Player>().isReady)
                    countReadyPlayerForChangeScene++;
            }

            if (countReadyPlayerForChangeScene == countNeededPlayer) // !!!!!!!! AJOUTER UN DELAIS POUR VOIR L'ANIMATION DE LA FUSEE !!!!!!!!!!!!!!!!!!!!!!!! 
            {
                networkManager.ServerChangeScene("SceneFin");
            }
        }
        

       
    }

    public Vector3 GetSpawnPosition(int team)
    {
        Vector3 result = Vector3.zero;

        if(team == 1)
        {
            result = team1StartPositions[nextSpawnTeam1].position;
            nextSpawnTeam1 = (nextSpawnTeam1 + 1) % team1StartPositions.Count;
        }
        
        if(team == 2)
        {
            result = team2StartPositions[nextSpawnTeam2].position;
            nextSpawnTeam2 = (nextSpawnTeam2 + 1) % team2StartPositions.Count;
        }
        return result;
    }


    IEnumerator EndGameCoroutine()
    {
        yield return new WaitForSeconds(10);
        networkManager.ServerChangeScene("SceneFin");
    }

    public void updateTimeLeft()
    {
        if (this.timeLeft == this.timeLeftTmp)
            return;

        this.timeLeft = this.timeLeftTmp;
    }
}
