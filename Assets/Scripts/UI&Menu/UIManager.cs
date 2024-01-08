using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private float _smoothTime = 0.01f;
    private Transform _cameraTransform;

    private void Start()
    {
        _cameraTransform = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();

    }
    private void Update()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, _cameraTransform.rotation, _smoothTime);
        transform.position = _player.transform.position;
    }
}
