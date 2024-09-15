using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Profiling;
using static UnityEngine.GridBrushBase;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;
using Vector4 = UnityEngine.Vector4;




public class moveasteroid : MonoBehaviour
{
    public float SpeedMin = 0.5f;
    public float SpeedMax = 7f;
    public int AsteroidsAmount = 10;
    public GameObject asteroid;

    private List<GameObject> asteroids = new List<GameObject> { };
    private List<int> asteroidsState = new List<int>(); // 0 = entering screen  1 = onscreen



    Vector3 RandomLocationOffScreen()
    {
        int screenSide = UnityEngine.Random.Range(0, 4);

        float y = 0;
        float x = 0;
        switch (screenSide)
        {
            case 0: //up
                y = UnityEngine.Random.Range(1.1f, 1.15f);
                x = UnityEngine.Random.Range(-0.15f, 1.15f);
                break;
            case 1: //down
                y = UnityEngine.Random.Range(-0.1f, -0.15f);
                x = UnityEngine.Random.Range(-0.15f, 1.15f);
                break;
            case 2: //left
                y = UnityEngine.Random.Range(-0.15f, 1.15f);
                x = UnityEngine.Random.Range(-0.1f, -0.15f);
                break;
            case 3: //right
                y = UnityEngine.Random.Range(-0.15f, 1.15f);
                x = UnityEngine.Random.Range(1.1f, 1.15f);
                break;
        }






        return new Vector3(x, y, -Camera.main.transform.position.z);
    }

    Vector2 RandomValidVelocity(Vector2 pos)
    {
        Vector2 worldpos = Camera.main.ViewportToWorldPoint(pos);
        float targetx = UnityEngine.Random.Range(0.1f, 0.9f);
        float targety = UnityEngine.Random.Range(0.1f, 0.9f);
        Vector2 cornorpos = Camera.main.ViewportToWorldPoint(new Vector2(targetx, targety));
        float deltaX = cornorpos.x - worldpos.x;
        float deltaY = cornorpos.y - worldpos.y;
        float angle = Mathf.Atan2(deltaY, deltaX);

        float xVelocity = Mathf.Cos(angle);
        float yVelocity = Mathf.Sin(angle);

        return new Vector2(xVelocity, yVelocity);
    }

    // Start is called before the first frame update
    void Start()
    {

        for (int i = 0; i < AsteroidsAmount; i++)
        {
            GameObject roid = GameObject.Instantiate(asteroid);
            float speed = UnityEngine.Random.Range(SpeedMin, SpeedMax);
            Vector3 pos = RandomLocationOffScreen();


            roid.transform.position = Camera.main.ViewportToWorldPoint(pos);
            Vector2 velocity = RandomValidVelocity(pos);


            Rigidbody2D rb = roid.GetComponent<Rigidbody2D>();
            rb.velocity = velocity * speed;
            asteroids.Add(roid);
            asteroidsState.Add(0);
        }
    }

    // Update is called once per frame
    void Update()
    {

        for (int i = 0; i < asteroids.Count; i++)
        {
            GameObject roid = asteroids[i];

            Vector3 size = roid.GetComponent<Renderer>().bounds.size;
            Vector3 pos = Camera.main.WorldToViewportPoint(roid.transform.position);



            if (asteroidsState[i] == 0) //dont despawn asteroids when they havent been on screen yet
            {
                if (!(pos.x < 0 | pos.x > 1 | pos.y < 0 | pos.y > 1))
                {
                    asteroidsState[i] = 1;
                }

                continue;
            }
            if (pos.x < 0 | pos.x > 1 | pos.y < 0 | pos.y > 1)
            {
                asteroidsState[i] = 0;
                pos = RandomLocationOffScreen();
                Vector2 velocity = RandomValidVelocity(pos);
                //Debug.Log("pos: " + pos + " velocity: " + velocity);
                roid.transform.position = Camera.main.ViewportToWorldPoint(pos);
                Rigidbody2D rb = roid.GetComponent<Rigidbody2D>();
                float speed = UnityEngine.Random.Range(SpeedMin, SpeedMax);
                rb.velocity = velocity * speed;

            }
            //Debug.Log(Camera.main.WorldToViewportPoint(objlocation));
        }

    }
}
