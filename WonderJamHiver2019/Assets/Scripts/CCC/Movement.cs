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

    bool canMove = true;

    public Animator animator;


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
    }

     void FixedUpdate()
    {
        if(horizontalAxis > 0)
        {
            animator.SetBool("IsMovingRight", true);
        }
        if (horizontalAxis < 0)
        {
            animator.SetBool("IsMovingRight", false);
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

        animator.SetFloat("Speed", Mathf.Abs(horizontalAxis) + Mathf.Abs(verticalAxis));

        Vector2 movement = new Vector2(horizontalAxis, verticalAxis);
        movement.Normalize();

        float magnitude = Mathf.Lerp(rb.velocity.magnitude, movementSpeed, Time.deltaTime * acceleration);

        rb.velocity = new Vector3(movement.x * magnitude, rb.velocity.y, movement.y * magnitude);
    }
}
