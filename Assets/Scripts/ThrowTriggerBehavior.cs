using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowTriggerBehavior : MonoBehaviour
{
    [NonSerialized] public bool isTriggerStay = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isTriggerStay = true;
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        if (other.CompareTag("Player"))
        {
            isTriggerStay = false;
        }
    }
}
