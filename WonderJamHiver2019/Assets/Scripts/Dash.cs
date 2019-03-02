using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Dash : NetworkBehaviour
{
    public float DashTime = 1000f;
    public float DashSpeed;
    public float DashCooldown = 1f;
    public float InputReactivationCooldowwn = .5f;
    public float ImpactForce = 350f;

    public float StunedDuration = 1f;

    float horizontalAxis;
    float verticalAxis;

    float cooldownTimer;
    float inputTimer;
    float startTime;
    float stopTime;

    [SyncVar]
    public bool isDashing;

    Rigidbody rb;
    Movement movement;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (!rb)
            Debug.Log("Pas de rigidbody sur le joueur");

        movement = GetComponent<Movement>();
        if (!movement)
            Debug.LogWarning("Pas de movement script sur le joueur");

        isDashing = false;
    }



    void Update()
    {

        horizontalAxis = Input.GetAxis("Horizontal");
        verticalAxis = Input.GetAxis("Vertical");

        if (!isLocalPlayer)
            return;

        if (Input.GetButtonDown("Fire1"))
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
            collision.gameObject.GetComponent<Dash>().RpcCasseToi((collision.transform.position - this.transform.position).normalized);
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
    }

    [ClientRpc]
    void RpcImpulseDash()
    {
        Vector3 direction = rb.velocity.normalized;
        rb.velocity = Vector3.zero;
        rb.AddForce(direction * DashSpeed);
    }

    [ClientRpc]
    void RpcCasseToi(Vector3 direction)
    {
        rb.velocity = Vector3.zero;
        rb.AddForce(direction * ImpactForce);
        movement.isStunedFor(StunedDuration);
    }
}
