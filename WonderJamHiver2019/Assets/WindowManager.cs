using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowManager : MonoBehaviour
{
    public GameObject firstWindow ;
    public GameObject secondWindow ;
    public GameObject thirdWindow ;
    public GameObject lastWindow ;


    // Start is called before the first frame update
    void Start()
    {
        firstWindow.SetActive( false);
        secondWindow.SetActive(false);
        thirdWindow.SetActive(false);
        lastWindow.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
