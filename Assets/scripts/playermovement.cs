using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playermovement : MonoBehaviour
{
    [SerializeField] private float acceleration = 10f;
    [SerializeField] private float maxVelocity = 10f;
    public Animator animator;
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
            animator.SetBool("isAccelerating", true);
            rb.AddForce(acceleration * transform.up);
            rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxVelocity);

        }
        else {
            animator.SetBool("isAccelerating", false);
            rb.velocity = rb.velocity / 1.05f;

        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("yaya1");
        if (collision.CompareTag("asteroid"));
        {
            Destroy(gameObject);
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
