using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    public GameObject player;

    private Quaternion tempoRotation;

    // Update is called once per frame
    void Update()
    {
        float tolerance = 0.2f;
        if (player.GetComponent<Rigidbody>().velocity.x > tolerance || player.GetComponent<Rigidbody>().velocity.y > tolerance || player.GetComponent<Rigidbody>().velocity.z > tolerance)
        {
            transform.rotation = Quaternion.LookRotation(player.GetComponent<Rigidbody>().velocity, Vector3.up);
            tempoRotation = transform.rotation;
        }

        else if (player.GetComponent<Rigidbody>().velocity.x < -tolerance || player.GetComponent<Rigidbody>().velocity.y < -tolerance || player.GetComponent<Rigidbody>().velocity.z < -tolerance)
        {
            transform.rotation = Quaternion.LookRotation(player.GetComponent<Rigidbody>().velocity, Vector3.up);
            tempoRotation = transform.rotation;
        }
        
        else
        {
            transform.rotation = tempoRotation;
        }
    }
}
