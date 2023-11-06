using System;
using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.Animations;

public class PlayerMovement : MonoBehaviour
{
    public Material slideMaterial;
    public Material originalMaterial;

    private Vector3 inputVelocity;
    private Vector3 inertieVelocity;

    private Rigidbody rb;
    private Vector3 bumperVelocity;
        
    private bool isBoosted;
    private bool isBumping;
    private Vector3 originalVelocity;

    private bool debugIsFrameCollide = false; //DEBUG 
    private Vector3 velovityBug; //DEBUG

    private float verticalInput;
    private float horizontalInput;

    private Vector3 movementDir;

    private bool jumping;
    private bool isGrounded;
    private bool isSliding;

    public float lerpSlide;

    private float yaw = 0.0f, pitch = 0.0f;

    [Header("Controller")]
    [SerializeField] private float speed = 1.0f;
    [SerializeField] private float maxSpeed = 200f;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _sensitivity;
    [SerializeField] private float clamp;

    private float _originalSensitivity;

    [Header("Liquid")]
    public float _liquidSpeed; //multiplicateur de vitesse dans la bulle. 1 signifie que la vitesse est la même dans la bulle qu'a l'exterieur. 
    public float _liquidLook; //Sensivité de la caméra dans la bulle (ne pas mettre une sensibilité au dessus de la sensibilité de base) 

    [Header("Bump")]
    public float _bumpForce; //puissance factorielle du bumper. 1 signifie que le bumper renvoie la même force que l'on lui donne. Valeur de base 0.9 (légère perte de puissance de vélocité à chaque saut de bumper)
    public float _bumpInfluence; //influence de la direction du bumper sur la trajectoire du joueur. Uniquement mettre une valeur float entre 0 et 1. 0 signifie que le player rebondit comme sur un miroir, et 1 qu'il suit complètement la direction du bumper.
    public float _bumpMIN; //Le minimum de base est de 50. Empêche le joueur de faire de touts petits sauts par erreurs.
    public float _bumpMAX; //le maximum de base est 100. Empêche le joueur de stacker trop de vitesse après au saut. 

    [Header("Camera")]
    [Range(0.0f, 70.0f)] public float _cameraDistance;
    public GameObject _cameraPivot;


    public LayerMask groundLayer;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        _originalSensitivity = _sensitivity;
    }

    void Update()
    {
        // Récuperation de données pour le mouvement
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        
        // Mouvement de la caméra
        Look();

        // Récuperation des données pour le saut
        if (Physics.Raycast(rb.transform.position, Vector3.down, 2.5f, groundLayer))
        {
            Debug.Log("grounded2222");
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

        Camera.main.transform.localPosition = new Vector3(0, 0, - _cameraDistance);

        bumperVelocity = rb.velocity;
    }

    void FixedUpdate()
    {
        //MovePlayer();
        VelocityPlayer();
        JumpPlayer();
        Debug.Log(rb.velocity.magnitude);
    }

    private void VelocityPlayer()
    {
        inputVelocity = transform.forward * verticalInput + transform.right * horizontalInput;
        inertieVelocity = rb.velocity;

        if (isGrounded && !isSliding)
        {
            rb.velocity =  Vector3.ClampMagnitude(inertieVelocity + inputVelocity.normalized * speed, clamp);
        }

        if (isGrounded && isSliding)
        {
            rb.velocity = Vector3.Lerp(inertieVelocity + inputVelocity.normalized * speed, Vector3.zero, lerpSlide);
        }

        if (!isGrounded)
        {
            rb.velocity = inertieVelocity + inputVelocity.normalized * speed;
        }
    }

    private void JumpPlayer()
    {
        if(jumping)
        {
            rb.velocity = new Vector3(rb.velocity.x, _jumpForce, rb.velocity.z);
            jumping = false;
        }
    }

    void Look() //tuto ytb pour la souris (fonctionnel)
    {
        pitch -= Input.GetAxisRaw("Mouse Y") * _sensitivity;
        pitch = Mathf.Clamp(pitch, -90.0f, 90.0f);
        yaw += Input.GetAxisRaw("Mouse X") * _sensitivity;
        transform.localRotation = Quaternion.Euler(0, yaw, 0);
        _cameraPivot.transform.localRotation = Quaternion.Euler(pitch, 0, 0);
    }  

    void OnTriggerEnter(Collider Liquid)
    {
        if (Liquid.gameObject.tag == "BoostLiquid")
        {
            rb.useGravity = false; //enlève la gravité dans le liquide de boost 
            isBoosted = true; // annule la possibilité de mouvement 
            originalVelocity = rb.velocity;
        }
    }

    void OnTriggerStay(Collider Liquid)
    {
        if (Liquid.gameObject.tag == "BoostLiquid")
        {
            //_sensitivity = Mathf.Min(_sensitivity / 1.05f, _liquidLook); //réduit la sensi de la souris dans le liquide de boost (pas néscessaire) 
            rb.AddForce(rb.velocity * _liquidSpeed, ForceMode.Force); //boost de la velocité 
        }
    }

    void OnTriggerExit(Collider Liquid)
    {
        if (Liquid.gameObject.tag == "BoostLiquid")
        {
            isBoosted = false;  
            _sensitivity = _originalSensitivity; 
            rb.useGravity = true; 
        }
    }

    void OnCollisionEnter(Collision Bumper)
    {
        if (Bumper.gameObject.tag == "Bouncer")
            {
            var _bumpSpeed = Mathf.Clamp(bumperVelocity.magnitude * _bumpForce, _bumpMIN, _bumpMAX); //récupère la vitesse (magnitude) du player et la factorise avec la puissance que le bump va renvoyer, et défini une distance de saut de bumper Min et Max 
            Vector3 _mirrorDirection = Vector3.Reflect(bumperVelocity.normalized, Bumper.contacts[0].normal); //Vecteur Miroir réfléchi sur la normale du bumper.
            Vector3 _bumpDirection = Vector3.Lerp(_mirrorDirection, Bumper.contacts[0].normal, _bumpInfluence); //Définition de l'influence de la direction du bumper (max 1) sur le vecteur miroir (min 0) réfléchi dessus.

            rb.velocity += _bumpDirection * _bumpSpeed; //application de la vélocité sur le player

            //_velovityBug = _bumpDirection * _bumpSpeed; //Debug
            //Debug.Log("Bumper Direction: " + _bumpDirection);
            //Debug.Log("Reflect Result: " + _velovityBug);
            //Debug.Log("Final Velocity: " + _rb.velocity);
            //_debugIsFrameCollide = true;
        }
    }
}
