using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collisionTest : MonoBehaviour
{
    void onTriggerEnter(Collider other) {
        if (other.gameObject.tag == "dummySquare") { }
    
    }
    void onTriggerStay(Collider other) {
        if (other.gameObject.tag == "dummySquare") { }

    }
    void onTriggerExit(Collider other) {
        if (other.gameObject.tag == "dummySquare") { }

    }
}
