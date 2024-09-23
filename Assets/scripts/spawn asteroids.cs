using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Profiling;
using static UnityEngine.GridBrushBase;
using TMPro;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;
using Vector4 = UnityEngine.Vector4;




public class SpawnAsteroids : MonoBehaviour
{
    public float SpeedMin = 0.5f;
    public float SpeedMax = 7f;
    public int AsteroidsAmount = 1;
    public int MaxSplits = 3;
    public int points = 0;
    public GameObject asteroidObj;
    private List<AsteroidData> asteroids = new List<AsteroidData> { };
    public TextMeshProUGUI currentPoints;
    public playermovement playermovement;


    private class AsteroidData
    {
        public GameObject asteroid;
        public int state = 0;  // 0 = entering screen  1 = onscreen
        public int split = 0;
        public int age = 0;
        public AsteroidData(GameObject roid, int splited = 0)
        {
            asteroid = roid;
            split = splited;
        }
    }

    public int getPoints()
    {
        return points;
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
        Vector2 targetpos = Camera.main.ViewportToWorldPoint(new Vector2(targetx, targety));
        float deltaX = targetpos.x - worldpos.x;
        float deltaY = targetpos.y - worldpos.y;
        float angle = Mathf.Atan2(deltaY, deltaX);

        float xVelocity = Mathf.Cos(angle);
        float yVelocity = Mathf.Sin(angle);

        return new Vector2(xVelocity, yVelocity);
    }


    List<AsteroidData> SplitAsteroid(GameObject asteroid)
    {
        int parentSplit = asteroid.GetComponent<Asteroid>().split;

        if (parentSplit == 0)      // Large asteroid
            points += 10;
        else if (parentSplit == 1) // Medium asteroid
            points += 50;
        else if (parentSplit == 2) // Small asteroid
            points += 100;

        if (parentSplit == MaxSplits)
        {
            return new List<AsteroidData> { };
        }

        Vector2 pos = asteroid.transform.position;
        float rotationradians = (asteroid.transform.rotation.eulerAngles.z % 360) * Mathf.Deg2Rad;
        float rotationRight = rotationradians + (Mathf.PI / 2);
        float rotationLeft = rotationradians - (Mathf.PI / 2);
        Vector2 rightVec = new Vector2(Mathf.Cos(rotationRight), Mathf.Sin(rotationRight));
        Vector2 leftVec = new Vector2(Mathf.Cos(rotationLeft), Mathf.Sin(rotationLeft));

        Vector2 size = asteroid.GetComponent<Renderer>().bounds.size / 1.5f;
        print(size);
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

        int split = parentSplit + 1;

        roid1.GetComponent<Asteroid>().split = split;
        roid2.GetComponent<Asteroid>().split = split;


        roid1.transform.localScale = roid1.transform.localScale / (1.5f * (split));
        roid2.transform.localScale = roid2.transform.localScale / (1.5f * (split));

        Dictionary<int, Color> colors = new Dictionary<int, Color>()
        {
            {0, Color.red },
            {1, Color.green},
            {2, Color.blue},
            {3, Color.yellow},
            {4, Color.black}
        };


        roid1.GetComponent<SpriteRenderer>().color = colors[split];
        roid2.GetComponent<SpriteRenderer>().color = colors[split];

        return new List<AsteroidData> { new AsteroidData(roid1,1), new AsteroidData(roid2,1) };
    }


    // Start is called before the first frame update
    void Start()
    {
  
     AddAstroid();
        
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

    int CountBigAsteroids()
    {
        int count = 0;
        for (int i = 0;i < asteroids.Count; i++)
        {
            if (asteroids[i].split == 0)
            {
                count++;
            }
        }
        return count;
    }
    void CleanUpDeadAsteroids() //cleans up asteroids are old and not on screen 
    {
        for (int i = 0; i < asteroids.Count; i++)
        {
            asteroids[i].age += 5;
            if (asteroids[i].age >= 20)
            {
                Vector2 pos = asteroids[i].asteroid.transform.position;
                if (pos.x < 0 | pos.x > 1 | pos.y < 0 | pos.y > 1)    //if age is greater than 20 and offscreen delete it
                {
                    UnityEngine.Object.Destroy(asteroids[i].asteroid);
                    asteroids.RemoveAt(i);
                    i--;
                }
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (CountBigAsteroids() <= 5)
        {   
            int random_roid_amount = UnityEngine.Random.Range(0, 3);
            for (int i = 0;i < random_roid_amount; i++)
            {
                AddAstroid();
            }
        }
        // I think if you shoot bullets after you die they still count for points so this SHOULD fix that..
        if (!playermovement.isPlayerDead)
        {
            currentPoints.text = points + " POINTS";
        }

        //bug if a asteroid spawns outside and then gets bumped by another befor it gos in the screen it will drift off screen forever
        for (int i = 0; i < asteroids.Count; i++)
        {
            GameObject roid = asteroids[i].asteroid;

            Vector3 size = roid.GetComponent<Renderer>().bounds.size;
            Vector3 pos = Camera.main.WorldToViewportPoint(roid.transform.position);
            bool dead = roid.GetComponent<Asteroid>().dead;
            if (dead)
            {
                asteroids.AddRange(SplitAsteroid(roid));
                UnityEngine.Object.Destroy(roid);
                asteroids.RemoveAt(i);
                i--;
                continue;
            }

            if (asteroids[i].state == 0) //dont despawn asteroids when they havent been on screen yet
            {
                if (!(pos.x < 0 | pos.x > 1 | pos.y < 0 | pos.y > 1)) //if asteroid is on screen
                {
                    asteroids[i].state = 1;
                }

                continue;
            }
            if (pos.x < 0 | pos.x > 1 | pos.y < 0 | pos.y > 1)  //if asteroid is off screen
            {
                if (asteroids[i].asteroid.GetComponent<Asteroid>().split != 0) //delete small split ateroids but respawn full ones
                {
                    UnityEngine.Object.Destroy(roid);
                    asteroids.RemoveAt(i);
                    continue;
                }
                asteroids[i].age = 0;
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
