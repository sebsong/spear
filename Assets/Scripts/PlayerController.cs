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
    public float LaunchForce;
    

    private Rigidbody2D rigidbody;
    private Animator animator;
    private bool shouldJump;
    private bool isAirborne;
    private int jumpCount;

    private BallController ballController;
    private bool canThrowBall;
    private bool shouldThrowBall;
    private bool launching;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        isAirborne = true;
        jumpCount = 0;

        ballController = Ball.GetComponent<BallController>();
        canThrowBall = true;
        shouldThrowBall = false;
        launching = false;
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

        if (Input.GetButtonDown("Control Ball") && !Ball.activeSelf && (canThrowBall || !isAirborne)) {
            shouldThrowBall = true;
        }
    }

    private void FixedUpdate() {
        Move();

        if (shouldJump) {
            Jump();
        }

        if (shouldThrowBall) {
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
        launching = false;
    }

    private bool isSameDirection(float velocity, float otherVelocity) {
        return velocity < 0 == otherVelocity < 0;
    }

    private void Move() {
        float horizontalAxis = Input.GetAxis("Horizontal");
        float newVelocityX = horizontalAxis * Speed;

        if (isAirborne) {
            if (newVelocityX == 0 || (isSameDirection(newVelocityX, rigidbody.velocity.x)) && Mathf.Abs(newVelocityX) < Mathf.Abs(rigidbody.velocity.x)) {
                // don't affect the velocity if trying to move in the same direction but just more slowly
                return;
            }
        }

        rigidbody.velocity = new Vector2(horizontalAxis * Speed, rigidbody.velocity.y);
    }

    private void ThrowBall() {
        canThrowBall = false;
        Ball.layer = Constants.IGNORE_LAYER;
        Ball.transform.position = transform.position;
        Ball.SetActive(true);
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 throwDir = mouseWorldPosition - transform.position;
        throwDir.Normalize();
        ballController.Launch(throwDir * ThrowForce);
        shouldThrowBall = false;
    }

    public void Launch(Vector2 force) {
        isAirborne = true;
        launching = true;
        rigidbody.velocity = Vector2.zero;
        rigidbody.AddForce(force, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        launching = false;
    }

    private void OnCollisionExit2D(Collision2D other) {
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.transform.parent && other.transform.parent.tag == "Reset Jump") {
            isAirborne = false;
            jumpCount = 0;
            canThrowBall = true;
        }

        if (other.gameObject.tag == "Ball" && ballController.inRecall) {
            ballController.FinishRecall();
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.transform.parent && other.transform.parent.tag == "Reset Jump") {
            isAirborne = true;
            jumpCount += 1;
        }

        if (other.gameObject.tag == "Ball") {
            Ball.layer = Constants.BALL_LAYER;
        }
    }
}
