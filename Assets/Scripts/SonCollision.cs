using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonCollision : MonoBehaviour
{
    private FMOD.Studio.EventInstance SonHit;
    private FMOD.Studio.EventInstance SonUseRing;
    private FMOD.Studio.EventInstance Music;
    private Rigidbody rb;
    private float velocity;
    private PlayerMovements movementscript;


    // Start is called before the first frame update
    void Start()
    {
        SonHit = FMODUnity.RuntimeManager.CreateInstance("event:/PlayerBehave/Hit");
        SonUseRing = FMODUnity.RuntimeManager.CreateInstance("event:/RingBehave/UseRing");
        Music = FMODUnity.RuntimeManager.CreateInstance("event:/WorldBehave/Ambiance");
        Music.start();
        rb = GetComponent<Rigidbody>();
        movementscript = GetComponent<PlayerMovements>();
        
    }

    // Update is called once per frame
    void Update()
    {
        Music.setParameterByName("Hauteur", transform.position.y);
        velocity = rb.velocity.magnitude;

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (movementscript.isGrounded)
            {
                SonHit.setParameterByName("Type", 0);
                SonHit.start();
            }
            else
            {
                SonHit.setParameterByName("Type", 1);
                SonHit.start();
            }
            
        }
        if (collision.gameObject.CompareTag("Wall"))
        {
            SonHit.setParameterByName("Type", 1);
            SonHit.start();
        }
        if (collision.gameObject.CompareTag("Wall"))
        {
            SonHit.setParameterByName("Type", 2);
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
