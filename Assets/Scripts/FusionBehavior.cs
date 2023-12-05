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
    private Transform otherObjectTransform;

    private void Start()
    {
        mergeRange = transform.localScale.x/2 * mergeMinRange;
        Debug.Log("Start");
        //Physics.SphereCast(transform.position, mergeRange, new Vector3(0,0.001f,0), out RaycastHit otherObject);
        var otherObject = Physics.OverlapSphere(transform.position, mergeRange);

        for (var i = 0; i < otherObject.Length; i++)
        {
            if (otherObject[i].transform.CompareTag("BoostLiquid"))
            {
                if (otherObject.Length == 0)
                {
                    break;
                }
                if (otherObject.Length == 1)
                {
                    otherObjectTransform = otherObject[i].transform;
                    InvokeRepeating("LerpLiquid", 0, 0.02f);
                }
                Debug.Log("SphereFound");
                //otherObjectTransform = otherObject;
                InvokeRepeating("LerpLiquid", 0, 0.02f);
            }
        }

        Debug.Log("Object: " + otherObject);
    }

    public void LerpLiquid()
    {
        Debug.Log("LerpActivated");
        if (otherObjectTransform != null)
        {
            Debug.Log("LerpSimulated");
            float maxPos = mergeSpeed / ((transform.position - otherObjectTransform.position).magnitude);
            transform.position = Vector3.MoveTowards(transform.position, otherObjectTransform.position, maxPos);
            Debug.Log("LerpValue" + maxPos);

            if (Vector3.Distance(transform.position, otherObjectTransform.position) <= 0.1)
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
