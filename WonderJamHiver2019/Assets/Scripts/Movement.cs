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

    Rigidbody rb;

    public bool canMove = true;

    float stunTimer;
    bool isStuned;

   

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (!rb)
            Debug.LogWarning("Pas de rigidbody sur le joueur");
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
        movement.Normalize();

        float magnitude = Mathf.Lerp(rb.velocity.magnitude, movementSpeed, Time.deltaTime * acceleration);

        //this.transform.position += new Vector3(movement.x * magnitude, rb.velocity.y, movement.y * magnitude);
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
}
