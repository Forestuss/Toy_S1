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
    private bool reload = true;
    private FMOD.Studio.EventInstance SonReload;
    private FMOD.Studio.EventInstance SonPlaced;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        movementScript = GetComponent<PlayerMovements>();

        SonPlaced = FMODUnity.RuntimeManager.CreateInstance("event:/RingBehave/CreateRing");
        SonReload = FMODUnity.RuntimeManager.CreateInstance("event:/PlayerBehave/RechargeRing");

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
                SonPlaced.start();
                reload = false;
            }

            else
            {
                Instantiate(_Ring, _Target.transform.position, Camera.main.transform.rotation);
                SonPlaced.start();
                reload = false;
            }

            _timerCooldown = _ringCooldown;

            if (ringCharge > 0)
            {
                ringCharge -= 1;
                SonPlaced.setParameterByName("Charge", ringCharge);
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
            if (!reload)
            {
                SonPlaced.setParameterByName("Charge", ringCharge);
                SonReload.start();
                reload = true;
            }
        }

        if (Type == "Ground")
        {
            ringCharge = Mathf.Clamp(ringCharge + _ringChargeOnGround, 0, _ringMaxCharge);
            SonPlaced.setParameterByName("Charge", ringCharge);
            if (!reload)
            {
                SonReload.start();
                SonPlaced.setParameterByName("Charge", ringCharge);
                reload = true;
            }
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
