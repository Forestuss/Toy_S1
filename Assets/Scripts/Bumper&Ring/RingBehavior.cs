using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class RingBehavior : MonoBehaviour
{
    [SerializeField] private float _ringBoost; //multiplicateur de v�locit� du drag. 1 redonne la m�me v�locit�. Valeur de base suppos�e 1.2
    [SerializeField] private float _ringMinSpeed; //v�locit� minimale du joueur e nsortie de ring.
    [SerializeField] private float _ringDragMaxAngle; //Angle maximal d'entr�e dans un ring, au del�, le joueur passe au travers de la structure car il n'est pas allign� avec.
    [SerializeField] private float _ringDragDotDirectionMax; //Valeur de 0 � 1, indiquant la permissivit� de l'activation du ring par rapport � la dotdirection du joueur et du ring, pour �viter que le joueur puisse r�aliser des 180�
    [SerializeField] private float _ringThrowRadiusActivation; //Taille du trigger du Throw au centre du ring
    [SerializeField] private float _ringThrowVelocityRatio; //Puissance avec laquelle le throw du ring d�tourne la direction du joueur (lerp de 0 � 1). Valeur de base suppos�e 0
    [SerializeField] private float _ringDragVelocityRatio; //Puissance avec laquelle le drag du ring d�tourne la v�locit� du joueur vers son centre. 

    private Vector3 _dragDirection; //Direction du drag
    private float _dragVelocity; //Puissance du drag
    private int _ringDirection; //0 ou 1 indiquant la direction du drag et du throw selon la position d'entr�e du joueur
    private bool _isRingBoostLocked = false; //Emp�che une activation en boucle du drag ou du throw, ou une activation dans des conditions que la v�locit� + position du joueur ne remplie pas

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Vector3 _dotDirection = (other.transform.position - transform.position).normalized;

            if (Vector3.Angle(this.transform.forward, other.attachedRigidbody.velocity.normalized) < _ringDragMaxAngle && Vector3.Dot(_dotDirection, other.attachedRigidbody.velocity.normalized) < _ringDragDotDirectionMax)
            {
                _ringDirection = 0;
                gameObject.GetComponentInChildren<RingRotate>().SpeedRotate();
                other.gameObject.GetComponent<PlayerMovements>().MaxSpeedAugment();
                Debug.Log("RIGHT anglezone/outzone and dotDirection n�1: " + Vector3.Dot(_dotDirection, other.attachedRigidbody.velocity.normalized));
            }

            else if (Vector3.Angle(this.transform.forward, other.attachedRigidbody.velocity.normalized) > 180 - _ringDragMaxAngle && Vector3.Dot(_dotDirection, other.attachedRigidbody.velocity.normalized) < _ringDragDotDirectionMax)
            {
                _ringDirection = 1;
                gameObject.GetComponentInChildren<RingRotate>().SpeedRotate();
                other.gameObject.GetComponent<PlayerMovements>().MaxSpeedAugment();
                Debug.Log("RIGHT anglezone/outzone and dotDirection n�2: " + Vector3.Dot(_dotDirection, other.attachedRigidbody.velocity.normalized));
            }

            else
            {
                _isRingBoostLocked = true;
                Debug.Log("WRONG angle in zone or dotDirection: " + Vector3.Dot(_dotDirection, other.attachedRigidbody.velocity.normalized));
            }

            Vector3 _ringDragCenterDirection = (transform.position - other.attachedRigidbody.position).normalized;

            if (!_isRingBoostLocked)
            {
                other.attachedRigidbody.useGravity = false;
                _dragVelocity = Mathf.Max(other.attachedRigidbody.velocity.magnitude, _ringMinSpeed);
                _dragDirection = Vector3.Lerp(other.attachedRigidbody.velocity.normalized, _ringDragCenterDirection, _ringDragVelocityRatio);
                other.attachedRigidbody.velocity = _dragDirection * _dragVelocity * _ringBoost;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (_isRingBoostLocked == false)
        {
            if (other.CompareTag("Player") && Vector3.Distance(transform.position, other.transform.position) <= _ringThrowRadiusActivation * transform.localScale.x / 100)
            {

                _isRingBoostLocked = true;
                _dragVelocity = Mathf.Max(other.attachedRigidbody.velocity.magnitude, _ringMinSpeed);

                if (_ringDirection == 0)
                {
                    _dragDirection = Vector3.Lerp(other.attachedRigidbody.velocity.normalized, this.transform.forward, _ringThrowVelocityRatio);
                }

                else
                {
                    _dragDirection = Vector3.Lerp(other.attachedRigidbody.velocity.normalized, -this.transform.forward, _ringThrowVelocityRatio);
                }

                other.attachedRigidbody.velocity = _dragDirection * _dragVelocity;
            }
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.attachedRigidbody.useGravity = true;
            _isRingBoostLocked = false;
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, _ringThrowRadiusActivation * transform.localScale.x / 100);
    }
}
