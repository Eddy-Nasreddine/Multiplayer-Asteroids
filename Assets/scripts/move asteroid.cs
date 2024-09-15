using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class moveasteroid : MonoBehaviour
{
    public float SpeedMin = 0.5f;
    public float SpeedMax = 7f;
    public int AsteroidsAmount = 10;
    public GameObject asteroid;
    private float rotation = 0;

    private GameObject[] asteroids;

    // Start is called before the first frame update
    void Start()
    {

        for (int i = 0; i < AsteroidsAmount; i++)
        {
            GameObject roid = GameObject.Instantiate(asteroid);
            float speed = UnityEngine.Random.Range(SpeedMin, SpeedMax);
            float directionDegrees = UnityEngine.Random.Range(0f, 260f);
            float xVelocity = Mathf.Cos(directionDegrees);
            float yVelocity = Mathf.Sin(directionDegrees);


            float x = UnityEngine.Random.Range(0f, 1f);
            float y = UnityEngine.Random.Range(0f, 1f);
            Vector3 viewportPoint = new Vector3(x, y, -Camera.main.transform.position.z);

            roid.transform.position = Camera.main.ViewportToWorldPoint(viewportPoint);

            Rigidbody2D rb = roid.GetComponent<Rigidbody2D>();
            rb.velocity = new Vector2(xVelocity, yVelocity) * speed;
        }
    }

    // Update is called once per frame
    void Update()
    {
       
        //Debug.Log(Camera.main.WorldToViewportPoint(objlocation));


    }
}
