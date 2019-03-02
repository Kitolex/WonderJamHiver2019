﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameManager : MonoBehaviour
{
    public static MainGameManager singleton;

    public List<Transform> team1StartPositions;
    public List<Transform> team2StartPositions;

    
    public Base teamBase1; // A renseigner dans la scene
    public Base teamBase2; // A renseigner dans la scene

    private int nextSpawnTeam1;
    private int nextSpawnTeam2;

    void Awake()
    {
        if(null == MainGameManager.singleton){
            MainGameManager.singleton = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        nextSpawnTeam1 = 0;
        nextSpawnTeam2 = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(teamBase1.currentPression >= teamBase1.neededPressionToWin)
        {
            // TEAM 1 WIN
        }

        else if(teamBase2.currentPression >= teamBase2.neededPressionToWin)
        {
            // TEAM 2 WIN
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
}
