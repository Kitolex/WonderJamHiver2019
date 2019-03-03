using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Movement : NetworkBehaviour
{
    float horizontalAxis;
    float verticalAxis;

    [Range(1,30)]
    public float movementSpeed;
    public float acceleration;
    public float inputMinThreshold = 0.1f;
    public float minSpeedWhenInput = 0.8f;

    [Header("Multiplicateur selon % pression")]

    public float speedMul_0_25 = 0.96f;
    public float speedMul_25_50 = 0.985f;
    public float speedMul_50_75 = 1.0f;
    public float speedMul_75_100 = 1.035f;

    Rigidbody rb;

    public bool canMove = true;

    float stunTimer;
    bool isStuned;
    public GameObject stundEffect;

    private float hackTimer;
    public bool isHacked;
    public GameObject hackEffectStart;
    public GameObject hackEffectDuration;

    public Animator animator;
    public SpriteRenderer spriteRenderer;
    private Player player;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (!rb)
            Debug.LogWarning("Pas de rigidbody sur le joueur");

        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        player = GetComponent<Player>();

    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time > stunTimer && isStuned)
        {
            isStuned = false;
            canMove = true;
            stundEffect.SetActive(false);
        }

        if(Time.time > hackTimer && isHacked)
        {
            isHacked = false;
            hackEffectDuration.SetActive(false);
            player.audioSourceHackDuration.Stop();
        }

        if (!isLocalPlayer)
        {
            return;
        }           

        GetAxis();
        ApplyMovement();
    }

    void GetAxis()
    {
        horizontalAxis = Input.GetAxis("Horizontal");
        verticalAxis = Input.GetAxis("Vertical");

        if(isHacked)
        {
            horizontalAxis *= -1.0f;
            verticalAxis *= -1.0f;
        }
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

        movement.Normalize();

       CmdUpdateMovementEffect(movement.magnitude, horizontalAxis < 0);

        float magnitude = Mathf.Lerp(rb.velocity.magnitude, movementSpeed, Time.deltaTime * acceleration);

        magnitude = Mathf.Max(magnitude, minSpeedWhenInput);

        float pourcentage = (float)player.ressourceCount / (float)player.maxRessourceCount;
        if(pourcentage < 0.25f)
            magnitude *= speedMul_0_25;
        else if(pourcentage < 0.5f)
            magnitude *= speedMul_25_50;
        else if(pourcentage < 0.75f)
            magnitude *= speedMul_50_75;
        else
            magnitude *= speedMul_75_100;

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
        stundEffect.SetActive(true);
        stunTimer = Time.time + stunedTime;
    }

    public void isHackedFor(float hackTime)
    {
        isHacked = true;
        Instantiate(hackEffectStart, this.transform.position + this.transform.up * 8.0f - this.transform.forward * 5.0f, Quaternion.FromToRotation(this.transform.forward, this.transform.up));
        hackEffectDuration.SetActive(true);
        hackTimer = Time.time + hackTime;
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
