using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class moveasteroid : MonoBehaviour
{
    private float speed;
    private Vector2 movingDirection;
    private Vector2 movingDirection2;
    public GameObject asteroid;

    // Start is called before the first frame update
    void Start()
    {
        speed = UnityEngine.Random.Range(0.1f, 5f);
        float directionDegrees = UnityEngine.Random.Range(0f, 260f);
        movingDirection.x = Mathf.Cos(directionDegrees);
        movingDirection.y = Mathf.Sin(directionDegrees);

        directionDegrees = UnityEngine.Random.Range(0f, 260f);
        movingDirection2.x = Mathf.Cos(directionDegrees);
        movingDirection2.y = Mathf.Sin(directionDegrees);

        Vector2 screenMiddle = new Vector2(0,0);
        GameObject asteroid1 = GameObject.Instantiate(asteroid);

        Rigidbody2D rb1 = asteroid1.GetComponent<Rigidbody2D>();
        rb1.velocity = movingDirection * speed;
        asteroid1.transform.position = screenMiddle;

        GameObject asteroid2 = GameObject.Instantiate(asteroid);
        Rigidbody2D rb2 = asteroid2.GetComponent<Rigidbody2D>();
        rb2.velocity = movingDirection2 * speed;
        asteroid2.transform.position = screenMiddle;

    }

    // Update is called once per frame
    void Update()
    {
    }
}
