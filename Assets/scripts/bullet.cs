using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("asteroid"))
        {
            Destroy(gameObject);
        }

    }
    void Update()
    {
       
        Vector3 viewportPosition = Camera.main.WorldToViewportPoint(gameObject.transform.position);
        if (viewportPosition.x < 0 | viewportPosition.x > 1 | viewportPosition.y < 0 | viewportPosition.y > 1)
        {
            Destroy(gameObject);
        }  
    }
}
