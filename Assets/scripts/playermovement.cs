using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
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

    // References to life images on the canvas
    public Image[] lifeImages;
    private int currentLives;

    // Reference to the SpriteRenderer for changing the ship's color
    private SpriteRenderer spriteRenderer;
    public Color hitColor = Color.red; // Color when hit (red)
    public float blinkDuration = 0.2f; // Time to stay red

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // Get the SpriteRenderer component
        currentLives = lifeImages.Length; // Set current lives to the number of life images
    }

    // Update is called once per frame
    void Update()
    {
        ShipAcceleration();
        ShipRotation();
    }
    private void FixedUpdate()
    {
        if (isAccelerating)
        {
            animator.SetBool("isAccelerating", true);
            rb.AddForce(acceleration * transform.up);
            rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxVelocity);

        }
        else
        {
            animator.SetBool("isAccelerating", false);
            rb.velocity /= 1.05f;
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("asteroid"))
        {
            HandleLifeLoss();
            StartCoroutine(BlinkRed()); // Start blinking red on hit
        }
    }

    private void ShipAcceleration()
    {
        isAccelerating = Input.GetKey(KeyCode.W);
    }

    private void ShipRotation()
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

    private void HandleLifeLoss()
    {
        currentLives--;
        if (currentLives >= 0)
        {
            // Hide the life image corresponding to the current life count
            lifeImages[currentLives].enabled = false;
            StartCoroutine(InvincibilityFrames(2.5f));
        }

        if (currentLives <= 0)
        {
            Destroy(gameObject);
            gameover.Setup(spawnAsteroids.getPoints());
            isPlayerDead = true;
        }
    }

    // Coroutine to make the ship blink red
    private IEnumerator BlinkRed()
    {
        // Store the original color of the ship
        Color originalColor = spriteRenderer.color;

        // Change the color to red
        spriteRenderer.color = hitColor;

        // Wait for the blink duration
        yield return new WaitForSeconds(blinkDuration);

        // Revert the color back to the original
        spriteRenderer.color = originalColor;
    }

    // Handles invinceibility after ship collides with an asteroid 
    private IEnumerator InvincibilityFrames(float duration)
    {
        PolygonCollider2D polygon = GetComponent<PolygonCollider2D>();
        polygon.enabled = false;
        yield return new WaitForSeconds(duration);
        polygon.enabled = true;

    }
}
