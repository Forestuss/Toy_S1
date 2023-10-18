using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BlocVanish : MonoBehaviour
{
    public float liquidVanish;
    public float bumperVanish;
    private float timer;

    public bool isResetOnCollision;
    public bool isResetOnTrigger;

    private void Awake()
    {
        if (gameObject.CompareTag("BoostLiquid"))
        {
            timer = liquidVanish;
        }

        if (gameObject.CompareTag("Bouncer"))
        {
            timer = bumperVanish;
        }
    }

    private void FixedUpdate()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter(Collider Liquid)
    {
        if (isResetOnTrigger)
        {
            timer = liquidVanish;
        }
    }

    private void OnCollisionEnter(Collision Bumper)
    {
        if (isResetOnCollision)
        {
            timer = bumperVanish;
        }
    }
}
