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
        Vector3 moveAdjust = Vector3.zero;
        if (viewportPosition.x < 0)
        {
            Destroy(gameObject);
        }
        else if (viewportPosition.y < 0)
        {
            Destroy(gameObject);
        }
        else if (viewportPosition.x > 1)
        {
            Destroy(gameObject);
        }
        else if ((viewportPosition.y > 1))
        {
            Destroy(gameObject);
        }
        gameObject.transform.position = Camera.main.ViewportToWorldPoint(viewportPosition + moveAdjust);
        
    }
}
