using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

public class rotate : MonoBehaviour
{
    private int rotationDirection;
    private float rotationSpeed;
    [HideInInspector]
    public int dead = 0; // 0 dead  1 alive  2 ignore
    // Start is called before the first frame update
    void Start()
    {
        rotationDirection = UnityEngine.Random.Range(1, 3) == 1 ? 1 : -1;
        rotationSpeed = UnityEngine.Random.Range(0.01f, 0.2f);
    }

    // Update is called once per frame
    void Update()
    {
        float rotation = rotationSpeed * rotationDirection;
        transform.Rotate(0.0f, 0.0f, rotation, Space.World);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<rotate>().dead == 2) return;
        if (dead == 2) return;


        dead = 1;
    }




}
