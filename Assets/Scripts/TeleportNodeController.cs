using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportNodeController : MonoBehaviour
{
    public GameObject Player;
    public float LaunchForce;

    private PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        playerController = Player.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Ball") {
            Player.transform.position = transform.position;
            Vector2 dir = other.gameObject.GetComponent<Rigidbody2D>().velocity;
            dir.Normalize();
            playerController.Launch(dir * LaunchForce);

            BallController ballController = other.gameObject.GetComponent<BallController>();
            ballController.Recall();
        }
        
    }
}
