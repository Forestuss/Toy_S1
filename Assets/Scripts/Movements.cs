using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movements : MonoBehaviour
{
    private Vector3 inputVelocity;
    private Vector3 inertieVelocity;

    private Rigidbody rb;

    private float _verticalInput;
    private float _horizontalInput;

    private float pitch, yaw;

    private bool isGrounded;
    private bool jumping;

    [Header("Controller")]
    [SerializeField] private float speed = 10f;
    [SerializeField] private float airDrag = 0.75f;
    [SerializeField] private float sensitivity = 2;

    private Vector3 movementDir;

    [Header("Camera")]
    [Range(0.0f, 70.0f)] public float _cameraDistance;
    public GameObject _cameraPivot;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");

        if (Physics.Raycast(rb.transform.position, Vector3.down, 2.5f))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            jumping = true;
        }
        inertieVelocity = rb.velocity;
        

    }

    private void FixedUpdate()
    {
        Look();
        if (isGrounded)
        {
            movementDir = transform.forward * _verticalInput + transform.right * _horizontalInput;
            rb.AddForce(movementDir.normalized * speed, ForceMode.Force);
            Debug.Log("Grounded");
        }
        else
        {
            movementDir = (transform.forward * _verticalInput + transform.right * _horizontalInput) * airDrag;
            rb.AddForce(movementDir.normalized * speed, ForceMode.Force);
            Debug.Log("Not Grounded");
        }
        rb.velocity = inputVelocity + inertieVelocity;
    }
    void Look() //tuto ytb pour la souris (fonctionnel)
    {
        pitch -= Input.GetAxisRaw("Mouse Y") * sensitivity;
        pitch = Mathf.Clamp(pitch, -90.0f, 90.0f); 
        yaw += Input.GetAxisRaw("Mouse X") * sensitivity;
        transform.localRotation = Quaternion.Euler(0, yaw, 0);
        _cameraPivot.transform.localRotation = Quaternion.Euler(pitch,0 ,0);
    }
}
