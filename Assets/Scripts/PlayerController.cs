using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float GravityForce;

    private bool isAirborne;

    // Start is called before the first frame update
    void Start()
    {
        isAirborne = true;
    }

    // Update is called once per frame
    void Update()
    {
        // Apply gravity
        if (isAirborne) {
            transform.position += Vector3.down * GravityForce * Time.deltaTime;
        }
        Debug.Log(isAirborne);
        
    }

    private void OnCollisionEnter2D(Collision2D other) {
        isAirborne = false;
    }

    private void OnCollisionExit2D(Collision2D other) {
        isAirborne = true;
    }
}
