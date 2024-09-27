using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    private int rotationDirection;
    private float rotationSpeed;
    [HideInInspector]
    public bool dead = false;
    public int split = 0;
    // Start is called before the first frame update
    void Start()
    {
        rotationDirection = UnityEngine.Random.Range(1, 3) == 1 ? 1 : -1;
        rotationSpeed = UnityEngine.Random.Range(20f, 100f);
    }

    // Update is called once per frame
    void Update()
    {
        float rotation = rotationSpeed * rotationDirection * Time.deltaTime;
        transform.Rotate(0.0f, 0.0f, rotation, Space.World);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("bullet")){
            dead = true;
        }
    }




}
