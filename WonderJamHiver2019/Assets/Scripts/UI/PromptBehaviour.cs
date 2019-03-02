using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PromptBehaviour : MonoBehaviour
{
    SpriteRenderer sr;
    Player player;
    public Sprite AButton;
    public Sprite BButton;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }


    private void Update()
    {
        transform.LookAt(Camera.main.transform);
        if (player)
        {
            if (player.realTeam == 0)
                sr.sprite = AButton;
            else
                sr.sprite = BButton;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Player p = other.GetComponent<Player>();

        if(p && p.isLocalPlayer)
        {
            player = p;
            sr.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Player p = other.GetComponent<Player>();

        if (p && p.isLocalPlayer)
        {
            sr.enabled = false;
            player = null;
        }
    }
}
