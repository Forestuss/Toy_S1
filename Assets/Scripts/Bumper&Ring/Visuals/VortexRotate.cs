using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VortexRotate : MonoBehaviour
{
    [SerializeField] private float _rotateSpeed;
    [SerializeField] private float _childSpeedRatio;
    private void FixedUpdate()
    {
        transform.Rotate(new Vector3(0, 0, -_rotateSpeed));    
    }
}
