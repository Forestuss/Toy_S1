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
    [SerializeField] private int _bumperMaxCharge; //Nombre de charges maximum de bumper à placer. Si 0: charges illimitées
    [SerializeField] private float _bumperCooldown; //Le temps d'attente avant de pouvoir placer un autre bumper

    [Header("bumper Reset")]
    [SerializeField] private int _bumperChargeOnGround; //Nombre de charges récupérées en touchant le sol
    [SerializeField] private int _bumperChargeOnBumper; //Nombre de charges récupérées en touchant un Bumper

    [Header("Debug")]
    public float bumperCharge;
    [SerializeField] private float _timerCooldown; //Cooldown effectif du bumper 
    [SerializeField] private bool _isUnlimited; //Debug qui retire les limitations

    private PlayerMovements movementScript;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        movementScript = GetComponent<PlayerMovements>();

        bumperCharge = _bumperMaxCharge;
        _timerCooldown = _bumperCooldown;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1) && (_timerCooldown <= 0 || _isUnlimited) && (bumperCharge > 0 || _isUnlimited || _bumperMaxCharge == 0))
        {
            Vector3 rayDirection = (_Target.transform.position - transform.position).normalized;
            RaycastHit hit;

            if (Physics.Raycast(transform.position, rayDirection, out hit, 10))
            {
                Vector3 hitnormal = hit.normal;
                Vector3 hitpos = hit.point - hitnormal*7;
                Instantiate(_Bumper, hitpos, Quaternion.Euler(hitnormal));
            }

            else
            {
                Instantiate(_Bumper, _Target.transform.position, Camera.main.transform.rotation);
            }

            _timerCooldown = _bumperCooldown;

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
            bumperCharge = Mathf.Clamp(bumperCharge + _bumperChargeOnBumper, 0, _bumperMaxCharge);
        }

        if (Type == "Ground")
        {
            bumperCharge = Mathf.Clamp(bumperCharge + _bumperChargeOnGround, 0, _bumperMaxCharge);
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
