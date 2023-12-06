using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FusionBehavior : MonoBehaviour
{
    public float mergeSpeed;
    public float mergeSizeRatio;
    public float mergeMinRange; //1 = la taille de la bulle, modulable
    public GameObject objectMergeResult;
    public FusionBehavior fusionScript;

    private bool isSecondTick = false;
    private bool isLerpActive = false;
    private bool isOriginalObject = false;
    private float mergeRange;
    private int nearestObjectIndex = 0;
    private Transform nearestObjectTransform;

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
                                     
                    Debug.Log("Multiples bulles trouvées :" + otherObjects.Length);

                    if (Vector3.Distance(otherObjects[i].transform.position, transform.position) < Vector3.Distance(otherObjects[nearestObjectIndex].transform.position, transform.position))
                    {
                        nearestObjectIndex = i;
                    }
                    nearestObjectTransform = otherObjects[i].transform;
                }

                else
                {
                    nearestObjectTransform = otherObjects[0].transform;
                    return;
                }
                Debug.Log("SphereFound");
                nearestObjectTransform = otherObjects[1].transform;
                InvokeRepeating("LerpLiquid", 0, 0.02f);
            }
        }                                              

        Debug.Log("Object: " + otherObjects);
    }

    public void LerpLiquid()
    {
        Debug.Log("LerpActivated");
        if (nearestObjectTransform != null)
        {
            Debug.Log("LerpSimulated");
            float maxPos = mergeSpeed / ((transform.position - nearestObjectTransform.position).magnitude);
            transform.position = Vector3.MoveTowards(transform.position, nearestObjectTransform.position, maxPos);
            Debug.Log("LerpValue" + maxPos);

            if (Vector3.Distance(transform.position, nearestObjectTransform.position) <= 0.1)
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
