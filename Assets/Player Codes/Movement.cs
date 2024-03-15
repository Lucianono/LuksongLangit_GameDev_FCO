using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    Rigidbody2D rb;
    public float moveSpeed = 5f;
    public float baseJumpForce = 5f; // Base jump force
    public float maxJumpForce = 10f; // Maximum jump force
    public float jumpChargeRate = 10f; // Rate at which jump force increases while charging
    public float maxJumpTime = 1f; // Maximum duration of the jump
    private float jumpTimeCounter;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float hAxis = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(hAxis * moveSpeed, rb.velocity.y);

        // Start charging the jump if the player is grounded and presses the jump button
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            jumpTimeCounter = 0;
        }

        // Charge the jump if the player is holding down the jump button
        if (Input.GetKey(KeyCode.Space) && isGrounded)
        {
            jumpTimeCounter += Time.deltaTime;
        }

        // Release the charged jump
        if (Input.GetKeyUp(KeyCode.Space) && isGrounded)
        {
            float jumpForce = CalculateJumpForce();
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpTimeCounter = 0;
        }

        // Reset isGrounded flag when leaving the ground
        if (!isGrounded)
        {
            jumpTimeCounter = 0;
        }
    }

    // Set isGrounded to true when the player is on the ground
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
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
        float jumpForce = Mathf.Lerp(baseJumpForce, maxJumpForce, chargePercentage);
        return jumpForce;
    }
}