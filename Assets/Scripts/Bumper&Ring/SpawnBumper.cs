using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBumper : MonoBehaviour
{
    [Header("Instantiate Data")]
    [SerializeField] private GameObject _Bumper;
    [SerializeField] private GameObject _Target;

    [Header("Bumper Charges")]
    public int bumperMaxCharge; //Nombre de charges maximum de bumper à placer. Si 0: charges illimitées
    public float bumperCooldown; //Le temps d'attente avant de pouvoir placer un autre bumper

    [Header("bumper Reset")]
    public int bumperChargeOnGround; //Nombre de charges récupérées en touchant le sol
    public int bumperChargeOnBumper; //Nombre de charges récupérées en touchant un Bumper

    [Header("Debug")]
    public float bumperCharge;
    [SerializeField] private float _timerCooldown;
    [SerializeField] private bool _isUnlimited;

    private PlayerMovements movementScript;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        movementScript = GetComponent<PlayerMovements>();

        bumperCharge = bumperMaxCharge;
        _timerCooldown = bumperCooldown;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1) && (_timerCooldown <= 0 || _isUnlimited) && (bumperCharge > 0 || _isUnlimited || bumperMaxCharge == 0))
        {
            Vector3 rayDirection = (_Target.transform.position - transform.position).normalized;
            RaycastHit hit;

            if (Physics.Raycast(transform.position, rayDirection, out hit, 10))
            {
                Instantiate(_Bumper, hit.point, Camera.main.transform.rotation);
                //Debug.Log("special instantiate");
            }

            else
            {
                Instantiate(_Bumper, _Target.transform.position, Camera.main.transform.rotation);
            }

            _timerCooldown = bumperCooldown;

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

        if (_timerCooldown > 0)
        {
            _timerCooldown -= Time.deltaTime;
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
