using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonCollision : MonoBehaviour
{
    //
    private FMOD.Studio.EventInstance SonHit;
    private FMOD.Studio.EventInstance SonUseRing;
    private FMOD.Studio.EventInstance Music;
    private FMOD.Studio.EventInstance PiedMarche;

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

        Music.start();

        rb = GetComponent<Rigidbody>();
        movementscript = GetComponent<PlayerMovements>();
        
    }

    // Update is called once per frame
    void Update()
    {
        Music.setParameterByName("Hauteur", transform.position.y-250f);
        velocity = rb.velocity.magnitude;
;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("collision");
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
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            PiedMarche.setParameterByName("TypeSol", 1);

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
