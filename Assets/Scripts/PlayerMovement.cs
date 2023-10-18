using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.Animations;

public class PlayerMovement : MonoBehaviour
{

    private Rigidbody _rb;
    private Vector3 bumperVelocity;
        
    private bool _isBoosted;
    private bool _isBumping;
    private Vector3 originalVelocity;

    private bool _debugIsFrameCollide = false; //DEBUG 
    private Vector3 _velovityBug; //DEBUG

    private float _verticalInput;
    private float _horizontalInput;

    private Vector3 _movementDir;

    private bool _jumping;
    private bool _isGrounded;


    private float yaw = 0.0f, pitch = 0.0f;

    [Header("Controller")]
    [SerializeField] private float _speed = 1.0f;
    [SerializeField] private float maxSpeed = 200f;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _sensitivity;

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

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _originalSensitivity = _sensitivity;
       
    }

    void Update()
    {
        // Récuperation de données pour le mouvement
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");
        
        // Mouvement de la caméra
        Look();

        // Récuperation des données pour le saut
        if (Physics.Raycast(_rb.transform.position, Vector3.down, 2.5f))
        {
            _isGrounded = true;
        }
        else
        {
            _isGrounded = false;
        }

        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
        {
            _jumping = true;
        }

        Camera.main.transform.localPosition = new Vector3(0, 0, - _cameraDistance);

        bumperVelocity = _rb.velocity;
    }

    void FixedUpdate()
    {
        //MovePlayer();
        MovePlayerAlternate();
        JumpPlayer();
    }

    private void MovePlayer()
    {
        /*_movementDir = new Vector3(_horizontalInput, _rb.velocity.y, _verticalInput);
        _rb.AddRelativeForce(_movementDir.normalized * _speed, ForceMode.Force);*/
        if(_isGrounded)
        {
            _movementDir = transform.forward * _verticalInput + transform.right * _horizontalInput;
            _rb.AddForce(_movementDir.normalized * _speed, ForceMode.Force);
            Debug.Log("Grounded");
        }
        else
        {
            _movementDir = transform.forward * _verticalInput + transform.right * _horizontalInput;
            _rb.AddForce(_movementDir.normalized * _speed / 3, ForceMode.Force);
            Debug.Log("Not Grounded");
        }

        /*
        if(_rb.velocity.magnitude > 10)
        {
            _rb.velocity = _rb.velocity.normalized * maxSpeed;
        }
        */
    }

    private void MovePlayerAlternate()
    {
        if (_isGrounded)
        {
            _movementDir = transform.forward * _verticalInput + transform.right * _horizontalInput;
            _rb.AddForce(_movementDir.normalized * _speed, ForceMode.Force);
            Debug.Log("Grounded");
        }
        else
        {
            _movementDir = transform.forward * _verticalInput + transform.right * _horizontalInput;
            _rb.AddForce(_movementDir.normalized * _speed / 3, ForceMode.Force);
            Debug.Log("Not Grounded");
        }
    }

    private void JumpPlayer()
    {
        if(_jumping)
        {
            _rb.velocity = new Vector3(_rb.velocity.x, _jumpForce, _rb.velocity.z);
            _jumping = false;
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

    

    void Movement() //tuto ytb pour le mouvement (fonctionne en changant directement la vélocité) 
    {
        Vector2 axis = new Vector2(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal")).normalized * _speed; 
        Vector3 forward = new Vector3(-Camera.main.transform.right.z, 0.0f, Camera.main.transform.right.x);
        Vector3 direction = forward * axis.x + Camera.main.transform.right * axis.y + Vector3.up * _rb.velocity.y;
        _rb.velocity = direction;
    }

    void OnTriggerEnter(Collider Liquid)
    {
        if (Liquid.gameObject.tag == "BoostLiquid")
        {
            _rb.useGravity = false; //enlève la gravité dans le liquide de boost 
            _isBoosted = true; // annule la possibilité de mouvement 
            originalVelocity = _rb.velocity;
        }
    }

    void OnTriggerStay(Collider Liquid)
    {
        if (Liquid.gameObject.tag == "BoostLiquid")
        {
            _sensitivity = Mathf.Min(_sensitivity / 1.05f, _liquidLook); //réduit la sensi de la souris dans le liquide de boost (pas néscessaire) 
            _rb.AddForce(_rb.velocity * _liquidSpeed, ForceMode.Force); //boost de la velocité 
        }
    }

    void OnTriggerExit(Collider Liquid)
    {
        if (Liquid.gameObject.tag == "BoostLiquid")
        {
            _isBoosted = false;  
            _sensitivity = _originalSensitivity; 
            _rb.useGravity = true; 
        }
    }

    void OnCollisionEnter(Collision Bumper)
    {
        if (Bumper.gameObject.tag == "Bouncer")
            {
            var _bumpSpeed = Mathf.Clamp(bumperVelocity.magnitude * _bumpForce, _bumpMIN, _bumpMAX); //récupère la vitesse (magnitude) du player et la factorise avec la puissance que le bump va renvoyer, et défini une distance de saut de bumper Min et Max 
            Vector3 _mirrorDirection = Vector3.Reflect(bumperVelocity.normalized, Bumper.contacts[0].normal); //Vecteur Miroir réfléchi sur la normale du bumper.
            Vector3 _bumpDirection = Vector3.Lerp(_mirrorDirection, Bumper.contacts[0].normal, _bumpInfluence); //Définition de l'influence de la direction du bumper (max 1) sur le vecteur miroir (min 0) réfléchi dessus.

            _rb.velocity = _bumpDirection * _bumpSpeed; //application de la vélocité sur le player

            //_velovityBug = _bumpDirection * _bumpSpeed; //Debug
            //Debug.Log("Bumper Direction: " + _bumpDirection);
            //Debug.Log("Reflect Result: " + _velovityBug);
            //Debug.Log("Final Velocity: " + _rb.velocity);
            //_debugIsFrameCollide = true;
        }
    }
}

