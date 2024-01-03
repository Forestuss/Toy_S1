using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingDragAngleTrigger : MonoBehaviour
{
    private RingBehavior _ringScript;
    private void Start()
    {
        _ringScript = transform.parent.GetComponent<RingBehavior>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _ringScript._isRingDragAngleZoneActive = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _ringScript._isRingDragAngleZoneActive = false;
        }
    }
}
