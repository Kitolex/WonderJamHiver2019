using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class MainMenuManager : MonoBehaviour
{

    public GameObject MainMenuPanel;
    public GameObject CreationPartiePanel;
    public GameObject RejoindrePartiePanel;

    public InputField adresseServer;
    public InputField namePartie;

    private NetworkManager networkManager;




    // Start is called before the first frame update
    void Start()
    {
        networkManager = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<NetworkManager>();   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreerPartie()
    {
        MainMenuPanel.SetActive(false);
        RejoindrePartiePanel.SetActive(false);

        CreationPartiePanel.SetActive(true);

        
    }

    public void CreatePartie()
    {
        PublicNetworkManager.singleton.StartCoroutine(PublicNetworkManager.singleton.CreatePartie(namePartie.text));
    }
    public void HostPartie()
    {
        networkManager.StartHost();
    }


    public void RejoindrePartie()
    {
        MainMenuPanel.SetActive(false);
        CreationPartiePanel.SetActive(false);

        RejoindrePartiePanel.SetActive(true);
    }

    public void StartClient()
    {
        networkManager.networkAddress = this.adresseServer.text;
        if(this.adresseServer.text.Equals(""))
            networkManager.networkAddress = "localhost";
        networkManager.StartClient();
    }

    public void Retour()
    {
        CreationPartiePanel.SetActive(false);
        RejoindrePartiePanel.SetActive(false);

        MainMenuPanel.SetActive(true);
    }

    public void Quitter()
    {
#if UNITY_EDITOR
        // Application.Quit() does not work in the editor so
        // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }
}
