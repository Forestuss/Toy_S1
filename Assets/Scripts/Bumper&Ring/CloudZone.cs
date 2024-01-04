using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudZone : MonoBehaviour
{
    [SerializeField] private float _cloudForce;
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.attachedRigidbody.AddForce(transform.up * _cloudForce, ForceMode.Force);
            Debug.Log("working");
        }
    }
}
