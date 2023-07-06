using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public GameObject Owner;
    public float MinRecallForce;

    private Rigidbody2D rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Recall Ball")) {
            RecallBall();
        }
    }

    private void RecallBall() {
        Vector2 direction = (Owner.transform.position - transform.position).normalized;
        float force = Mathf.Max(MinRecallForce);

        rigidbody.AddForce(force * direction, ForceMode2D.Impulse);
    }
}
