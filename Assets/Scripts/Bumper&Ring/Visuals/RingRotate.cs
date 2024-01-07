using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingRotate : MonoBehaviour
{
    [SerializeField] private float _rotateSpeed;

    private float _rotateTimer;
    private bool _isRotating;
    private float _playerSpeed;

    private void Update()
    {
        if (_isRotating)
        {
            _rotateTimer = _rotateTimer - Time.deltaTime;
            float _rotateAngle = _rotateSpeed * _playerSpeed * (_rotateTimer/_playerSpeed);
            transform.Rotate(new Vector3(_rotateAngle, 0, 0), Space.Self);

            if (_rotateTimer <= 0)
            {
                _isRotating = false;
            }
        }
    }

    public void SpeedRotate()
    {
        _playerSpeed = Mathf.Clamp(GameObject.FindGameObjectWithTag("Player").gameObject.GetComponent<Rigidbody>().velocity.magnitude/100, 1.5f, 3);
        _rotateTimer = _playerSpeed;
        //Debug.Log("RotateSpeed: " +  _playerSpeed);
        _isRotating = true;
    }
}
