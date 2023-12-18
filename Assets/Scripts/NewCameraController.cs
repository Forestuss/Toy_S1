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

    [SerializeField] private GameObject cameraPivot;
    [SerializeField] private GameObject cameraTarget;
    [SerializeField] private GameObject player;

    private Vector2 turn;

    private bool playerGrounded;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        playerGrounded = player.GetComponent<Movements>().isGrounded;

        horizontal = Input.GetAxisRaw("Mouse X") * sensitivity;
        horizontalRotation += horizontal;
       
        vertical = Input.GetAxisRaw("Mouse Y") * sensitivity;
        verticalRotation -= vertical;
        verticalRotation = Mathf.Clamp(verticalRotation, -90.0f, 90.0f);

        cameraPivot.transform.position = player.transform.position;
        cameraPivot.transform.localRotation = Quaternion.Euler(verticalRotation, horizontalRotation, 0);
    }

    private void FixedUpdate()
    {
        transform.position = cameraTarget.transform.position;
        player.transform.localRotation = Quaternion.Euler(0, horizontalRotation, 0);
    }
}
