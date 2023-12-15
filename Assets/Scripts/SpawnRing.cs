using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRing : MonoBehaviour
{
    [SerializeField] private GameObject ring;
    [SerializeField] private GameObject Target;

    [Header("Liquid Charges")]
    public int ringMaxCharge; //Nombre de charges maximum de ring � placer. Si 0: charges illimit�es
    public float ringCooldown; //Le temps d'attente avant de pouvoir placer un autre ring

    [Header("Liquid Time/Reset")]
    public int ringChargeOnGround; //Nombre de charges r�cup�r�es en touchant le sol
    public int ringChargeOnBumper; //Nombre de charges r�cup�r�es en touchant un Bumper
    public int ringChargeOnRing; //Nombre de charges r�cup�r�es en touchant un Ring

    public float ringChargeTime; //Temps avant que X nombre de ring nous soit donn� (une fois le timer arriv� au max, le syst�me attend que le joueur n'est plus de ring). Mettre 0 pour d�sactiver
    public float ringChargeAmount; //X nombre de ring donn�s par le ChargeTime (pr�f�rablement le nombre de ring max)

    [Header("Have Fun")]
    [SerializeField] private bool isUnlimited; //yolo, pas de restrictions

    [Header("Debug")]
    [SerializeField] private float ringCharge;
    [SerializeField] private float timerCooldown;
    [SerializeField] private float timerCharge;

    [NonSerialized] public bool isCollidedRing;
    [NonSerialized] public bool isCollidedBumper;
    [NonSerialized] public bool isCollidedGround;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        ringCharge = ringMaxCharge;
        timerCooldown = ringCooldown;
        timerCharge = ringChargeTime;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && (timerCooldown <= 0 || isUnlimited) && (ringCharge > 0 || isUnlimited || ringMaxCharge == 0))
        {
            Instantiate(ring, Target.transform.position, Quaternion.identity);
            timerCooldown = ringCooldown;

            if (ringCharge > 0) 
            {
                ringCharge -= 1;
                Debug.Log("Bulles restantes: " +  ringCharge);
            }

        }

        if (ringCharge == 0 && timerCharge <= 0 && ringChargeTime != 0)
        {
            timerCharge = ringChargeTime;
            ringCharge = ringChargeAmount;
            //Debug.Log(ringChargeAmount + " Ring recharg�s ! (charge time)");
        }


        if (isCollidedBumper)
        {
            CheckLiquidReset("Bouncer");
        }
        if (isCollidedGround)
        {
            CheckLiquidReset("Ground");
        }
        if (isCollidedRing)
        {
            CheckLiquidReset("Ring");
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
            ringCharge = Mathf.Clamp(ringCharge + ringChargeOnBumper, 0, ringMaxCharge);
            Debug.Log(ringChargeOnBumper + " Ring recharg�s ! (Bumper)");
            isCollidedBumper = false;
        }

        if (Type == "Ground")
        {
            ringCharge = Mathf.Clamp(ringCharge + ringChargeOnGround, 0, ringMaxCharge);
            Debug.Log(ringChargeOnGround + " Ring recharg�s ! (Sol)");
            isCollidedGround = false;
        }

        if (Type == "Ring")
        {
            ringCharge = Mathf.Clamp(ringCharge + ringChargeOnRing, 0, ringMaxCharge);
            Debug.Log(ringChargeOnRing + " Ring recharg�s ! (Ring)");
            isCollidedRing = false;
        }
    }
}
                                                                      