using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PromptBase : MonoBehaviour
{
    public SpriteRenderer sr1;
    public SpriteRenderer sr2;
    Player player;


    private void Update()
    {
        //transform.LookAt(Camera.main.transform);
    }

    private void OnTriggerEnter(Collider other)
    {
        Player p = other.GetComponent<Player>();

        if (p && p.isLocalPlayer)
        {
            player = p;
            sr1.enabled = true;
            sr2.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Player p = other.GetComponent<Player>();

        if (p && p.isLocalPlayer)
        {
            sr1.enabled = false;
            sr2.enabled = false;
            player = null;
        }
    }
}
