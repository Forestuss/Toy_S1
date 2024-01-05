using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonCollision : MonoBehaviour
{
    private FMOD.Studio.EventInstance SonHit;
    private FMOD.Studio.EventInstance SonUseRing;
    private Rigidbody rb;
    private float velocity;
    private PlayerMovements movementscript;

    // Start is called before the first frame update
    void Start()
    {
        SonHit = FMODUnity.RuntimeManager.CreateInstance("event:/PlayerBehave/Hit");
        SonUseRing = FMODUnity.RuntimeManager.CreateInstance("event:/RingBehave/UseRing");
        rb = GetComponent<Rigidbody>();
        movementscript = GetComponent<PlayerMovements>();
        
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
            SonHit.start();
            if (movementscript.isGrounded)
            {

                

            }
            
        }
            
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("RingsSound"))
        {
            SonUseRing.setParameterByName("F2V", velocity);
            SonUseRing.start();
        }
    }
}
