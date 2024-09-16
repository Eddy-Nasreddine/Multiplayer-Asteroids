using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playermovement : MonoBehaviour
{
    [SerializeField] private float acceleration = 10f;
    [SerializeField] private float maxVelocity = 10f;
    public Rigidbody2D rb;
    [SerializeField] private float rotationSpeed = 180f;
    [SerializeField] private float bulletspeed = 8f;
    private bool isAccelerating = false;
    // Start is called before the first frame update

    [Header("Object references")]
    [SerializeField] private Transform BulletSpawn;
    [SerializeField] private Rigidbody2D bulletPrefab;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

        //horizontal = Input.GetAxisRaw("Horizontal");

        //vertical = Input.GetAxisRaw("Vertical");
        //Vector2 direction = new Vector2(horizontal, vertical);
        //float inputMagnitude = Mathf.Clamp01(direction.magnitude);
        //direction.Normalize();
        //transform.Translate(direction * speed * inputMagnitude * Time.deltaTime, Space.World);
        //ShootBullets();
        ////transform.rotation.z += 1;
        //if (direction != Vector2.zero)
        //{
        //    Quaternion toRotation = Quaternion.LookRotation(Vector3.forward, direction);
        //    transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        //}
        shipAcceleration();
        shipRotation();
        ShootBullets();
    }
    private void FixedUpdate()
    {
        if (isAccelerating)
        {
            rb.AddForce(acceleration * transform.up);
            rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxVelocity);

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

    private void ShootBullets()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Rigidbody2D bullet = Instantiate(bulletPrefab, BulletSpawn.position, BulletSpawn.rotation);
            bullet.transform.Rotate(0, 0, -90);
            Vector2 shipVelocity = rb.velocity;
            Vector2 shipDirection = transform.up;
            float shipSpeed = Vector2.Dot(shipVelocity ,shipDirection);
            Debug.Log(shipVelocity + " " + shipDirection + " DOT:" + shipSpeed);
            if (shipSpeed < 0)
            {
                shipSpeed = 0;
            }
            bullet.velocity = shipDirection * shipSpeed;
            bullet.AddForce(bulletspeed * transform.up, ForceMode2D.Impulse);
            print("shooot");
        }
    }
}
