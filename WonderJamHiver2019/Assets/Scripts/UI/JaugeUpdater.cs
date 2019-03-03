using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JaugeUpdater : MonoBehaviour
{
    public Slider slider;

    // Update is called once per frame
    void Update()
    {
        if (PlayerState.singleton.myTeam == 1)
        {
            slider.value = (float)MainGameManager.singleton.teamBase1.currentPression / (float)MainGameManager.singleton.teamBase1.neededPressionToWin;
        }

        else if(PlayerState.singleton.myTeam == 2)
        {
            slider.value = (float)MainGameManager.singleton.teamBase2.currentPression / (float)MainGameManager.singleton.teamBase2.neededPressionToWin;
        }
    }
}
