using System;
using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    [Space]
    [Header("Sensibilité")]
    public float camSensitivity = 2f;

    [Space]
    [Header("Camera Settings")]
    public float positionSmoothTime = 0.01f;
    public float cameraHeight = 2f;
    public float cameraHeightSmoothTime = 0.01f;

    [Space]
    [Header("Usefull for Camera")] 
    [SerializeField] private CinemachineVirtualCamera _cineMachineCamera;
    [SerializeField] private GameObject _cameraPivot;
    [SerializeField] private GameObject _cameraTarget;
    [SerializeField] private GameObject _player;
    private Vector3 _playerPos;
    private bool _playerGrounded;
    private bool _playerBoosted;

    [Space]
    [Header("Inputs")]
    private float _horizontal;
    private float _horizontalRotation;
    private float _vertical;
    private float _verticalRotation;


    // Update is called once per frame
    void Update()
    {
        _playerPos = _player.transform.position;

        _playerGrounded = _player.GetComponent<Movements>().isGrounded;
        _playerBoosted = _player.GetComponent<Movements>().inRing;

        _horizontal = Input.GetAxisRaw("Mouse X") * camSensitivity;
        _horizontalRotation += _horizontal;
       
        _vertical = Input.GetAxisRaw("Mouse Y") * camSensitivity;
        _verticalRotation -= _vertical;
        _verticalRotation = Mathf.Clamp(_verticalRotation, -90.0f, 90.0f);

        if (_playerGrounded)
        {
            cameraHeight = Mathf.Lerp(cameraHeight, 2, cameraHeightSmoothTime);
            _cameraPivot.transform.position = _player.transform.position + new Vector3(0, cameraHeight, 0);
        }

        else
        {
            cameraHeight = Mathf.Lerp(cameraHeight, 0, cameraHeightSmoothTime);
            _cameraPivot.transform.position = _player.transform.position + new Vector3(0, cameraHeight, 0);
        }

        _cameraPivot.transform.localRotation = Quaternion.Euler(_verticalRotation, _horizontalRotation, 0);


        CameraHeight();

        FOVMangaer();
    }

    private void FixedUpdate()
    {
        transform.position = _cameraTarget.transform.position;
        _player.transform.localRotation = Quaternion.Euler(0, _horizontalRotation, 0);
    }

    void FOVMangaer()
    {
        if(_playerBoosted)
        {
            _cineMachineCamera.m_Lens.FieldOfView = Mathf.Lerp(_cineMachineCamera.m_Lens.FieldOfView, 80, positionSmoothTime);
        }

        else
        {
            _cineMachineCamera.m_Lens.FieldOfView = Mathf.Lerp(_cineMachineCamera.m_Lens.FieldOfView, 70, positionSmoothTime);
        }
    }
    
    void CameraHeight()
    {
        if (_playerGrounded)
        {
            cameraHeight = Mathf.Lerp(cameraHeight, 2, cameraHeightSmoothTime);
            _cameraPivot.transform.position = _player.transform.position + new Vector3(0, cameraHeight, 0);
        }

        else
        {
            cameraHeight = Mathf.Lerp(cameraHeight, 0, cameraHeightSmoothTime);
            _cameraPivot.transform.position = _player.transform.position + new Vector3(0, cameraHeight, 0);
        }
    }

    void PlayerDistanceDisplay()
    {
        if (_playerPos.x - Camera.main.transform.position.x < 2f || _playerPos.y - Camera.main.transform.position.y < 2f || _playerPos.z - Camera.main.transform.position.z < 2f)
        {

        }
    }
}
