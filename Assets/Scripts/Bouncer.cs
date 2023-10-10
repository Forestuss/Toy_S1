using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bouncer : MonoBehaviour
{
    [SerializeField]
    private float ImpulseForce = 20f;
    private void OnCollisionEnter(Collision collision)
    {
        Vector3 force = Vector3.up * ImpulseForce;
        collision.rigidbody.AddForce(force, ForceMode.Impulse);
    }
}
