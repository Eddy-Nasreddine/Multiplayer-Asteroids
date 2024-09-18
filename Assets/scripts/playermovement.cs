using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playermovement : MonoBehaviour
{
    [SerializeField] private float acceleration = 10f;
    [SerializeField] private float maxVelocity = 10f;
    private Rigidbody2D rb;
    [SerializeField] private float rotationSpeed = 180f;
    private bool isAccelerating = false;
    // Start is called before the first frame update

    

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        shipAcceleration();
        shipRotation();
    }
    private void FixedUpdate()
    {
        if (isAccelerating)
        {
            rb.AddForce(acceleration * transform.up);
            rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxVelocity);

        }
        else {

            rb.velocity = rb.velocity / 1.05f;

        }
    }
    private void shipAcceleration()
    {
        isAccelerating = Input.GetKey(KeyCode.W);
    }

    private void shipRotation()
    {
        if (Input.GetKey(KeyCode.A)) {
            transform.Rotate(0f,0f,1f, Space.World);
        } else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(0f, 0f, -1f, Space.World);
        }

    }

    
}
