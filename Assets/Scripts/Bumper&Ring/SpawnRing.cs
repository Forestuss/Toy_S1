using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRing : MonoBehaviour
{
    [Header("Instantiate Data")]
    [SerializeField] private GameObject _Ring;
    [SerializeField] private GameObject _Target;

    [Header("Ring Charges")]
    public int ringMaxCharge; //Nombre de charges maximum de ring à placer. Si 0: charges illimitées
    public float ringCooldown; //Le temps d'attente avant de pouvoir placer un autre ring

    [Header("Ring Reset")]
    public int ringChargeOnGround; //Nombre de charges récupérées en touchant le sol
    public int ringChargeOnBumper; //Nombre de charges récupérées en touchant un Bumper

    [Header("Debug")]
    public float ringCharge;
    [SerializeField] private float _timerCooldown;
    [SerializeField] private bool _isUnlimited;

    private PlayerMovements movementScript;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        movementScript = GetComponent<PlayerMovements>();

        ringCharge = ringMaxCharge;
        _timerCooldown = ringCooldown;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && (_timerCooldown <= 0 || _isUnlimited) && (ringCharge > 0 || _isUnlimited || ringMaxCharge == 0))
        {
            Vector3 rayDirection = (_Target.transform.position - transform.position).normalized;
            RaycastHit hit;

            if (Physics.Raycast(transform.position, rayDirection, out hit, 10))
            {
                Instantiate(_Ring, hit.point, Camera.main.transform.rotation);
                //Debug.Log("special instantiate");
            }

            else
            {
                Instantiate(_Ring, _Target.transform.position, Camera.main.transform.rotation);
            }

            _timerCooldown = ringCooldown;

            if (ringCharge > 0)
            {
                ringCharge -= 1;
                Debug.Log("Bulles restantes: " + ringCharge);
            }
        }
    }

    private void FixedUpdate()
    {
        if (movementScript.isGrounded)
        {
            CheckRingReset("Ground");
        }

        if (_timerCooldown > 0)
        {
            _timerCooldown -= Time.deltaTime;
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
