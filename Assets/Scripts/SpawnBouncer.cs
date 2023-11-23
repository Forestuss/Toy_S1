using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBouncer : MonoBehaviour
{
    [SerializeField] private GameObject bloc;
    [SerializeField] private GameObject Target;

    [Header("Bumper Charges")]
    public int bumperMaxCharge; //Nombre de charges maximum de bumper � placer. Si 0: charges illimit�es
    public float bumperCooldown; //Le temps d'attente avant de pouvoir placer un autre bumper

    [Header("bumper Time/Reset")]
    public int bumperChargeOnGround; //Nombre de charges r�cup�r�es en touchant le sol
    public int bumperChargeOnBumper; //Nombre de charges r�cup�r�es en touchant un Bumper
    public int bumperChargeOnLiquid; //Nombre de charges r�cup�r�es en touchant une Bulle

    public float bumperChargeTime; //Temps avant que X nombre de Bumper nous soit donn� (une fois le timer arriv� au max, le syst�me attend que le joueur n'est plus de Bumper). Mettre 0 pour d�sactiver
    public float bumperChargeAmount; //X nombre de Bumper donn�s par le ChargeTime (pr�f�rablement le nombre de Bumper max)

    [Header("Have Fun")]
    [SerializeField] private bool isUnlimited; //yolo, pas de restrictions

    [Header("Debug")]
    [SerializeField] private float bumperCharge;
    [SerializeField] private float timerCooldown;
    [SerializeField] private float timerCharge;

    [NonSerialized] public bool isCollidedLiquid;
    [NonSerialized] public bool isCollidedBumper;
    [NonSerialized] public bool isCollidedGround;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        bumperCharge = bumperMaxCharge;
        timerCooldown = bumperCooldown;
        timerCharge = bumperChargeTime;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1) && (timerCooldown <= 0 || isUnlimited) && (bumperCharge > 0 || isUnlimited || bumperMaxCharge == 0))
        {
            Instantiate(bloc, Target.transform.position, Camera.main.transform.rotation);

            timerCooldown = bumperCooldown;

            if (bumperCharge > 0)
            {
                bumperCharge -= 1;
                Debug.Log("Bump restants: " + bumperCharge);
            }

        }


        if (bumperCharge == 0 && timerCharge <= 0 && bumperChargeTime != 0)
        {
            timerCharge = bumperChargeTime;
            bumperCharge = bumperChargeAmount;
            Debug.Log(bumperChargeAmount + " Bumper recharg� ! (charge time)");
        }

        
        if (isCollidedBumper)
        {
            CheckBumperReset("Bouncer");
        }
        if (isCollidedGround)
        {
            CheckBumperReset("Ground");
        }
        if (isCollidedLiquid)
        {
            CheckBumperReset("Liquid");
        }
        
    }

    private void FixedUpdate()
    {
        if (timerCooldown > 0)
        {
            timerCooldown -= Time.deltaTime;
        }

        if (timerCharge > 0) 
        {
            timerCharge -= Time.deltaTime;
        }   
    }

    public void CheckBumperReset(string Type)
    {
        if (Type == "Bouncer")
        {
            bumperCharge = Mathf.Clamp(bumperCharge + bumperChargeOnBumper, 0, bumperMaxCharge);
            Debug.Log(bumperChargeOnBumper + " Bumper recharg�s ! (Bumper)");
            isCollidedBumper = false;
        }

        if (Type == "Ground")
        {
            bumperCharge = Mathf.Clamp(bumperCharge + bumperChargeOnGround, 0, bumperMaxCharge);
            Debug.Log(bumperChargeOnGround + " Bumper recharg�s ! (Sol)");
            isCollidedGround = false;
        }

        if (Type == "Liquid")
        {
            bumperCharge = Mathf.Clamp(bumperCharge + bumperChargeOnLiquid, 0, bumperMaxCharge);
            Debug.Log(bumperChargeOnLiquid + " Bumper recharg�s ! (Bulle)");
            isCollidedLiquid = false;
        }
    }
}
