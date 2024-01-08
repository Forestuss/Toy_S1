using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private Transform _cameraTransform;

    [SerializeField] private float _smoothTime = 0.01f;

    private void Start()
    {
        _cameraTransform = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
    }
    private void Update()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, _cameraTransform.rotation, _smoothTime);
    }
}
