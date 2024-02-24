using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionCode : MonoBehaviour
{
    void onTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Jumppad")
        {
            print("Enter");
        }
    }
    void OnTriggerStay(Collider other)
    {

    }
    
    void OnTriggerExit(Collider other)
    {

    }
}
