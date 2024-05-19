using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Movement : MonoBehaviour
{
   
    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator animator;
    AudioSource audioSource;
    public AudioClip jumpSFX, landSFX, hitSFX;
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
    public GameObject gameOverCanvas;
    public GameObject timeCanvas;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
      

        if (Mathf.Abs(rb.velocity.x) < 0.01f)
        {
            rb.velocity = new Vector2(0f, rb.velocity.y);
        }

        float hAxis = Input.GetAxis("Horizontal");

        if (hAxis < 0) sr.flipX = true;
        else if (hAxis > 0) sr.flipX = false;

        // Stop horizontal movement while charging jump or jumping
        if(isChargingJump){
            rb.velocity = new Vector2(0f, rb.velocity.y);
        }

        // Allow horizontal movement only if not charging jump or jumping
        if (!isChargingJump && isGrounded && !isJumping)
        {
            rb.velocity = new Vector2(hAxis * moveSpeed, rb.velocity.y);
            lastHorizontalInput = hAxis;
        }

        // Start charging the jump if the player is grounded and presses the jump button
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded  && !isChargingJump)
        {
            
            isChargingJump = true;
            jumpTimeCounter = 0;
            animator.SetTrigger("charge");
        }

        // Charge the jump if the player is holding down the jump button and charging animation has started
        if (Input.GetKey(KeyCode.Space) && isGrounded)
        {
            jumpTimeCounter += Time.deltaTime;

            // Automatically trigger the jump if the max jump force has been reached
            if (jumpTimeCounter >= maxJumpTime)
            {
                ReleaseJump();
            }
        }

        // Release the charged jump if charging animation has started
        if (Input.GetKeyUp(KeyCode.Space) && isGrounded )
        {
            ReleaseJump();
        }

        if (isGrounded && isJumping)
        {
            isJumping = false;
        }

        if(rb.velocity.y < -1){

            rb.sharedMaterial = normalMat;

        }
        animator.SetBool("isWalking", hAxis != 0);
        animator.SetBool("isFalling", rb.velocity.y < -1);

        if (rb.velocity.x != 0 && isGrounded)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else 
        {
            audioSource.Stop();
        }

    }

    void playSound(AudioClip c) 
    {
        AudioSource aud = gameObject.AddComponent<AudioSource>();
        aud.clip = c;
        aud.volume = 0.5f;
        aud.Play();
        Destroy(aud, c.length);
    }

    // Set isGrounded to true when the player is on the ground
    private void OnTriggerStay2D(Collider2D collision)
    {
        
        jumpForce = 0;

        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            rb.sharedMaterial = normalMat;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "finishline")
        {
            Time.timeScale = 0f;
            gameOverCanvas.SetActive(true);

        }

        if (collision.gameObject.CompareTag("Ground"))
        {
            playSound(landSFX);
        }
    }

    // Set isGrounded to false when the player leaves the ground
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
        if (collision.gameObject.name == "finishline")
        {
            Time.timeScale = 1f;
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
        playSound(jumpSFX);
        jumpForce = CalculateJumpForce();
        
        rb.velocity = new Vector2( lastHorizontalInput  * moveSpeed, jumpForce);
       
        jumpTimeCounter = 0;
        isChargingJump = false;
        isJumping = true;
        rb.sharedMaterial = bounceMat;
        animator.SetTrigger("jump");
    }

}

