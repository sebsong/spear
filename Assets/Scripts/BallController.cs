using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public GameObject Owner;
    public float MinRecallForce;

    private Rigidbody2D rigidbody;
    private bool inRecall;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        inRecall = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Control Ball") && gameObject.activeSelf) {
            Recall();
        }

        if (inRecall) {
            float force = Mathf.Max(MinRecallForce);
            transform.position = Vector2.MoveTowards(transform.position, Owner.transform.position, force * Time.deltaTime);
        }
    }

    public void Launch(Vector2 launchForce) {
        rigidbody.isKinematic = false;
        rigidbody.velocity = Vector2.zero;
        rigidbody.AddForce(launchForce, ForceMode2D.Impulse);
    }

    public void FinishRecall() {
        if (inRecall) {
            inRecall = false; 
            gameObject.SetActive(false);
        }
    }

    private void Recall() {
        gameObject.layer = Constants.IGNORE_LAYER;
        inRecall = true;
        rigidbody.isKinematic = true;
        rigidbody.velocity = Vector2.zero;
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag == "Player" && inRecall) {
            // rigidbody.isKinematic = false;
            // Vector2 launchForce = (Owner.transform.position - transform.position).normalized * MinRecallForce;
            // Rigidbody2D playerRididBody = other.gameObject.GetComponent<Rigidbody2D>();
            // playerRididBody.AddForce(launchForce, ForceMode2D.Impulse);
        }
    }
}
