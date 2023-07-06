using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float Speed;
    public float JumpSpeed;
    public int MaxJumps;
    public GameObject Ball;
    public float ThrowForce;
    

    private Rigidbody2D rigidbody;
    private Animator animator;
    private bool shouldJump;
    private bool isAirborne;
    private int jumpCount;

    private BallController ballController;
    private bool shouldThrowBall;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        isAirborne = true;
        jumpCount = 0;

        ballController = Ball.GetComponent<BallController>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalAxis = Input.GetAxis("Horizontal");
        float horizontalTranslation = horizontalAxis * Speed * Time.deltaTime;

        Look(horizontalAxis);
        JumpInput();

        animator.SetBool("Running", horizontalAxis != 0 && !isAirborne);
        animator.SetBool("Idle", horizontalAxis == 0 && !isAirborne);
        animator.SetInteger("Jump Count", jumpCount);
        animator.SetBool("Jumping", rigidbody.velocity.y > 0 && isAirborne);
        animator.SetBool("Falling", rigidbody.velocity.y < 0 && isAirborne);
        if (Input.GetButtonDown("Control Ball") && !Ball.activeSelf) {
            Debug.Log("THROW");
            shouldThrowBall = true;
        }
    }

    private void FixedUpdate() {
        Move();

        if (shouldJump) {
            Jump();
        }

        if (shouldThrowBall) {
            Ball.SetActive(true);
            ThrowBall();
        }
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

    private void JumpInput() {
        if (jumpCount < MaxJumps && Input.GetButtonDown("Jump")) {
            shouldJump = true;
            if (isAirborne) {
                jumpCount += 1;
            }
        }
    }

    private void Jump() {
        rigidbody.velocity = new Vector2(rigidbody.velocity.x, 0);
        rigidbody.AddForce(Vector2.up * JumpSpeed, ForceMode2D.Impulse);
        shouldJump = false;
    }

    private void Move() {
        float horizontalAxis = Input.GetAxis("Horizontal");
        rigidbody.velocity = new Vector2(horizontalAxis * Speed, rigidbody.velocity.y);
    }

    private void ThrowBall() {
        Ball.transform.position = transform.position;
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 throwForce = (mouseWorldPosition - transform.position).normalized * ThrowForce;

        ballController.Launch(throwForce);
        shouldThrowBall = false;
    }

    private void OnCollisionEnter2D(Collision2D other) {
        // if (other.gameObject.tag == "Ball") {

        //     rigidbody.AddForce(other.gameObject.GetComponent<Rigidbody2D>().velocity, ForceMode2D.Impulse);
        // }
    }

    private void OnCollisionExit2D(Collision2D other) {
    }

    private void OnTriggerEnter2D(Collider2D other) {
        isAirborne = false;
        jumpCount = 0;
    }

    private void OnTriggerExit2D(Collider2D other) {
        isAirborne = true;
        jumpCount += 1;
    }
}
