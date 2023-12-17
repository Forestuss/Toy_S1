using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FusionBehavior : MonoBehaviour
{
    [Header("Merge")]
    public float mergeSpeed;
    public float mergeSizeRatio;
    public float mergeMinRange; //1 = la taille de la bulle, modulable

    [Header("Power Merge")]
    public float powerMergeMinAngle;
    public float powerMergeMinDistance;
    public float powerMergeMultiplier;

    [Header("Merge Results")]
    public GameObject objectMergeResult;
    public FusionBehavior fusionScript;

    private bool isSecondTick = false;
    private bool isLerpActive = false;
    private bool isOriginalObject = false;
    private float mergeRange;
    private int nearestObjectIndex = 0;
    private GameObject nearestObject;

    private void Start()
    {
        mergeRange = transform.localScale.x/2 * mergeMinRange;
        Debug.Log("Start");

        var otherObjects = Physics.OverlapSphere(transform.position, mergeRange);

        if (otherObjects.Length == 0)
        {
            Debug.Log("Pas de collider trouvés");
            return;
        }

        for (var i = 0; i < otherObjects.Length; i++)
        {

            Debug.Log("Index de Bulle actuel :" + i);

            if (otherObjects[i].transform.CompareTag("BoostLiquid"))
            {
                Debug.Log("Tag BoostLiquid trouvé");

                if (otherObjects.Length > 1)
                {
                                     
                    Debug.Log("Multiples ring trouvés :" + otherObjects.Length);

                    if (Vector3.Distance(otherObjects[i].transform.position, transform.position) < Vector3.Distance(otherObjects[nearestObjectIndex].transform.position, transform.position))
                    {
                        nearestObjectIndex = i;
                    }
                    nearestObject = otherObjects[i].gameObject;
                }

                else
                {
                    nearestObject = otherObjects[0].gameObject;
                    break;
                }
            }
        }
        
        if (nearestObject.CompareTag("BoostLiquid") && Vector3.Distance(transform.position, nearestObject.transform.position) < powerMergeMinDistance && ((Vector3.Angle(transform.forward, nearestObject.transform.forward) < powerMergeMinAngle) || Vector3.Angle(transform.forward, nearestObject.transform.forward) > 180 - powerMergeMinAngle))
        {
            Debug.Log("Power Merge Situation Detected");
            PowerMergeRing();
        }

        else if (nearestObject.CompareTag("BoostLiquid"))
        {
            Debug.Log("Normal Merge Situation Detected");
            InvokeRepeating("MergeRing", 0, 0.02f);
        }
    }


    public void PowerMergeRing()
    {
        Debug.Log("Power Merge Activated");

        if (nearestObject != null)
        {
            Instantiate(objectMergeResult, nearestObject.transform.position, nearestObject.transform.rotation);
            Destroy(nearestObject);
            Destroy(this.gameObject);
            Debug.Log("Power Merge Executed");
        }
    }

    public void MergeRing()
    {
        Debug.Log("Merge Activated");
        if (nearestObject != null)
        {
            Debug.Log("Merge Ongoing");
            float maxPos = mergeSpeed / ((transform.position - nearestObject.transform.position).magnitude);
            transform.position = Vector3.MoveTowards(transform.position, nearestObject.transform.position, maxPos);
            Debug.Log("LerpValue" + maxPos);

            if (Vector3.Distance(transform.position, nearestObject.transform.position) <= 0.1)
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, transform.localScale.x / 2 * mergeMinRange);
    }
}
