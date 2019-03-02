using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LobbyManager : NetworkBehaviour
{
    public static LobbyManager singleton;

    public Animator UILaunch;

    private NetworkManager networkManager;

    public TeamLobbyManager teamLobbyManager1;
    public TeamLobbyManager teamLobbyManager2;

    private int countReadyPlayer;

    public int countNeededPlayer;

    void Awake()
    {
        if(null == LobbyManager.singleton){
            LobbyManager.singleton = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        networkManager = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<NetworkManager>();
        countReadyPlayer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (countReadyPlayer==countNeededPlayer)
        {
            if (Input.GetButtonDown("Start"))
            {
                foreach(GameObject go in GameObject.FindGameObjectsWithTag("Player")) {
                    go.GetComponent<Player>().RpcPrepareToStartGame();
                }
            }

            int countReadyPlayerForChangeScene = 0;

            foreach(GameObject go in GameObject.FindGameObjectsWithTag("Player")) {
                if(go.GetComponent<Player>().isReady)
                    countReadyPlayerForChangeScene++;
            }

            if(countReadyPlayerForChangeScene == countNeededPlayer)
            {
                networkManager.ServerChangeScene("MainGame2");
            }
        }
    }

    public void AddReadyPlayer()
    {
        countReadyPlayer++;
        if (countReadyPlayer == countNeededPlayer && isServer)
            UILaunch.SetTrigger("Display");
    }

    public void RemoveReadyPlayer()
    {
        if (countReadyPlayer == countNeededPlayer && isServer)
            UILaunch.SetTrigger("Hide");
        countReadyPlayer--;
    }
}
