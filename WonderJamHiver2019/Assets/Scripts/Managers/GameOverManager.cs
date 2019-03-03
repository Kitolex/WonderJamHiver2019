using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GameOverManager : NetworkBehaviour
{
    private float scoreTeam1;
    private float scoreTeam2;
    public Text whoWon;
    private NetworkManager networkManager;


    private void Awake()
    {
        networkManager = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<NetworkManager>();

        networkManager.autoCreatePlayer = false;

    }
    // Start is called before the first frame update
    void Start()
    {
       // networkManager = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<NetworkManager>();

        scoreTeam1 = PlayerState.singleton.scoreTeam1;
        scoreTeam2 = PlayerState.singleton.scoreTeam2;
    }

    // Update is called once per frame
    void Update()
    {

        if (scoreTeam1 > scoreTeam2)
        {
            whoWon.text = "RED team won!";
            whoWon.color = new Color32(168, 40, 41, 255);
        }
        if (scoreTeam1 < scoreTeam2) { 
            whoWon.text = "Green team won!";
            whoWon.color = new Color32(104, 150, 66, 255);
        }

        else { 
            whoWon.text = "It's a TIE!";
            whoWon.color = Color.white;
        }
    }

    public void RetourMenuPrincipal()
    {
        networkManager.autoCreatePlayer = true;

        if (isServer)
            networkManager.StopHost();
        if(isClient)
            networkManager.StopClient();
    }
}
