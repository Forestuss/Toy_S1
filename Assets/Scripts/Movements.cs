using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class Movements : MonoBehaviour
{
    public Material slideMaterial;
    public Material originalMaterial;

    private Vector3 inputVelocity;
    private Vector3 inertieVelocity;

    private Rigidbody rb;

    private float verticalInput;
    private float horizontalInput;

    [SerializeField] private bool jumping;
    [SerializeField] public bool isGrounded;
    [SerializeField] private bool isSliding;

    public float lerpSlide;

    [Header("Controller")]
    [SerializeField] private float speed = 1.0f;
    [SerializeField] private float maxSpeed = 200f;
    [SerializeField] private float jumpForce;
    [SerializeField] private float clamp;

    [Header("Jump Tolerance")]
    public float coyoteTime = 0.2f;
    public float coyoteTimeCounter = 0f;

    public LayerMask groundLayer;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        coyoteTimeCounter = coyoteTime;
    }

    void Update()
    {
        // Récuperation de données pour le mouvement
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // Récuperation des données pour le saut
        if (Physics.Raycast(rb.transform.position, Vector3.down, 2.5f, groundLayer))
        {
            //Debug.Log("grounded2222");
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

        if (Input.GetKey(KeyCode.C) && isGrounded)
        {
            Debug.Log("slides");
            isSliding = true;
            GetComponent<MeshRenderer>().material = slideMaterial;
        }
        else
        {
            isSliding = false;
            GetComponent<MeshRenderer>().material = originalMaterial;
        }
    }

    void FixedUpdate()
    {
        //MovePlayer();
        VelocityPlayer();
        // coyoteTimeChecking();
        JumpPlayer();
        //Debug.Log(rb.velocity.magnitude);
    }

    private void VelocityPlayer()
    {
        inputVelocity = transform.forward * verticalInput + transform.right * horizontalInput;
        inertieVelocity = rb.velocity;

        if (isGrounded && isSliding)
        {
            rb.velocity = Vector3.ClampMagnitude(inertieVelocity + inputVelocity.normalized * speed, clamp);
        }

        if (isGrounded && !isSliding)
        {
            rb.velocity = Vector3.Lerp(inertieVelocity + inputVelocity.normalized * speed, Vector3.zero, lerpSlide);
        }

        if (!isGrounded)
        {
            rb.velocity = inertieVelocity + inputVelocity.normalized * speed;
        }
        if(rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
        }
    }

    private void JumpPlayer()
    {
        if (jumping)
        {
            rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
            jumping = false;
        }
    }

    private void coyoteTimeChecking()
    {
        if (isGrounded)
            coyoteTimeCounter = coyoteTime;
        else
            coyoteTimeCounter -= Time.deltaTime;


        if (isGrounded && Input.GetButtonDown("Jump"))
            jumping = true;

        else if (!isGrounded && coyoteTimeCounter > 0 && Input.GetButtonDown("Jump"))
            jumping = true;
    }
}
