using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    [SerializeField] private float bulletlife = 5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    //private void Awake()
    //{
    //    Destroy(gameObject);
    //}

    // Update is called once per frame
    void Update()
    {
       
        Vector3 viewportPosition = Camera.main.WorldToViewportPoint(gameObject.transform.position);
        if (viewportPosition.x < 0 | viewportPosition.x > 1 | viewportPosition.y < 0 | viewportPosition.y > 1)
        {
            Destroy(gameObject);
        }  
    }
}
