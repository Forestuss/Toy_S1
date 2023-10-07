using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AttractionEffect : MonoBehaviour
{
    [SerializeField]
    public float _Speed = 200;
    public bool IsGravityOn = true;

    private Vector3 BH;
    private Vector3 BHR;
    private float _Distance;
    private float _ChargeForce;
    private GameObject BlackHole;

    private void Start()
    {
        BlackHole = GameObject.Find("BlackHole");
    }

    private void GravitySwitch()
    {
        
    }
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.O))
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            GetComponent<Rigidbody>().AddForce(BHR.normalized * _Speed * _ChargeForce / _Distance, ForceMode.Impulse); 
            _ChargeForce = 0;
        }
    }
    void FixedUpdate()
    {
        BH = BlackHole.transform.position - transform.position;
        BHR = transform.position - BlackHole.transform.position;

        _Distance = Mathf.Clamp(Vector3.Distance(transform.position, BH)/50, 1, 3);

        if (Input.GetKey(KeyCode.O))
        {
            GetComponent<Rigidbody>().AddForce(BH.normalized * _Speed / _Distance, ForceMode.Force);
            _ChargeForce = Mathf.Clamp(_ChargeForce + 1, 1, 5)/2;
        }

        if (Input.GetKey(KeyCode.Space)) 
        {
            GravitySwitch();
            IsGravityOn = false;
        }
    }
}
