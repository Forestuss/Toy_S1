using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugRays : MonoBehaviour
{
    private GameObject player;
    private Rigidbody rb;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = player.GetComponent<Rigidbody>();
    }
    private void Update()
    {
        Debug.DrawRay(player.transform.position, rb.velocity.normalized * 3, Color.magenta);
        Debug.DrawLine(player.transform.position, transform.position, Color.yellow);
    }
}
