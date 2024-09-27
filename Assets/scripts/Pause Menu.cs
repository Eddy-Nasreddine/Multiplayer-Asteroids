using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            this.GetComponent<Canvas>().enabled = !this.GetComponent<Canvas>().enabled;
            Time.timeScale = Time.timeScale == 1 ? 0 : 1;
        }
    }
}
