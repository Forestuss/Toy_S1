using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingBehavior : MonoBehaviour
{
    public float ringBoost; //multiplicateur de v�locit� du drag. 1 redonne la m�me v�locit�. Valeur de base suppos�e 1.2
    public float ringMaxSpeed; //vitesse maximale du joueur en sortie de drag
    public float ringMaxSpeedDivideRatio; 
    public float ringDragMaxAngle; //90 maximum, valeur de base suppos�e 70
    public float ringDragVelocityRatio; //Puissance avec laquelle le ring d�tourne la direction du joueur (lerp de 0 � 1). Valeur de base suppos�e 0

    private Vector3 dragDirection;
    private float dragVelocity;

    private void OnTriggerEnter(Collider other)
    {
         if (other.CompareTag("Player"))
         {
            other.attachedRigidbody.useGravity = false;

            if (Vector3.Angle(this.transform.forward, other.attachedRigidbody.velocity.normalized) < ringDragMaxAngle) 
            {
                Debug.Log("case 1");
                dragVelocity = other.attachedRigidbody.velocity.magnitude;
                dragDirection = Vector3.Lerp(other.attachedRigidbody.velocity.normalized, this.transform.forward, ringDragVelocityRatio);
                other.attachedRigidbody.velocity = dragDirection * dragVelocity * ringBoost;

            }

            else if (Vector3.Angle(this.transform.forward, other.attachedRigidbody.velocity.normalized) > 180 - ringDragMaxAngle) 
            {
                Debug.Log("case 2");
                dragVelocity = other.attachedRigidbody.velocity.magnitude;
                dragDirection = Vector3.Lerp(other.attachedRigidbody.velocity.normalized, -this.transform.forward, ringDragVelocityRatio);
                other.attachedRigidbody.velocity = dragDirection * dragVelocity * ringBoost;
            }

            else
            {
                Debug.Log("not in good angle");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.attachedRigidbody.useGravity = true;
        }
            
    }
}
