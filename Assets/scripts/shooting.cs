using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField] private Rigidbody2D bulletPrefab;
    [SerializeField] private float bulletSpeed = 8f;
    [SerializeField] private float fireRate = 0.2f;  // Time between shots when holding space

    private float nextFireTime = 0f;  // Tracks when the next bullet can be fired
    public AudioSource source;
    public AudioClip firingClip;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        ShootBullets();
    }

    private void ShootBullets()
    {
        // Mouse button fires once per click
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            FireBullet();
        }

        // Space fires continuously with a fire rate limit
        if (Input.GetKey(KeyCode.Space) && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;  // Update next fire time based on fire rate
            FireBullet();
        }
    }

    private void FireBullet()
    {
        source.clip = firingClip;
        source.Play();
        Rigidbody2D bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
        bullet.transform.Rotate(0, 0, -90);
        Vector2 shipVelocity = rb.velocity;
        Vector2 shipDirection = transform.up;
        float shipSpeed = Vector2.Dot(shipVelocity, shipDirection);

        if (shipSpeed < 0)
        {
            shipSpeed = 0;
        }

        bullet.velocity = shipDirection * shipSpeed;
        bullet.AddForce(bulletSpeed * transform.up, ForceMode2D.Impulse);

    }
}
