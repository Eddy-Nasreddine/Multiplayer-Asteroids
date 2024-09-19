using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class shooting : MonoBehaviour
{

    private Rigidbody2D rb;

    [SerializeField] private Transform BulletSpawn;
    [SerializeField] private Rigidbody2D bulletPrefab;
    [SerializeField] private float bulletspeed = 8f;

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
        if (Input.GetKeyDown(KeyCode.Space) | Input.GetKeyDown(KeyCode.Mouse0))
        {
            Rigidbody2D bullet = Instantiate(bulletPrefab, BulletSpawn.position, BulletSpawn.rotation);
            bullet.transform.Rotate(0, 0, -90);
            Vector2 shipVelocity = rb.velocity;
            Vector2 shipDirection = transform.up;
            float shipSpeed = Vector2.Dot(shipVelocity, shipDirection);
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
