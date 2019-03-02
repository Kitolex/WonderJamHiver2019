using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Movement : NetworkBehaviour
{
    float horizontalAxis;
    float verticalAxis;

    [Range(1,10)]
    public float movementSpeed;
    public float acceleration;
    public float inputMinThreshold = 0.1f;
    public float minSpeedWhenInput = 0.8f;

    Rigidbody rb;

    public bool canMove = true;

    float stunTimer;
    bool isStuned;

    Animator animator;
    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (!rb)
            Debug.LogWarning("Pas de rigidbody sur le joueur");

        spriteRenderer = GetComponent<SpriteRenderer>();

        animator = gameObject.GetComponent<Animator>();
        if (!animator)
            Debug.LogWarning("Pas d'animator sur le joueur");

    }

    // Update is called once per frame
    void Update()
    {
       if (!isLocalPlayer)
        {
            return;
        }           

        GetAxis();
        ApplyMovement();

        if(Time.time > stunTimer && isStuned)
        {
            isStuned = false;
            canMove = true;
        }
    }

    void GetAxis()
    {
        horizontalAxis = Input.GetAxis("Horizontal");
        verticalAxis = Input.GetAxis("Vertical");
    }

    void ApplyMovement()
    {
        if (!canMove)
            return;

        Vector2 movement = new Vector2(horizontalAxis, verticalAxis);

        if(movement.magnitude < inputMinThreshold)
        {
            CmdUpdateMovementEffect(0.0f, horizontalAxis < 0);
            return;
        }

        if(movement.magnitude > 1.0f)
            movement.Normalize();

       CmdUpdateMovementEffect(movement.magnitude, horizontalAxis < 0);

        float magnitude = Mathf.Lerp(rb.velocity.magnitude, movementSpeed, Time.deltaTime * acceleration);

        magnitude = Mathf.Max(magnitude, minSpeedWhenInput);

        this.rb.velocity = new Vector3(movement.x * magnitude, rb.velocity.y , movement.y * magnitude);
    }

    public void disableMove()
    {
        canMove = false;
    }

    public void enableMove()
    {
        canMove = true;
    }

    public void isStunedFor(float stunedTime)
    {
        canMove = false;
        isStuned = true;
        stunTimer = Time.time + stunedTime;
    }

    [Command]
    public void CmdUpdateMovementEffect(float speed, bool flipX)
    {
        RpcUpdateMovementEffect(speed, flipX);
    }

    [ClientRpc]
    public void RpcUpdateMovementEffect(float speed, bool flipX)
    {
        animator.SetFloat("Speed", speed);
        if(speed > 0.01f)
        {
            spriteRenderer.flipX = flipX;
        }
    }
}
