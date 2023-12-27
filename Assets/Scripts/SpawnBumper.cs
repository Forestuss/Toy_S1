using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBumper : MonoBehaviour
{
    [SerializeField] private GameObject bloc;
    [SerializeField] private GameObject Target;

    [Header("Bumper Charges")]
    public int bumperMaxCharge; //Nombre de charges maximum de bumper à placer. Si 0: charges illimitées
    public float bumperCooldown; //Le temps d'attente avant de pouvoir placer un autre bumper

    [Header("bumper Time/Reset")]
    public int bumperChargeOnGround; //Nombre de charges récupérées en touchant le sol
    public int bumperChargeOnBumper; //Nombre de charges récupérées en touchant un Bumper
    public int bumperChargeOnLiquid; //Nombre de charges récupérées en touchant une Bulle

    [Header("Have Fun")]
    [SerializeField] private bool isUnlimited; //yolo, pas de restrictions

    [Header("Debug")]
    [SerializeField] private float bumperCharge;
    [SerializeField] private float timerCooldown;

    private Movements movementScript;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        movementScript = GetComponent<Movements>();

        bumperCharge = bumperMaxCharge;
        timerCooldown = bumperCooldown;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1) && (timerCooldown <= 0 || isUnlimited) && (bumperCharge > 0 || isUnlimited || bumperMaxCharge == 0))
        {
            Vector3 rayDirection = (Target.transform.position - transform.position).normalized;
            RaycastHit hit;

            if (Physics.Raycast(transform.position, rayDirection, out hit, 10))
            {
                Instantiate(bloc, hit.point, Camera.main.transform.rotation);
                Debug.Log("special instantiate");
            }

            else
            {
                Instantiate(bloc, Target.transform.position, Camera.main.transform.rotation);
            }

            timerCooldown = bumperCooldown;

            if (bumperCharge > 0)
            {
                bumperCharge -= 1;
                Debug.Log("Bumper restantes: " + bumperCharge);
            }
        }
    }

    private void FixedUpdate()
    {
        if (movementScript.isGrounded)
        {
            CheckBumperReset("Ground");
        }

        if (timerCooldown > 0)
        {
            timerCooldown -= Time.deltaTime;
        } 
    }

    public void CheckBumperReset(string Type)
    {
        if (Type == "Bouncer")
        {
            bumperCharge = Mathf.Clamp(bumperCharge + bumperChargeOnBumper, 0, bumperMaxCharge);
            //Debug.Log(bumperChargeOnBumper + " Bumper rechargés ! (Bumper)");
        }

        if (Type == "Ground")
        {
            bumperCharge = Mathf.Clamp(bumperCharge + bumperChargeOnGround, 0, bumperMaxCharge);
            //Debug.Log(bumperChargeOnGround + " Bumper rechargés ! (Sol)");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ring"))
        {
            CheckBumperReset("Ring");
            return;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Bouncer"))
        {
            CheckBumperReset("Bouncer");
            return;
        }
    }
}
