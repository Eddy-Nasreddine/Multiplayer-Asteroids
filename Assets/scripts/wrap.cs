using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wrap : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 viewportPosition = Camera.main.WorldToViewportPoint(transform.position);
        Vector3 moveAdjust = Vector3.zero;
        if (viewportPosition.x < 0)
        {
            viewportPosition.x += 1;
        }
        else if (viewportPosition.y < 0)
        {
            viewportPosition.y += 1;
        } else if (viewportPosition.x > 1){
            viewportPosition.x -= 1;
        } else if ((viewportPosition.y > 1)) {
            viewportPosition.y -= 1;
        }
        transform.position = Camera.main.ViewportToWorldPoint(viewportPosition + moveAdjust);
    }
}
