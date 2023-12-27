using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NewCameraController : MonoBehaviour
{
    private float horizontal;
    private float horizontalRotation;
    private float vertical;
    private float verticalRotation;

    public float sensitivity = 2f;
    public float smoothTime = 0.01f;

    [SerializeField] private CinemachineVirtualCamera cineMachineCamera;

    [SerializeField] private GameObject cameraPivot;
    [SerializeField] private GameObject cameraTarget;
    [SerializeField] private GameObject player;

    private bool playerGrounded;
    private bool playerBoosted;


    // Update is called once per frame
    void Update()
    {
        playerGrounded = player.GetComponent<Movements>().isGrounded;
        playerBoosted = player.GetComponent<Movements>().inRing;

        horizontal = Input.GetAxisRaw("Mouse X") * sensitivity;
        horizontalRotation += horizontal;
       
        vertical = Input.GetAxisRaw("Mouse Y") * sensitivity;
        verticalRotation -= vertical;
        verticalRotation = Mathf.Clamp(verticalRotation, -90.0f, 90.0f);

        cameraPivot.transform.position = player.transform.position;
        cameraPivot.transform.localRotation = Quaternion.Euler(verticalRotation, horizontalRotation, 0);

        FOVMangaer();
    }

    private void FixedUpdate()
    {
        transform.position = cameraTarget.transform.position;
        player.transform.localRotation = Quaternion.Euler(0, horizontalRotation, 0);
    }

    void FOVMangaer()
    {
        if(playerBoosted)
        {
            cineMachineCamera.m_Lens.FieldOfView = Mathf.Lerp(cineMachineCamera.m_Lens.FieldOfView, 80, smoothTime);
        }

        else
        {
            cineMachineCamera.m_Lens.FieldOfView = Mathf.Lerp(cineMachineCamera.m_Lens.FieldOfView, 70, smoothTime);
        }
    }
  
}
