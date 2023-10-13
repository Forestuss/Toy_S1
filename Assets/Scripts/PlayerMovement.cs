using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.Animations;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody _rb;
    private bool _isGrounded;
    private bool _isBoosted;
    private bool _isBumping;
    private Vector3 originalVelocity;
    private Vector3 bumperVelocity;

    private float yaw = 0.0f, pitch = 0.0f;
    [SerializeField] private float _speed = 1.0f, _jumpForce, _sensitivity;

    private float _originalSensitivity;

    public float _liquidSpeed; //multiplicateur de vitesse dans la bulle. 1 signifie que la vitesse est la même dans la bulle qu'a l'exterieur. 
    public float _liquidLook; //Sensivité de la caméra dans la bulle (ne pas mettre une sensibilité au dessus de la sensibilité de base) 

    public float _bumpForce; //puissance factorielle du bumper. 1 signifie que le bumper renvoie la même force que l'on lui donne. Valeur de base 0.9 (légère perte de puissance de vélocité à chaque saut de bumper)
    public float _bumpInfluence; //influence de la direction du bumper sur la trajectoire du joueur. Uniquement mettre une valeur float entre 0 et 1. 0 signifie que le player rebondit comme sur un miroir, et 1 qu'il suit complètement la direction du bumper.
    public float _bumpMIN; //Le minimum de base est de 50. Empêche le joueur de faire de touts petits sauts par erreurs.
    public float _bumpMAX; //le maximum de base est 100. Empêche le joueur de stacker trop de vitesse après au saut. 
    
    private bool _debugIsFrameCollide = false; //DEBUG 
    private Vector3 _velovityBug; //DEBUG
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _originalSensitivity = _sensitivity;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Physics.Raycast(_rb.transform.position, Vector3.down, 2.5f)) //tuto ytb pour le saut
        {
            _rb.velocity = new Vector3(_rb.velocity.x, _jumpForce, _rb.velocity.z);
        }
        Look();

        bumperVelocity = _rb.velocity;
    }

    void Look() //tuto ytb pour la souris (fonctionnel)
    {

        pitch -= Input.GetAxisRaw("Mouse Y") * _sensitivity;
        pitch = Mathf.Clamp(pitch, -90.0f, 90.0f);
        yaw += Input.GetAxisRaw("Mouse X") * _sensitivity;
        Camera.main.transform.localRotation = Quaternion.Euler(pitch, yaw, 0);
    }

    void FixedUpdate()
    {
        if (_isBoosted == false)
        {
            //Movement(); //activer ou réactiver si l'on veut tester les bumpers (ils ne fonctionnent pas quand activé). 
        }

        //if (_debugIsFrameCollide == true) //Debug
        //{
            //Debug.Log("After Velocity: " + _rb.velocity);
            //_debugIsFrameCollide = false;
        //}
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

