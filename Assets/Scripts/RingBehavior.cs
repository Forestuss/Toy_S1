using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingBehavior : MonoBehaviour
{
    public float ringBoost; //multiplicateur de vélocité du drag. 1 redonne la même vélocité. Valeur de base supposée 1.2
    public float ringMaxSpeed; //vitesse maximale du joueur en sortie de drag
    public float ringMaxSpeedDivideRatio; 
    public float ringDragMaxAngle; //90 maximum, valeur de base supposée 70
    public float ringThrowRadiusActivation;
    public float ringThrowVelocityRatio; //Puissance avec laquelle le ring détourne la direction du joueur (lerp de 0 à 1). Valeur de base supposée 0
    public float ringDragVelocityRatio; //Puissance avec laquelle le drag du ring détourne la vélocité du joueur vers son centre. 

    [NonSerialized] public bool isRingThrowTriggerStay;
    [NonSerialized] public ThrowTriggerBehavior ThrowTrigger;

    private Vector3 dragDirection; 
    private float dragVelocity;
    private int ringDirection;
    private bool isRingBoostLocked = false;

    private void Start()
    {
        ThrowTrigger = GetComponentInChildren<ThrowTriggerBehavior>();
    }

    private void OnTriggerEnter(Collider other)
    {
         if (other.CompareTag("Player"))
         {
            other.attachedRigidbody.useGravity = false;

            if (Vector3.Angle(this.transform.forward, other.attachedRigidbody.velocity.normalized) < ringDragMaxAngle) 
            {
                ringDirection = 0;
                //dragVelocity = other.attachedRigidbody.velocity.magnitude;
                //dragDirection = Vector3.Lerp(other.attachedRigidbody.velocity.normalized, this.transform.forward, ringThrowVelocityRatio);
                //other.attachedRigidbody.velocity = dragDirection * dragVelocity * ringBoost;

            }

            else if (Vector3.Angle(this.transform.forward, other.attachedRigidbody.velocity.normalized) > 180 - ringDragMaxAngle) 
            {
                ringDirection = 1;
                //dragVelocity = other.attachedRigidbody.velocity.magnitude;
                //dragDirection = Vector3.Lerp(other.attachedRigidbody.velocity.normalized, -this.transform.forward, ringThrowVelocityRatio);
                //other.attachedRigidbody.velocity = dragDirection * dragVelocity * ringBoost;
            }

            else
            {
                isRingBoostLocked = true;
            }

            Vector3 ringDragCenterDirection = (transform.position - other.attachedRigidbody.position).normalized;

            dragVelocity = other.attachedRigidbody.velocity.magnitude;
            dragDirection = Vector3.Lerp(other.attachedRigidbody.velocity.normalized, ringDragCenterDirection, ringDragVelocityRatio);
            other.attachedRigidbody.velocity = dragDirection * dragVelocity * ringBoost;

        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (isRingBoostLocked == false)
        {
            if (ThrowTrigger.isTriggerStay)
            {
                isRingBoostLocked = true;
                dragVelocity = other.attachedRigidbody.velocity.magnitude;

                if (ringDirection == 0)
                {
                    dragDirection = Vector3.Lerp(other.attachedRigidbody.velocity.normalized, this.transform.forward, ringThrowVelocityRatio);
                }

                else
                {
                    dragDirection = Vector3.Lerp(other.attachedRigidbody.velocity.normalized, -this.transform.forward, ringThrowVelocityRatio);
                }

                other.attachedRigidbody.velocity = dragDirection * dragVelocity;
            }


            //if (other.CompareTag("Player") && Vector3.Distance(transform.position, other.transform.position) <= ringThrowRadiusActivation * transform.localScale.x / 100)
            {
               
            }
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.attachedRigidbody.useGravity = true;
            isRingBoostLocked = false;
        }
            
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, ringThrowRadiusActivation * transform.localScale.x / 100);
    }

}
