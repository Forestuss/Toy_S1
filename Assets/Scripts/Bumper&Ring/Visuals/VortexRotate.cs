using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VortexRotate : MonoBehaviour
{
    [SerializeField] private float _rotateSpeed;
    [SerializeField] private float _childSpeedRatio;

    [SerializeField] private Transform _mediumVortex;
    [SerializeField] private Transform _smallVortex;
    private void FixedUpdate()
    {
        transform.Rotate(new Vector3(0, 0, _rotateSpeed));  
        _mediumVortex.Rotate(new Vector3(0, 0, -_rotateSpeed * _childSpeedRatio));
        _smallVortex.Rotate(new Vector3(0, 0, _rotateSpeed * _childSpeedRatio * 2));
    }
}
