using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PublicNetworkManager : MonoBehaviour
{
    private const string URL_VOTE = "https://kgames.valereplantevin.ca/api/vote";
    private const string URL_CREATE_PARTIE = "https://kgames.valereplantevin.ca/api/create";
    private const string URL_DELETE_PARTIE = "https://kgames.valereplantevin.ca/api/partie";
    private const string URL_LAUNCH_PARTIE = "https://kgames.valereplantevin.ca/api/launch";

    private NetworkManager networkManager;

    public string namePartie;

    public static PublicNetworkManager singleton;

    public bool active;



    private bool activeTimerForPublicToPlay;

    public float minTimeToPublicPlay;

    private float timeLeftToPublicPlay;



    private bool activeTimerBetween;

    public float timeBetweenEvent;

    private float timeLeftBetweenEvent;




    private bool activeTimerGetter;

    public float timeGetter;

    private float timeLeftGetter;


    private int playerID;
    private string eventPublic;

    private Text publicVow;


    public void Awake()
    {
        if (null == PublicNetworkManager.singleton)
        {
            PublicNetworkManager.singleton = this;
        }
        DontDestroyOnLoad(this.gameObject);
        active = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        // StartCoroutine(CreatePartie("Partie1")); 
        //StartCoroutine(GetChoicePublic("Partie1")); 
        //StartCoroutine(DeleteParty("Partie1"));
        networkManager = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<NetworkManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(PlayerState.singleton.inGame && publicVow == null)
            publicVow = GameObject.Find("PublicVow").GetComponent<Text>();

        if (activeTimerForPublicToPlay)
        {
            timeLeftToPublicPlay -= Time.deltaTime;
            if (timeLeftToPublicPlay < 0)
            {

                Debug.Log("A new Challenger In Comming");
                StartCoroutine(ShowMessage("!! WARNING !! I sense a greater presence", new Color32(253, 165, 25, 255), 3));
               
                StartCoroutine(LaunchPartie(namePartie));                          
                activeTimerForPublicToPlay = false;
            }
        }

        if (activeTimerBetween)
        {
            timeLeftBetweenEvent -= Time.deltaTime;
            if (timeLeftBetweenEvent < 0)
            {
                Debug.Log("GO EVENT");
                activeTimerBetween = false;
                ApplyEventOnPlayer();
                
            }
        }

        if (activeTimerGetter)
        {
            timeLeftGetter -= Time.deltaTime;
            if (timeLeftGetter < 0)
            {
                Debug.Log("GO Get choice");
                StartCoroutine(GetChoicePublic(namePartie));
                activeTimerGetter = false;
            }
        }

    }

    IEnumerator ShowMessage(string message, Color32 color, float delay)
    {
        publicVow.text = message;
        publicVow.color = color;
        publicVow.enabled = true;
        yield return new WaitForSeconds(delay);
        publicVow.enabled = false;
    }

    public void ActiveLoop()
    {
        timeLeftToPublicPlay = minTimeToPublicPlay;
        activeTimerForPublicToPlay = true;
    }

    public void GestionLoop()
    {
        Debug.Log("DebutLoop");
        StartCoroutine(GetChoicePublic(namePartie));
    }

    public void ActiveEvent(string eventPublic)
    {
        Debug.Log("Debut Syteme : " + eventPublic);
        timeLeftBetweenEvent =timeBetweenEvent ;
        activeTimerBetween = true;

        string[] action = eventPublic.Split(':');

        playerID = int.Parse(action[0].Substring(action[0].Length - 1));
        Debug.Log(playerID);
        WarnPlayer(playerID);
        this.eventPublic = action[1];
        Debug.Log(this.eventPublic);

    }

    private void WarnPlayer(int playerID)
    {
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Player"))
        {
            Player p = go.GetComponent<Player>();
            if (p.playerID == this.playerID)
            {
                
               StartCoroutine(ShowMessage("!! DANGER !! Something incoming...", new Color32(200, 45, 45, 255), 4));
                
            }
        }
    }

    public void ApplyEventOnPlayer()
    {
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Player")) {
            Player p = go.GetComponent<Player>();
            if (p.playerID==this.playerID)
            {
                if (eventPublic.Equals("Reverse"))
                {
                    p.RpcStartBeingHacked();
                }
                else if (eventPublic.Equals("Stun"))
                {
                    p.RpcReceivedLightning();
                }
                GestionLoop();
            }
        }
    }


    public IEnumerator CreatePartie(string namePartie)
    {
        string URL = URL_CREATE_PARTIE + "/" +namePartie;
        UnityWebRequest www = UnityWebRequest.Post(URL,"");
        yield return www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            this.namePartie = namePartie;
            networkManager.StartHost();
        }
    }

    public IEnumerator LaunchPartie(string namePartie)
    {
        string URL = URL_LAUNCH_PARTIE + "/" + namePartie;
        UnityWebRequest www = UnityWebRequest.Post(URL, "");
        yield return www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            GestionLoop();
        }
    }

    IEnumerator GetChoicePublic(string namePartie)
    {
        string URL = URL_VOTE+"/"+ namePartie;
        Debug.Log(URL);
        UnityWebRequest www = UnityWebRequest.Get(URL);
        yield return www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log("Rien Trouve comme Choix");
            timeLeftGetter = timeGetter;
            activeTimerGetter = true;
        }
        else
        {
            ActiveEvent(www.downloadHandler.text);
        }

    }

    IEnumerator DeleteParty(string namePartie)
    {
        string URL = URL_DELETE_PARTIE + "/" + namePartie;
        UnityWebRequest www = UnityWebRequest.Delete(URL);
        yield return www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log("ERROR");
        }
        else
        {
            Debug.Log("OK");
        }

    }
}


