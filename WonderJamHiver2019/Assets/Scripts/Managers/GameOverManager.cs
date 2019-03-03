using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GameOverManager : NetworkBehaviour
{
    private float scoreTeam1;
    private float scoreTeam2;
    public Text whoWon;
    public Text scoreRed;
    public Text scoreGreen;

    private NetworkManager networkManager;
    private EventSystem ES;
    public GameObject menuBtn;

    private void Awake()
    {
        networkManager = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<NetworkManager>();

        networkManager.autoCreatePlayer = false;

    }
    // Start is called before the first frame update
    void Start()
    {

        scoreTeam1 = PlayerState.singleton.scoreTeam1;
        scoreTeam2 = PlayerState.singleton.scoreTeam2;

        ES = GameObject.FindObjectOfType<EventSystem>();
        if (ES.firstSelectedGameObject == null || ES.currentSelectedGameObject == null )
            ES.firstSelectedGameObject = menuBtn;
    }

    // Update is called once per frame
    void Update()
    {
        scoreRed.text = scoreTeam1.ToString();
        scoreGreen.text = scoreTeam2.ToString();

        if (scoreTeam1 > scoreTeam2)
        {
            whoWon.text = "RED team won!";
            whoWon.color = new Color32(168, 40, 41, 255);
            Debug.Log("RED");
        }
        else if (scoreTeam1 < scoreTeam2) { 
            whoWon.text = "Green team won!";
            whoWon.color = new Color32(104, 150, 66, 255);
            Debug.Log("GREEN");

        }
        else { 
            whoWon.text = "It's a TIE!";
            whoWon.color = Color.white;
            Debug.Log("TIE");

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
