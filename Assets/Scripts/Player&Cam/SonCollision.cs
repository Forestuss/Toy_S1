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

    private bool isCollisionBefore;
    private float compteur;
    public float EspacementHit;



    // Start is called before the first frame update
    void Start()
    {
        SonHit = FMODUnity.RuntimeManager.CreateInstance("event:/PlayerBehave/Hit");
        SonUseRing = FMODUnity.RuntimeManager.CreateInstance("event:/RingBehave/UseRing");
        Music = FMODUnity.RuntimeManager.CreateInstance("event:/WorldBehave/Ambiance");
        PiedMarche = FMODUnity.RuntimeManager.CreateInstance("event:/PlayerBehave/MoveOnFloor");
        isCollisionBefore = false;
        Music.start();

        rb = GetComponent<Rigidbody>();
        movementscript = GetComponent<PlayerMovements>();
        
    }

    // Update is called once per frame
    void Update()
    {
        Music.setParameterByName("Hauteur", transform.position.y-245f);
        velocity = rb.velocity.magnitude;

        //Reset du compteur de hit pour eviter la surcharge
        compteur=Time.deltaTime;
        if (compteur==EspacementHit)
        {
            isCollisionBefore = false;
        }
    }
    private void Hit(Collision collision)
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
    private void OnCollisionEnter(Collision collision)
    {
        if (!isCollisionBefore)
        {
            isCollisionBefore = true;
            SonHit.setParameterByName("F2V", velocity);
            Hit(collision);
        }
        else
        {
            SonHit.setParameterByName("F2V",velocity/1.5f);
            Hit(collision);
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
        if (movementscript.isGrounded && rb.velocity.magnitude<60)
        {
            PiedMarche.start();
        }
        
    }
}
