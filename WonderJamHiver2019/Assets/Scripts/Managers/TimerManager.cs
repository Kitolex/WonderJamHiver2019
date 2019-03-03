using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TimerManager : MonoBehaviour
{
    public Text chrono;

    void Update()
    {
        if (MainGameManager.singleton.timeLeft >= 100)
        {
            chrono.text = MainGameManager.singleton.timeLeft + "";
        }
        else if (MainGameManager.singleton.timeLeft >= 10)
        {
            chrono.text = "0" + MainGameManager.singleton.timeLeft;
        }
        else if (MainGameManager.singleton.timeLeft >= 0)
        {
            chrono.text = "00" + MainGameManager.singleton.timeLeft;
        }
        else
        {
            chrono.text = "000";
        }
    }
}