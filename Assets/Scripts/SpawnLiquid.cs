using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnLiquid : MonoBehaviour
{
    [SerializeField]
    private GameObject bubble;
    [SerializeField]
    private GameObject Target;

    [Header("Liquid Charges")]
    public int liquidMaxCharge; //Nombre de charges maximum de otherFusionObject � placer. Si 0: charges illimit�es
    public float liquidCooldown; //Le temps d'attente avant de pouvoir placer une autre bubble

    [Header("Liquid Time/Reset")]
    public int liquidChargeOnGround; //Nombre de charges r�cup�r�es en touchant le sol
    public int liquidChargeOnBumper; //Nombre de charges r�cup�r�es en touchant un Bumper
    public int liquidChargeOnLiquid; //Nombre de charges r�cup�r�es en touchant une Bulle

    public float liquidChargeTime; //Temps avant que X nombre de bulle nous soit donn� (une fois le timer arriv� au max, le syst�me attend que le joueur n'est plus de bulles). Mettre 0 pour d�sactiver
    public float liquidChargeAmount; //X nombre de bulle donn�s par le ChargeTime (pr�f�rablement le nombre de bulles max)

    [Header("Have Fun")]
    [SerializeField] private bool isUnlimited; //yolo, pas de restrictions

    [Header("Debug")]
    [SerializeField] private float liquidCharge;
    [SerializeField] private float timerCooldown;
    [SerializeField] private float timerCharge;

    [NonSerialized] public bool isCollidedLiquid;
    [NonSerialized] public bool isCollidedBumper;
    [NonSerialized] public bool isCollidedGround;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        liquidCharge = liquidMaxCharge;
        timerCooldown = liquidCooldown;
        timerCharge = liquidChargeTime;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && (timerCooldown <= 0 || isUnlimited) && (liquidCharge > 0 || isUnlimited || liquidMaxCharge == 0))
        {
            Instantiate(bubble, Target.transform.position, Quaternion.identity);
            timerCooldown = liquidCooldown;

            if (liquidCharge > 0) 
            {
                liquidCharge -= 1;
                //Debug.Log("Bulles restantes: " +  liquidCharge);
            }

        }

        if (liquidCharge == 0 && timerCharge <= 0 && liquidChargeTime != 0)
        {
            timerCharge = liquidChargeTime;
            liquidCharge = liquidChargeAmount;
            //Debug.Log(liquidChargeAmount + " Bulles recharg�e ! (charge time)");
        }


        if (isCollidedBumper)
        {
            CheckLiquidReset("Bouncer");
        }
        if (isCollidedGround)
        {
            CheckLiquidReset("Ground");
        }
        if (isCollidedLiquid)
        {
            CheckLiquidReset("Liquid");
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

    public void CheckLiquidReset(string Type)
    {
        if (Type == "Bouncer")
        {
            liquidCharge = Mathf.Clamp(liquidCharge + liquidChargeOnBumper, 0, liquidMaxCharge);
            //Debug.Log(liquidChargeOnBumper + " Bulles recharg�es ! (Bumper)");
            isCollidedBumper = false;
        }

        if (Type == "Ground")
        {
            liquidCharge = Mathf.Clamp(liquidCharge + liquidChargeOnGround, 0, liquidMaxCharge);
            //Debug.Log(liquidChargeOnGround + " Bulles recharg�es ! (Sol)");
            isCollidedGround = false;
        }

        if (Type == "Liquid")
        {
            liquidCharge = Mathf.Clamp(liquidCharge + liquidChargeOnLiquid, 0, liquidMaxCharge);
            //Debug.Log(liquidChargeOnLiquid + " Bulles recharg�es ! (Bulle)");
            isCollidedLiquid = false;
        }
    }
}
                                                                      