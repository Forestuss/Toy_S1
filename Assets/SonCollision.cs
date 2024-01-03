using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonCollision : MonoBehaviour
{
    private FMOD.Studio.EventInstance SonHit;
    private Rigidbody rb;
    private float velocity;

    // Start is called before the first frame update
    void Start()
    {
        SonHit = FMODUnity.RuntimeManager.CreateInstance("event:/PlayerBehave/Hit");
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        velocity = rb.velocity.magnitude;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Bouncer"))
        {
            SonHit.setParameterByName("F2V", velocity);
            SonHit.start();
        }
            
    }
}
