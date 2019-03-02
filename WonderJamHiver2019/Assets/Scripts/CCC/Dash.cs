using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Dash : NetworkBehaviour
{
    public float DashSpeed = 500.0f;
    public float DashCooldown = 1.0f;
    public float InputReactivationCooldowwn = 0.5f;
    public float ImpactForce = 600.0f;

    public float StunedDuration = 1.5f;

    float horizontalAxis;
    float verticalAxis;

    float cooldownTimer;
    float inputTimer;
    float startTime;
    float stopTime;

    [Range(0, 1)]
    public float minDashEffect;
    [Range(0, 1)]
    public float maxDashEffect;
    float dashEffectPercentage;

    [SyncVar]
    public bool isDashing;

    Rigidbody rb;
    Movement movement;
    Player player;

    Animator animator;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (!rb)
            Debug.LogWarning("Pas de rigidbody sur le joueur");

        movement = GetComponent<Movement>();
        if (!movement)
            Debug.LogWarning("Pas de movement script sur le joueur");

        player = GetComponent<Player>();
        if (!player)
            Debug.LogWarning("Pas de Player script sur le joueur");

        isDashing = false;

        animator = gameObject.GetComponent<Animator>();
        if (!animator)
            Debug.LogWarning("Pas d'animator sur le joueur");

    }



    void Update()
    {

        horizontalAxis = Input.GetAxis("Horizontal");
        verticalAxis = Input.GetAxis("Vertical");

        if (!isLocalPlayer)
            return;

        if (Input.GetButtonDown("Fire1") && !player.enterTeamZone)
        { 
            StartDash();
        }

        if (Time.time > inputTimer && this.isDashing)
        {
            movement.canMove = true;
            CmdStopDashing();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isServer)
            return;

        if (this.isDashing && collision.gameObject.tag.Equals("Player"))
        {
            this.dashEffectPercentage = Mathf.Clamp( (inputTimer - Time.time) / InputReactivationCooldowwn , minDashEffect, maxDashEffect);
            collision.gameObject.GetComponent<Dash>().RpcCasseToi((collision.transform.position - this.transform.position).normalized, dashEffectPercentage);
            RpcTriggerContactAnimation();
        }
    }



    void StartDash()
    {
        if (Time.time < cooldownTimer)
            return;

        if (horizontalAxis != 0 || verticalAxis != 0)
        {
            CmdStartDashing();
            cooldownTimer = Time.time + DashCooldown;
            inputTimer = Time.time + InputReactivationCooldowwn;
            movement.canMove = false;

        }
    }

    [Command]
    void CmdStartDashing()
    {
        this.isDashing = true;
        RpcImpulseDash();
    }

    [Command]
    void CmdStopDashing()
    {
        this.isDashing = false;
        RpcTriggerStopDashAnimation();
    }

    [ClientRpc]
    void RpcImpulseDash()
    {
        Vector3 direction = rb.velocity.normalized;
        rb.velocity = Vector3.zero;
        rb.AddForce(direction * DashSpeed);
        animator.SetTrigger("startDashing");
        animator.SetBool("isDoneDashing", false);
    }

    [ClientRpc]
    void RpcCasseToi(Vector3 direction, float dashEffectPercentage)
    {
        rb.velocity = Vector3.zero;
        rb.AddForce(direction * ImpactForce * dashEffectPercentage);
        movement.isStunedFor(StunedDuration);
        player.CmdDropRessource();
    }

    [ClientRpc]
    void RpcTriggerContactAnimation()
    {
        animator.SetBool("isDoneDashing", true);
        animator.SetBool("isDoneDashingWithoutContact", false);

    }

    [ClientRpc]
    void RpcTriggerStopDashAnimation()
    {
        animator.SetBool("isDoneDashing", false);

        animator.SetBool("isDoneDashingWithoutContact", true);
    }

    

}
