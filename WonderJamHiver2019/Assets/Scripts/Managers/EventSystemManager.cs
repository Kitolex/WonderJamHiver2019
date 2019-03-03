using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class EventSystemManager : MonoBehaviour
{
    public EventSystem ES;
    public GameObject firstSelected;

    // Start is called before the first frame update
    public void Setup()
    {
        ES.firstSelectedGameObject = firstSelected;
        ES.SetSelectedGameObject(firstSelected);
    }

    // Update is called once per frame
    void Update()
    {
        if (ES.currentSelectedGameObject == null)
            ES.SetSelectedGameObject(firstSelected);
    }
}
