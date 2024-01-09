using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetTimer : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ring"))
        {
            other.transform.GetComponent<DestroyTimer>().tempsDestruct = other.transform.GetComponent<DestroyTimer>().tempsAvantDestruct;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Bouncer")
        {
            other.transform.GetComponent<DestroyTimer>().tempsDestruct = other.transform.GetComponent<DestroyTimer>().tempsAvantDestruct;
        }
    }
}
