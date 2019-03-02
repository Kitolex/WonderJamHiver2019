using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LobbyManager : NetworkBehaviour
{

    public Animator UILaunch;

    private NetworkManager networkManager;


    private int countReadyPlayer;

    public int countNeededPlayer;

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
