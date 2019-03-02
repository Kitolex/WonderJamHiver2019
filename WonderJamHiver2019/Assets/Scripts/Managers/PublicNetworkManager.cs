using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PublicNetworkManager : MonoBehaviour
{
    private const string URL_VOTE = "http://localhost:5000/api/vote";
    private const string URL_CREATE_PARTIE = "http://localhost:5000/api/create";

    // Start is called before the first frame update
    void Start()
    {
       // StartCoroutine(CreatePartie("Partie1"));   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator CreatePartie(string namePartie)
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
            Debug.Log("Form upload complete!");
        }


    }
}

public class PartieItem
{
    public string namePartie;
}
