using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float GravityForce;
    public float Speed;
    public float JumpSpeed;
    public float JumpDuration;
    

    private Animator animator;
    private bool isAirborne;
    private float jumpTimer;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        isAirborne = true;
        jumpTimer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal_axis = Input.GetAxis("Horizontal");
        float horizontal_translation = horizontal_axis * Speed * Time.deltaTime;
        animator.SetBool("Running", horizontal_axis != 0 && !isAirborne);
        animator.SetBool("Idle", horizontal_axis == 0 && !isAirborne);

        if (horizontal_translation > 0) {
            // Look Right
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x);
            transform.localScale = scale;
        } else if (horizontal_translation < 0) {
            // Look Left
            Vector3 scale = transform.localScale;
            scale.x = -Mathf.Abs(scale.x);
            transform.localScale = scale;
        }

        if (Input.GetButtonDown("Jump")) {
            jumpTimer = JumpDuration;
        }
        
        float vertical_translation = 0f;
        bool isJumping = jumpTimer > 0;
        if (isJumping) {
            vertical_translation = JumpSpeed * Time.deltaTime;
            jumpTimer -= Time.deltaTime;
        } else if (isAirborne) {
            // Apply Gravity
            vertical_translation = -GravityForce * Time.deltaTime;
        }

        animator.SetBool("Jumping", vertical_translation > 0 && isAirborne);
        animator.SetBool("Falling", vertical_translation < 0 && isAirborne);

        transform.Translate(horizontal_translation, vertical_translation, 0);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        isAirborne = false;
    }

    private void OnCollisionExit2D(Collision2D other) {
        isAirborne = true;
    }
}
