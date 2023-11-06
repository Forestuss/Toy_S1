using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SizeManager : MonoBehaviour
{
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
        }
    }
}
