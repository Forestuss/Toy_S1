using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    public GameObject player;

    private Quaternion tempoRotation;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(player.GetComponent<Rigidbody>().velocity);
        float tolerance = 1f;
        if (player.GetComponent<Rigidbody>().velocity.x > tolerance || player.GetComponent<Rigidbody>().velocity.y > tolerance || player.GetComponent<Rigidbody>().velocity.z > tolerance)
        {
            transform.rotation = Quaternion.LookRotation(player.GetComponent<Rigidbody>().velocity, Vector3.up);
            tempoRotation = transform.rotation;


            Debug.Log("positif");
        }

        else if (player.GetComponent<Rigidbody>().velocity.x < -tolerance || player.GetComponent<Rigidbody>().velocity.y < -tolerance || player.GetComponent<Rigidbody>().velocity.z < -tolerance)
        {
            transform.rotation = Quaternion.LookRotation(player.GetComponent<Rigidbody>().velocity, Vector3.up);
            tempoRotation = transform.rotation;

            Debug.Log("négatif");
        }
        
        else
        {
            transform.rotation = tempoRotation;
            Debug.Log(tempoRotation);
        }


        

        Debug.DrawRay(player.transform.position, player.GetComponent<Rigidbody>().velocity);
        

        //Vector3 direction = player.GetComponent<Rigidbody>().velocity - player.transform.position;

        //target.transform.position = direction + transform.position;

        //transform.LookAt(target.transform.position, Vector3.up);
    }
}
