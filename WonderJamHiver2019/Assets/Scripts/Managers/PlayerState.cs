using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public static PlayerState singleton;

    public int myTeam;
    public int playerID;

    public bool inGame;
    public bool inEndGame;
    public int scoreTeam1;
    public int scoreTeam2;

    public Base myBase;

    void Awake()
    {
        if(null == PlayerState.singleton){
            PlayerState.singleton = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        myTeam = 0;
        inGame = false;
        inEndGame = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
