using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    //Declare Rigidbody 2D
    Rigidbody2D rb;

    //Declare Movement Multiplier
    public float moveSpeed = 10f;

    //Declare Jump Force
    public float jumpForce = 10f;

    //Check if Player is on Ground
    public bool isGrounded;

    void Start()
    {
        //Get Rigidbody 2D
        rb = GetComponent<Rigidbody2D>();
    }
    // Update is called once per frame
    void Update()
    {
        //Store Horizontal Axis
        float hAxis = Input.GetAxis("Horizontal");

        //Set Velocity
        rb.velocity = new Vector2(hAxis * moveSpeed, rb.velocity.y);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

}