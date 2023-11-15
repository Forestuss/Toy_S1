using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BubbleBehavior : MonoBehaviour
{
    public float mergeBubbleSpeed;
    public float mergeBubbleSizeRatio;

    private bool isFirstTick = false;

    private void Start()
    {
        gameObject.AddComponent<Rigidbody>();
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
    }
    private void OnTriggerStay(Collider other)
    {
        Debug.Log("working");

        if (other.gameObject.CompareTag("BoostLiquid"))
        {
            float maxPos = mergeBubbleSpeed/((transform.position - other.transform.position).magnitude);
            transform.position = Vector3.MoveTowards(transform.position, other.transform.position, maxPos);    
        }
    }

    private void Update()
    {
        if (isFirstTick == false)
        {
            Debug.Log("FirstTick");
            Destroy(gameObject.GetComponent<Rigidbody>());
        }
    }
}
