using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonCollision : MonoBehaviour
{
    private FMOD.Studio.EventInstance SonHit;
    private FMOD.Studio.EventInstance SonUseRing;
    private FMOD.Studio.EventInstance Music;
    private FMOD.Studio.EventInstance PiedMarche;
    private FMOD.Studio.EventInstance Slide;
    private Rigidbody rb;
    private float velocity;
    private PlayerMovements movementscript;



    // Start is called before the first frame update
    void Start()
    {
        SonHit = FMODUnity.RuntimeManager.CreateInstance("event:/PlayerBehave/Hit");
        SonUseRing = FMODUnity.RuntimeManager.CreateInstance("event:/RingBehave/UseRing");
        Music = FMODUnity.RuntimeManager.CreateInstance("event:/WorldBehave/Ambiance");
        PiedMarche = FMODUnity.RuntimeManager.CreateInstance("event:/PlayerBehave/MoveOnFloor");
        Slide = FMODUnity.RuntimeManager.CreateInstance("event:/PlayerBehave/Slide");
        Music.start();
        Slide.start();  
        rb = GetComponent<Rigidbody>();
        movementscript = GetComponent<PlayerMovements>();
        
    }

    // Update is called once per frame
    void Update()
    {
        Music.setParameterByName("Hauteur", transform.position.y-250f);
        if (rb.velocity.magnitude>= 60.0f && movementscript.isGrounded)
        {
            Slide.setParameterByName("Volume", 1);
        }
        else
        {
            Slide.setParameterByName("Volume", 0);
        }
        Slide.setParameterByName("Vitesse", rb.velocity.magnitude);
        velocity = rb.velocity.magnitude;

    }

    private void OnCollisionEnter(Collision collision)
    {
        SonHit.setParameterByName("F2V", velocity);
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
        else if (collision.gameObject.CompareTag("Wall"))
        {
            SonHit.setParameterByName("Type", 1);
            SonHit.start();
        }
        else if (collision.gameObject.CompareTag("Tree"))
        {
            SonHit.setParameterByName("Type", 2);
            SonHit.start();
        }
        else if (collision.gameObject.CompareTag("Bouncer"))
        {
            SonHit.setParameterByName("Type", 3);
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            PiedMarche.setParameterByName("TypeSol", 1);
            Slide.setParameterByName("TypeSol", 1);


        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            PiedMarche.setParameterByName("TypeSol", 1);
            Slide.setParameterByName("TypeSol", 1);
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
    private void Footstep()
    {
        PiedMarche.start();
    }
}
