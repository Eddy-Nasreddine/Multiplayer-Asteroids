using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playermovement : MonoBehaviour
{
    [SerializeField] private float acceleration = 10f;
    [SerializeField] private float maxVelocity = 10f;
    [SerializeField] private bool mouseControls = true;
    public Animator animator;
    private Rigidbody2D rb;
    [Range(180f, 720f)]
    [SerializeField] private float rotationSpeed = 0.5f;
    private bool isAccelerating = false;
    public GameOver gameover;
    public SpawnAsteroids spawnAsteroids;
    public bool isPlayerDead = false;
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
        if (collision.CompareTag("asteroid"))
        {
            Destroy(gameObject);
            gameover.Setup(spawnAsteroids.getPoints());
            isPlayerDead = true;
        }
    }

    private void shipAcceleration()
    {
        isAccelerating = Input.GetKey(KeyCode.W);
    }

    private void shipRotation()
    {
        if (mouseControls == false)
        {
            if (Input.GetKey(KeyCode.A))
            {
                transform.Rotate(0f, 0f, 1f, Space.World);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                transform.Rotate(0f, 0f, -1f, Space.World);
            }
        }
        else if (mouseControls == true)
        {
            Vector3 mousepos = Input.mousePosition;
            mousepos.z = 0f;
            if (!(mousepos.x < 0 | mousepos.x > Screen.width | mousepos.y < 0 | mousepos.y > Screen.height)) //if mouse pos is on screen
            {
                Vector3 mousePosWorld = Camera.main.ScreenToWorldPoint(mousepos);
                //print("mousepos: " + Camera.main.ScreenToWorldPoint(mousepos) + "  ship pos: " + transform.position);
                float deltaX = mousePosWorld.x - transform.position.x;
                float deltaY = mousePosWorld.y - transform.position.y;
                float angle = (Mathf.Atan2(deltaY, deltaX) * Mathf.Rad2Deg);

                float shipAngle = transform.rotation.eulerAngles.z;
                float angleWorldSpace = (((angle + 90f - shipAngle + 360) % 360) - 180f); //degrees to turn to face the mouse but normalized to unitys rotation and the ships rotation
                //print(angleWorldSpace);
                float turnDirection = Math.Sign(angleWorldSpace);
                //print(turnDirection);

                float TurnDistance = turnDirection * rotationSpeed * Time.deltaTime;
                if (Math.Abs(angleWorldSpace) < Math.Abs(TurnDistance))
                {
                    transform.Rotate(0f, 0f, angleWorldSpace, Space.World);
                }else
                {
                    transform.Rotate(0f, 0f, TurnDistance, Space.World);
                } 
            }
        }
    }
}
