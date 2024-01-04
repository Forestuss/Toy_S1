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
    [SerializeField] private int _ringMaxCharge; //Nombre de charges maximum de ring à placer. Si 0: charges illimitées
    [SerializeField] private float _ringCooldown; //Le temps d'attente avant de pouvoir placer un autre ring

    [Header("Ring Reset")]
    [SerializeField] private int _ringChargeOnGround; //Nombre de charges récupérées en touchant le sol
    [SerializeField] private int _ringChargeOnBumper; //Nombre de charges récupérées en touchant un Bumper

    [Header("Debug")]
    public float ringCharge;
    [SerializeField] private float _timerCooldown; //Cooldown entre 2 possible poses de ring
    [SerializeField] private bool _isUnlimited; //Debug qui retire les limitations

    private PlayerMovements movementScript;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        movementScript = GetComponent<PlayerMovements>();

        ringCharge = _ringMaxCharge;
        _timerCooldown = _ringCooldown;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && (_timerCooldown <= 0 || _isUnlimited) && (ringCharge > 0 || _isUnlimited || _ringMaxCharge == 0))
        {
            Vector3 rayDirection = (_Target.transform.position - transform.position).normalized;
            RaycastHit hit;

            if (Physics.Raycast(transform.position, rayDirection, out hit, 10))
            {
                Instantiate(_Ring, hit.point, Camera.main.transform.rotation);
            }

            else
            {
                Instantiate(_Ring, _Target.transform.position, Camera.main.transform.rotation);
            }

            _timerCooldown = _ringCooldown;

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
            ringCharge = Mathf.Clamp(ringCharge + _ringChargeOnBumper, 0, _ringMaxCharge);
        }

        if (Type == "Ground")
        {
            ringCharge = Mathf.Clamp(ringCharge + _ringChargeOnGround, 0, _ringMaxCharge);
        }

        if (Type == "CloudZone")
        {
            ringCharge = Mathf.Clamp(ringCharge + 1, 0, _ringMaxCharge);
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
