using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotate : MonoBehaviour
{
    private int rotationDirection;
    private float rotationSpeed;
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
}
