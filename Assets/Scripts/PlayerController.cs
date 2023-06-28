using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float GravityForce;
    public float Speed;
    public float JumpSpeed;
    public float JumpDuration;
    

    private bool isAirborne;
    private float jumpTimer;

    // Start is called before the first frame update
    void Start()
    {
        isAirborne = true;
        jumpTimer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal_translation = Input.GetAxis("Horizontal") * Speed * Time.deltaTime;


        if (Input.GetButtonDown("Jump")) {
            jumpTimer = JumpDuration;
        }
        
        float vertical_translation = 0f;
        if (jumpTimer > 0) {
            vertical_translation = JumpSpeed * Time.deltaTime;
            jumpTimer -= Time.deltaTime;
        } else if (isAirborne) {
            // Apply Gravity
            // transform.position += Vector3.down * GravityForce * Time.deltaTime;
            vertical_translation = -GravityForce * Time.deltaTime;
        }


        transform.Translate(horizontal_translation, vertical_translation, 0);

        Debug.Log(isAirborne);
        
    }

    private void OnCollisionEnter2D(Collision2D other) {
        isAirborne = false;
    }

    private void OnCollisionExit2D(Collision2D other) {
        isAirborne = true;
    }
}
