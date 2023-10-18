using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnLiquid : MonoBehaviour
{
    [SerializeField]
    private GameObject bubble;
    [SerializeField]
    private GameObject Target;

    [Header("Liquid")]
    public int liquidMaxCharge;
    public float liquidCooldown;
    public int liquidChargeOnGrond;
    public int liquidChargeOnTrigger;
    public bool isUnlimited;

    private float liquidCharge;
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        liquidCharge = liquidMaxCharge;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && (liquidCharge > 0 || isUnlimited))
        {
            Instantiate(bubble, Target.transform.position, Quaternion.identity);
            liquidCharge -= 1;
        }
    }

    private void FixedUpdate()
    {
        
    }
}
                                                                      