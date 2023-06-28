using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float GravityForce;
    public float Speed;
    public float JumpSpeed;
    public float JumpDuration;
    public int MaxJumps;
    

    private Animator animator;
    private bool isAirborne;
    private float jumpTimer;
    private int jumpCount;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        isAirborne = true;
        jumpTimer = 0f;
        jumpCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalAxis = Input.GetAxis("Horizontal");
        float horizontalTranslation = horizontalAxis * Speed * Time.deltaTime;
        animator.SetBool("Running", horizontalAxis != 0 && !isAirborne);
        animator.SetBool("Idle", horizontalAxis == 0 && !isAirborne);

        Look(horizontalAxis);

        if (Input.GetButtonDown("Jump") && jumpCount < MaxJumps) {
            jumpTimer = JumpDuration;
            jumpCount += 1;
        }
        
        float verticalTranslation = 0f;
        bool isJumping = jumpTimer > 0;
        if (isJumping) {
            verticalTranslation = JumpSpeed * Time.deltaTime;
            jumpTimer -= Time.deltaTime;
        } else if (isAirborne) {
            // Apply Gravity
            verticalTranslation = -GravityForce * Time.deltaTime;
        }

        animator.SetBool("Jumping", verticalTranslation > 0 && isAirborne);
        animator.SetInteger("Jump Count", jumpCount);
        animator.SetBool("Falling", verticalTranslation < 0 && isAirborne);

        transform.Translate(horizontalTranslation, verticalTranslation, 0);
    }

    private void Look(float horizontalAxis) {
        if (horizontalAxis > 0) {
            // Look Right
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x);
            transform.localScale = scale;
        } else if (horizontalAxis < 0) {
            // Look Left
            Vector3 scale = transform.localScale;
            scale.x = -Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        isAirborne = false;
        jumpCount = 0;
    }

    private void OnCollisionExit2D(Collision2D other) {
        isAirborne = true;
    }
}
