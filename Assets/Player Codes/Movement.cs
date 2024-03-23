using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator animator;
    public float moveSpeed = 10f;
    public float baseJumpForce = 10f; // Base jump force
    public float maxJumpForce = 20f; // Maximum jump force
    public float jumpChargeRate = 10f; // Rate at which jump force increases while charging
    public float maxJumpTime = 1f; // Maximum duration of the jump
    private float jumpForce;
    private float jumpTimeCounter;
    private bool isGrounded;
    private bool isChargingJump;
    private bool isJumping;
    private bool hasStartedCharging; // New variable to track if charging animation has started
    private bool isWalled;
    private float lastHorizontalInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float hAxis = Input.GetAxis("Horizontal");

        if (hAxis < 0) sr.flipX = true;
        else if (hAxis > 0) sr.flipX = false;

        // Allow horizontal movement only if not charging jump or jumping
        if (!isChargingJump && isGrounded)
        {
            rb.velocity = new Vector2(hAxis * moveSpeed, rb.velocity.y);

        }
        else
        {
            // Stop horizontal movement while charging jump or jumping
            rb.velocity = new Vector2(0f, rb.velocity.y);
        }

        // Start charging the jump if the player is grounded and presses the jump button
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !animator.GetBool("isFalling") && !isChargingJump)
        {
            
            isChargingJump = true;
            hasStartedCharging = true; // Charging animation has started
            jumpTimeCounter = 0;
            animator.SetTrigger("charge");
        }

        // Charge the jump if the player is holding down the jump button and charging animation has started
        if (Input.GetKey(KeyCode.Space) && isGrounded && !animator.GetBool("isFalling") && hasStartedCharging)
        {
            jumpTimeCounter += Time.deltaTime;

            // Automatically trigger the jump if the max jump force has been reached
            if (jumpTimeCounter >= maxJumpTime)
            {
                ReleaseJump();
            }
        }

        // Release the charged jump if charging animation has started
        if (Input.GetKeyUp(KeyCode.Space) && isGrounded && !animator.GetBool("isFalling") && hasStartedCharging)
        {
            ReleaseJump();
        }

        // Prevent changing direction while mid-air
        if (!isGrounded)
        {
            //reset the jump timer 
            jumpTimeCounter = 0;

            // Set the horizontal velocity based on the last horizontal input direction
            if(isWalled){
                 rb.velocity = new Vector2(-lastHorizontalInput * moveSpeed, rb.velocity.y);
            }
            else{
                rb.velocity = new Vector2(lastHorizontalInput * moveSpeed, rb.velocity.y);
            }
            
        }

        // Set isJumping to false when landing
        if (isGrounded && isJumping)
        {
            isJumping = false;
        }
        animator.SetBool("isWalking", hAxis != 0);
        animator.SetBool("isFalling", rb.velocity.y < 0);

    }

    // Set isGrounded to true when the player is on the ground
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            isWalled = false;
        }
    }

    // detect if player is tumama sa wall
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") && !isGrounded)
        {
            isWalled = true;
        }
    }


    // Set isGrounded to false when the player leaves the ground
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    // Calculate the jump force based on the time the jump button was held down
    private float CalculateJumpForce()
    {
        float chargePercentage = Mathf.Clamp01(jumpTimeCounter / maxJumpTime);
        Debug.Log(chargePercentage);
        jumpForce = Mathf.Lerp(baseJumpForce, maxJumpForce, chargePercentage);
        return jumpForce;
    }

    // Release the charged jump
    private void ReleaseJump()
    {
        jumpForce = CalculateJumpForce();
        Vector2 jumpDirection = Vector2.right * lastHorizontalInput;
        rb.velocity = new Vector2(jumpDirection.x * jumpForce, jumpForce);
        jumpTimeCounter = 0;
        isChargingJump = false;
        isJumping = true;
        animator.SetTrigger("jump");
        hasStartedCharging = false; // Reset charging animation flag
    }
}