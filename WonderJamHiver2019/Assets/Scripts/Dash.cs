using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Dash : NetworkBehaviour
{
    public float DashDistance;
    public float DashForce;
    public float DashCooldown = 1f;
    public float ImpactForce = 50f;

    float horizontalAxis;
    float verticalAxis;

    float cooldownTimer;
    float startTime;

    float distanceTraveled;

    bool isDashing;
    bool shouldKeepDashing;

    List<GameObject> memoryHit;

    Vector3 initialPosition;
    Vector3 dashDirection;
    Vector3 computedDashForce;

    Rigidbody rb;
    Movement movement;



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (!rb)
            Debug.Log("Pas de rigidbody sur le joueur");

        movement = GetComponent<Movement>();
        if (!movement)
            Debug.LogWarning("Pas de movement script sur le joueur");

        memoryHit = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer)
            return;

        HandleInput();
    }

    void HandleInput()
    {
        horizontalAxis = Input.GetAxis("Horizontal");
        verticalAxis = Input.GetAxis("Vertical");

        if (Input.GetButtonDown("Fire1"))
            StartDash();
    }

    void StartDash()
    {
        if (Time.time < cooldownTimer)
            return;
        if(horizontalAxis!=0 || verticalAxis!=0)
        {
            cooldownTimer = Time.time + DashCooldown;
            isDashing = true;

            memoryHit.Clear();

            movement.canMove = false;

            StartCoroutine(DashCoroutine());
        }
    
    }

    public void StopDash()
    {
        isDashing = false;
        movement.canMove = true;
    }

    IEnumerator DashCoroutine()
    {
        if(!isDashing)
            yield break;

        // we initialize our various counters and checks
        startTime = Time.time;
        initialPosition = this.transform.position;
        distanceTraveled = 0;
        shouldKeepDashing = true;
        dashDirection = new Vector3(horizontalAxis, 0, verticalAxis).normalized;
        computedDashForce = DashForce * dashDirection;

        // we keep dashing until we've reached our target distance or until we get interrupted
        while (distanceTraveled < DashDistance && shouldKeepDashing)
        {
            distanceTraveled = Vector3.Distance(initialPosition, this.transform.position);
            rb.velocity = computedDashForce;
            yield return null;
        }

        StopDash();
    }

    private void OnCollisionStay(Collision collision)
    {
        if (!isDashing)
            return;

        if (collision.gameObject.tag != "Player")
            return;

        if (!isServer)
            return;

        if (!memoryHit.Contains(collision.gameObject))
        {
            collision.gameObject.GetComponent<Dash>().RpcGetDashed(collision.gameObject.transform.position - transform.position);
            memoryHit.Add(collision.gameObject);
        }
        
    }

    [ClientRpc]
    public void RpcGetDashed(Vector3 directionImpulse)
    {
        directionImpulse.Normalize();
        rb.velocity = directionImpulse * ImpactForce;
        Debug.Log("Test");
    }
}
