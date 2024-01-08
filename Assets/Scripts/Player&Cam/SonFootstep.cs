using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonFootstep : MonoBehaviour
{
    private FMOD.Studio.EventInstance PiedMarche;
    // Start is called before the first frame update
    void Start()
    {
        PiedMarche = FMODUnity.RuntimeManager.CreateInstance("event:/PlayerBehave/MoveOnFloor");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionStay(Collision collision)
    {
        
    }
    private void Footstep()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/PlayerBehave/MoveOnFloor");
    }
}
