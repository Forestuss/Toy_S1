using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRing : MonoBehaviour
{
    [SerializeField] private GameObject ring;
    [SerializeField] private GameObject Target;

    [Header("Liquid Charges")]
    public int ringMaxCharge; //Nombre de charges maximum de ring à placer. Si 0: charges illimitées
    public float ringCooldown; //Le temps d'attente avant de pouvoir placer un autre ring

    [Header("Liquid Time/Reset")]
    public int ringChargeOnGround; //Nombre de charges récupérées en touchant le sol
    public int ringChargeOnBumper; //Nombre de charges récupérées en touchant un Bumper
    public int ringChargeOnRing; //Nombre de charges récupérées en touchant un Ring

    [Header("Have Fun")]
    [SerializeField] private bool isUnlimited; //yolo, pas de restrictions

    [Header("Debug")]
    [SerializeField] private float ringCharge;
    [SerializeField] private float timerCooldown;

    private Movements movementScript;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        movementScript = GetComponent<Movements>();

        ringCharge = ringMaxCharge;
        timerCooldown = ringCooldown;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && (timerCooldown <= 0 || isUnlimited) && (ringCharge > 0 || isUnlimited || ringMaxCharge == 0))
        {
            Vector3 rayDirection = (Target.transform.position - transform.position).normalized;
            RaycastHit hit;

            if (Physics.Raycast(transform.position, rayDirection, out hit, 10))
            {
                Instantiate(ring, hit.point, Camera.main.transform.rotation);
                Debug.Log("special instantiate");
            }

            else
            {
                Instantiate(ring, Target.transform.position, Camera.main.transform.rotation); 
            }

            timerCooldown = ringCooldown;

            if (ringCharge > 0) 
            {
                ringCharge -= 1;
                Debug.Log("Bulles restantes: " +  ringCharge);
            }
        }
    }

    private void FixedUpdate()
    {
        if (movementScript.isGrounded)
        {
            CheckRingReset("Ground");
        }

        if (timerCooldown > 0)
        {
            timerCooldown -= Time.deltaTime;
        }
    }

    public void CheckRingReset(string Type)
    {
        if (Type == "Bouncer")
        {
            ringCharge = Mathf.Clamp(ringCharge + ringChargeOnBumper, 0, ringMaxCharge);
            //Debug.Log(ringChargeOnBumper + " Ring rechargés ! (Bumper)");
        }

        if (Type == "Ground")
        {
            ringCharge = Mathf.Clamp(ringCharge + ringChargeOnGround, 0, ringMaxCharge);
            //Debug.Log(ringChargeOnGround + " Ring rechargés ! (Sol)");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ring"))
        {
            CheckRingReset("Ring");
            return;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Bouncer"))
        {
            CheckRingReset("Bouncer");
            return;
        }
    }
}
                                                                      