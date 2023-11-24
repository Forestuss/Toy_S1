using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FusionBehavior : MonoBehaviour
{
    public float mergeSpeed;
    public float mergeSizeRatio;
    public GameObject objectMergeResult;
    public FusionBehavior fusionScript;

    private bool isSecondTick = false;
    private bool isLerpActive = false;
    private bool isOriginalObject = false;
    private GameObject otherFusionObject;

    private void Start()
    {
        gameObject.AddComponent<Rigidbody>();
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
    }
    private void OnTriggerEnter(Collider other) //active le script de fusion pour les deux bulles et maintient le rigidbody quand deux bulles se touchent à la première frame.
    {
        if (other.gameObject.CompareTag("BoostLiquid"))
        {
            Debug.Log("TriggerEnter Detected");
            isLerpActive = true;
            isOriginalObject = true;
            otherFusionObject = other.gameObject;
            InvokeRepeating("LerpLiquid", 0f, 0.02f);
        }
    }

    private void FixedUpdate() //s'occupe de retirer le rigidbody quand la bulle est seule
    {
        if (isSecondTick == false)
        {
            Debug.Log("FirstTick");
            isSecondTick = true;

            if (isLerpActive == false)
            {
                Destroy(gameObject.GetComponent<Rigidbody>());
            }
        }
    }

    public void LerpLiquid()
    {
        if (otherFusionObject != null)
        {
            float maxPos = mergeSpeed / ((transform.position - otherFusionObject.transform.position).magnitude);
            transform.position = Vector3.MoveTowards(transform.position, otherFusionObject.transform.position, maxPos);

            if (Vector3.Distance(transform.position, otherFusionObject.transform.position) <= 0.1)
            {
                if (isOriginalObject)
                {
                    Debug.Log("Instantiate de nouvelle bulle");
                    //Instantiate(objectMergeResult);
                }
                Destroy(this.gameObject);
            }
        }
    }
}
