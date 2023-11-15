using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SizeManager : MonoBehaviour
{
    public float sizeRatioOnSpeed; //Mettre à 0 pour annuler la fonctionnalité
    public float sizeRatioOnTime; //Mettre à 0 pour annuler la fonctionnalité

    private Rigidbody _RB;
    private float originalScale;

    [DoNotSerialize] public BlocVanish VanishScript;

    [Header("SpeedManager")]
    private GameObject playerRB;

    private float timer;
    private float force;
    private Vector3 result;

    private void Awake()
    {
        playerRB = GameObject.FindGameObjectWithTag("Player");
    }
    private void Start()
    {
        timer = sizeRatioOnTime;

        if (sizeRatioOnSpeed != 0)
        {
            force = Mathf.Clamp(sizeRatioOnSpeed * playerRB.GetComponent<Rigidbody>().velocity.magnitude / 100, 1, 10);
            Vector3 result = new Vector3(transform.localScale.x * force, transform.localScale.y * force, transform.localScale.z * force);
            transform.localScale = result;
        }
    }

    private void FixedUpdate()
    {
        if (sizeRatioOnTime != 0)
        {
            float sizeX = (transform.localScale.x + (transform.localScale.x * sizeRatioOnTime));
            float sizeY = (transform.localScale.y + (transform.localScale.y * sizeRatioOnTime));
            float sizeZ = (transform.localScale.z + (transform.localScale.z * sizeRatioOnTime));
            transform.localScale = new Vector3(sizeX, sizeY, sizeZ);

            if (sizeRatioOnTime > 0)
            {
                timer -= Time.deltaTime;
            }
        }
    }
}
