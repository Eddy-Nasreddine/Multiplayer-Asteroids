using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playermovement : MonoBehaviour
{
    private float horizontal;
    private float vertical;
    public float speed=5;
    public Rigidbody2D rb;
    public float rotationSpeed=360;
    // Start is called before the first frame update
    void Start()
    {
        rb=GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {   

        horizontal = Input.GetAxisRaw("Horizontal");
        
        vertical = Input.GetAxisRaw("Vertical");
        Vector2 direction = new Vector2(horizontal, vertical);
        float inputMagnitude = Mathf.Clamp01(direction.magnitude);
        direction.Normalize();
        transform.Translate(direction * speed * inputMagnitude * Time.deltaTime, Space.World);
        //transform.rotation.z += 1;
        if (direction != Vector2.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(Vector3.forward, direction);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
