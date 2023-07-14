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
            BallController ballController = other.gameObject.GetComponent<BallController>();

            Vector2 dir = ballController.GetMovementDir();
            Player.transform.position = transform.position;
            playerController.Launch(dir * LaunchForce);

            ballController.Recall();
        }
        
    }
}
