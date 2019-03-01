﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{

    public GameObject MainMenuPanel;
    public GameObject CreationPartiePanel;
    public GameObject RejoindrePartiePanel;


    // Start is called before the first frame update
    void Start()
    {
        
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

    public void RejoindrePartie()
    {
        MainMenuPanel.SetActive(false);
        CreationPartiePanel.SetActive(false);

        RejoindrePartiePanel.SetActive(true);


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