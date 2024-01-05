using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovements : MonoBehaviour
{
    [Space]
    [Header("Player Inputs")]
    private float _verticalInput;
    private float _horizontalInput;

    [Space]
    [Header("Player Speed")]
    [SerializeField] private float _speedMultiplier = 2f;
    [SerializeField] private float _maxSpeed = 200f;
    [SerializeField] private float _clampGroundSpeed = 50f;
    private bool _isSliding;
    private float _lerpSlide = 0.02f;    
    
    private Vector3 inertieVelocity;
    private Vector3 inputVelocity;

    [Space]
    [Header("Jump")]    
    [DoNotSerialize] public bool isGrounded;
    [SerializeField] private float _jumpForce = 60f;
    private bool _jumping;
    
    [Space]
    [Header("Layers")]
    [SerializeField] private LayerMask _groundLayer;

    [Space]
    [Header("Speed Display")]
    public TextMeshProUGUI speedDisp;

    [Space]
    [Header("Others")]
    [DoNotSerialize] public bool inRing;
    private Rigidbody _rb;

    private FMOD.Studio.EventInstance SonSpeed;


    void Start()
    {
        SonSpeed = FMODUnity.RuntimeManager.CreateInstance("event:/PlayerBehave/SoundSpeed");
        SonSpeed.start();
        _rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Récuperation de données pour le mouvement
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");

        inputVelocity = transform.forward * _verticalInput + transform.right * _horizontalInput;
        inertieVelocity = _rb.velocity;

        // Récuperation des données pour le saut
        if (Physics.Raycast(_rb.transform.position, Vector3.down, 2.5f, _groundLayer))
           isGrounded = true;
        else
            isGrounded = false;

        // Récuperation des inputs pour le saut
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            _jumping = true;

        // Récuperation des inputs pour le slide
        if (Input.GetKey(KeyCode.C) && isGrounded)
            _isSliding = true;
        else
            _isSliding = false;
    }

    void FixedUpdate()
    {
        VelocityPlayer();
        JumpPlayer();
    }

    private void VelocityPlayer()
    {
        if (isGrounded && _isSliding)
        {
            _rb.velocity = Vector3.ClampMagnitude(inertieVelocity + inputVelocity.normalized * _speedMultiplier, _clampGroundSpeed);
        }

        if (isGrounded && !_isSliding)
        {
            _rb.velocity = Vector3.Lerp(inertieVelocity + inputVelocity.normalized * _speedMultiplier, Vector3.zero, _lerpSlide);
        }

        if (!isGrounded)
        {
            _rb.velocity = inertieVelocity + inputVelocity.normalized * _speedMultiplier;
        }

        if(_rb.velocity.magnitude > _maxSpeed)
        {
            _rb.velocity = Vector3.ClampMagnitude(_rb.velocity, _maxSpeed);
        }

        float actualSpeed = _rb.velocity.magnitude;
        SonSpeed.setParameterByName("Vitesse", actualSpeed);
        speedDisp.SetText("speed : {0:1}", actualSpeed);
    }

    private void JumpPlayer()
    {
        if (_jumping)
        {
            _rb.velocity = new Vector3(_rb.velocity.x, _jumpForce, _rb.velocity.z);
            _jumping = false;
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
