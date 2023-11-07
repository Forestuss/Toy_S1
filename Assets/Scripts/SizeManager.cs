using System.Collections;
using System.Collections.Generic;
<<<<<<< Updated upstream
using Unity.VisualScripting;
=======
using System.Runtime.CompilerServices;
>>>>>>> Stashed changes
using UnityEngine;

public class SizeManager : MonoBehaviour
{
<<<<<<< Updated upstream
    public float sizeRatioOnSpeed; //Mettre à 0 pour annuler la fonctionnalité
    public float sizeRatioOnTime; //Mettre à 0 pour annuler la fonctionnalité

    private Rigidbody _RB;
    private float originalScale;

    [DoNotSerialize] public BlocVanish VanishScript;

    private void Start()
    {
        if (sizeRatioOnSpeed != 0)
        {
            _RB = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
            float sizeresult = transform.localScale.x + (transform.localScale.x * (sizeRatioOnSpeed * _RB.velocity.magnitude));
            transform.localScale = new Vector3(sizeresult, sizeresult, sizeresult);
            originalScale = sizeresult;

            VanishScript = gameObject.GetComponent<BlocVanish>();
=======

    public float sizeRatioOnTime;
    public float sizeRatioOnSpeed;

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

        if (sizeRatioOnSpeed > 0)
        {
            force = sizeRatioOnSpeed * playerRB.GetComponent<Rigidbody>().velocity.magnitude;
            Debug.Log(playerRB.GetComponent<Rigidbody>().velocity.magnitude);
            Vector3 result = new Vector3(transform.localScale.x * force, transform.localScale.y * force, transform.localScale.z * force);
            transform.localScale = result;
>>>>>>> Stashed changes
        }
    }

    private void FixedUpdate()
    {
<<<<<<< Updated upstream
        if (sizeRatioOnTime != 0)
        {
            float sizeX = (transform.localScale.x + (transform.localScale.x * sizeRatioOnTime));
            float sizeY = (transform.localScale.y + (transform.localScale.y * sizeRatioOnTime));
            float sizeZ = (transform.localScale.z + (transform.localScale.z * sizeRatioOnTime));
            transform.localScale = new Vector3(sizeX, sizeY, sizeZ);
=======
        if (sizeRatioOnTime > 0)
        {
            timer -= Time.deltaTime;
>>>>>>> Stashed changes
        }
    }
}
