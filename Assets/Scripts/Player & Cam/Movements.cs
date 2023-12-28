using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Movements : MonoBehaviour
{
    private Vector3 inputVelocity;
    [DoNotSerialize] public Vector3 inertieVelocity;

    private Rigidbody rb;

    private float verticalInput;
    private float horizontalInput;

    private bool jumping;
    
    private bool isSliding;

    [DoNotSerialize] public bool isGrounded;

    public float lerpSlide;

    [Header("Controller")]
    [SerializeField] private float speed = 1.0f;
    [SerializeField] private float maxSpeed = 200f;
    [SerializeField] private float jumpForce;
    [SerializeField] private float clamp;

    public LayerMask groundLayer;
    public LayerMask padsLayer;

    public TextMeshProUGUI speedDisp;

    public bool inRing;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Récuperation de données pour le mouvement
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        inputVelocity = transform.forward * verticalInput + transform.right * horizontalInput;
        inertieVelocity = rb.velocity;

        // Récuperation des données pour le saut
        if (Physics.Raycast(rb.transform.position, Vector3.down, 2.5f, groundLayer))
           isGrounded = true;

        else
            isGrounded = false;


        // Récuperation des inputs pour le saut
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            jumping = true;


        // Récuperation des inputs pour le slide
        if (Input.GetKey(KeyCode.C) && isGrounded)
            isSliding = true;

        else
            isSliding = false;
    }

    void FixedUpdate()
    {
        VelocityPlayer();
        JumpPlayer();
    }

    private void VelocityPlayer()
    {
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

        float actualSpeed = rb.velocity.magnitude;
        speedDisp.SetText("speed : {0:1}", actualSpeed);
    }

    private void JumpPlayer()
    {
        if (jumping)
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
            jumping = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Ring"))
            inRing = true;
    }    
    
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("Ring"))
            inRing = false;
    }
}
