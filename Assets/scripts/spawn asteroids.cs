using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Profiling;
using static UnityEngine.GridBrushBase;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;
using Vector4 = UnityEngine.Vector4;




public class SpawnAsteroids : MonoBehaviour
{
    public float SpeedMin = 0.5f;
    public float SpeedMax = 7f;
    public int AsteroidsAmount = 10;

    public GameObject asteroidObj;
    private List<AsteroidData> asteroids = new List<AsteroidData> { };




    private class AsteroidData
    {
        public GameObject asteroid;
        public int state = 0;  // 0 = entering screen  1 = onscreen
        public int split = 0;

        public AsteroidData(GameObject roid, int splited = 0)
        {
            asteroid = roid;
            split = splited;
        }
    }



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


    List<AsteroidData> SplitAsteroid(GameObject asteroid)
    {
        Vector2 pos = asteroid.transform.position;
        float rotationradians = (asteroid.transform.rotation.eulerAngles.z % 360) * Mathf.Deg2Rad;
        float rotationRight = rotationradians + (Mathf.PI / 2);
        float rotationLeft = rotationradians - (Mathf.PI / 2);
        Vector2 rightVec = new Vector2(Mathf.Cos(rotationRight), Mathf.Sin(rotationRight));
        Vector2 leftVec = new Vector2(Mathf.Cos(rotationLeft), Mathf.Sin(rotationLeft));

        Vector2 size = asteroid.GetComponent<Renderer>().bounds.size / 1.5f;

        GameObject roid1 = GameObject.Instantiate(asteroidObj);
        roid1.transform.position = (leftVec * (size / 5) ) + pos;
        Rigidbody2D rb = roid1.GetComponent<Rigidbody2D>();
        float speed = UnityEngine.Random.Range(SpeedMin, SpeedMax);
        rb.velocity = leftVec * speed;

        GameObject roid2 = GameObject.Instantiate(asteroidObj);
        roid2.transform.position = (rightVec * (size / 5)) + pos;
        rb = roid2.GetComponent<Rigidbody2D>();
        speed = UnityEngine.Random.Range(SpeedMin, SpeedMax);
        rb.velocity = rightVec * speed;

        roid1.GetComponent<rotate>().dead = 2;
        roid2.GetComponent<rotate>().dead = 2;
 

        roid1.transform.localScale = roid1.transform.localScale / 1.5f;
        roid2.transform.localScale = roid2.transform.localScale / 1.5f;

        roid1.GetComponent<SpriteRenderer>().material.color = Color.black;
        roid2.GetComponent<SpriteRenderer>().color = Color.black;

        return new List<AsteroidData> { new AsteroidData(roid1,1), new AsteroidData(roid2,1) };
    }


    // Start is called before the first frame update
    void Start()
    {

        for (int i = 0; i < AsteroidsAmount; i++)
        {
            AddAstroid();
        }
    }
    void AddAstroid()
    {
        GameObject roid = GameObject.Instantiate(asteroidObj);
        float speed = UnityEngine.Random.Range(SpeedMin, SpeedMax);
        Vector3 pos = RandomLocationOffScreen();


        roid.transform.position = Camera.main.ViewportToWorldPoint(pos);
        Vector2 velocity = RandomValidVelocity(pos);


        Rigidbody2D rb = roid.GetComponent<Rigidbody2D>();
        rb.velocity = velocity * speed;
        asteroids.Add(new AsteroidData(roid));
    }
    // Update is called once per frame
    void Update()
    {
        if (asteroids.Count <= 5)
        {   
            int random_roid_amount = UnityEngine.Random.Range(0, 3);
            for (int i = 0;i < random_roid_amount; i++)
            {
                AddAstroid();
            }
        }
        //bug if a asteroid spawns outside and then gets bumped by another befor it gos in the screen it will drift off screen forever
        for (int i = 0; i < asteroids.Count; i++)
        {
            GameObject roid = asteroids[i].asteroid;

            Vector3 size = roid.GetComponent<Renderer>().bounds.size;
            Vector3 pos = Camera.main.WorldToViewportPoint(roid.transform.position);
            int dead = roid.GetComponent<rotate>().dead;
            if (dead == 1)
            {
                asteroids.AddRange(SplitAsteroid(roid));
                UnityEngine.Object.Destroy(roid);
                asteroids.RemoveAt(i);
                i--;
                continue;
            }
            else if (dead == 3) {
                Debug.Log("ben");
                UnityEngine.Object.Destroy(roid);
                asteroids.RemoveAt(i);
                i--;
                continue;
            }

            if (asteroids[i].state == 0) //dont despawn asteroids when they havent been on screen yet
            {
                if (!(pos.x < 0 | pos.x > 1 | pos.y < 0 | pos.y > 1))
                {
                    asteroids[i].state = 1;
                }

                continue;
            }
            if (pos.x < 0 | pos.x > 1 | pos.y < 0 | pos.y > 1)
            {
                if (asteroids[i].asteroid.GetComponent<rotate>().dead == 2)
                {
                    UnityEngine.Object.Destroy(roid);
                    asteroids.RemoveAt(i);
                    continue;
                }
                asteroids[i].state = 0;
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
