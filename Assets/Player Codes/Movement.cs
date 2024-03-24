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
    public PhysicsMaterial2D bounceMat, normalMat; 
   
    public float jumpForce;
    public float jumpTimeCounter = 0;
    public bool isGrounded;
    public bool isChargingJump = false;
    public bool isJumping;
    public bool hasStartedCharging; // New variable to track if charging animation has started
    public float lastHorizontalInput;

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

        // Stop horizontal movement while charging jump or jumping
        if(isChargingJump){
            rb.velocity = new Vector2(0f, rb.velocity.y);
            Debug.Log("cahrge true");
        }

        // Allow horizontal movement only if not charging jump or jumping
        if (!isChargingJump && isGrounded && !isJumping)
        {
            rb.velocity = new Vector2(hAxis * moveSpeed, rb.velocity.y);
            lastHorizontalInput = hAxis;
        }

        // Start charging the jump if the player is grounded and presses the jump button
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !animator.GetBool("isFalling") && !isChargingJump)
        {
            
            isChargingJump = true;
            jumpTimeCounter = 0;
            animator.SetTrigger("charge");
        }

        // Charge the jump if the player is holding down the jump button and charging animation has started
        if (Input.GetKey(KeyCode.Space) && isGrounded && !animator.GetBool("isFalling"))
        {
            jumpTimeCounter += Time.deltaTime;

            // Automatically trigger the jump if the max jump force has been reached
            if (jumpTimeCounter >= maxJumpTime)
            {
                ReleaseJump();
            }
        }

        // Release the charged jump if charging animation has started
        if (Input.GetKeyUp(KeyCode.Space) && isGrounded && !animator.GetBool("isFalling"))
        {
            ReleaseJump();
        }

        // // Prevent changing direction while mid-air
        // if (!isGrounded)
        // {

        //     // Set the horizontal velocity based on the last horizontal input direction
        //     rb.velocity = new Vector2( lastHorizontalInput * moveSpeed, rb.velocity.y);
            
        // }

        // Set isJumping to false when landing
        if (isGrounded && isJumping)
        {
            isJumping = false;
        }

        if(rb.velocity.y < -1){

            rb.sharedMaterial = normalMat;

        }
        animator.SetBool("isWalking", hAxis != 0);
        animator.SetBool("isFalling", rb.velocity.y < -0.5);

        

    }

    // Set isGrounded to true when the player is on the ground
    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("ground detect");
        jumpForce = 0;

        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            rb.sharedMaterial = normalMat;
        }
    }

    


    // Set isGrounded to false when the player leaves the ground
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
            Debug.Log("exit");
        }
    }

    // Calculate the jump force based on the time the jump button was held down
    private float CalculateJumpForce()
    {
        float chargePercentage = Mathf.Clamp01(jumpTimeCounter / maxJumpTime);
        jumpForce = Mathf.Lerp(baseJumpForce, maxJumpForce, chargePercentage);
        return jumpForce;
    }

    // Release the charged jump
    private void ReleaseJump()
    {
        jumpForce = CalculateJumpForce();
        Vector2 jumpDirection = Vector2.right * lastHorizontalInput ;
        rb.velocity = new Vector2(jumpDirection.x * jumpForce, jumpForce);
        jumpTimeCounter = 0;
        isChargingJump = false;
        isJumping = true;
        rb.sharedMaterial = bounceMat;
        animator.SetTrigger("jump");
        Debug.Log("bouncerOn");
    }
}