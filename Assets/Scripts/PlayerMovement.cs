using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody _rb;
    private bool _isGrounded;
    private bool _isBoosted;
    private Vector3 originalVelocity;

    private float yaw = 0.0f, pitch = 0.0f;
    [SerializeField] private float _speed = 1.0f, _jumpForce, _sensitivity = 2.0f;

    public float _boostSpeed;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Physics.Raycast(_rb.transform.position, Vector3.down, 2.5f)) //tuto ytb pour le saut
        {
            _rb.velocity = new Vector3(_rb.velocity.x, _jumpForce, _rb.velocity.z);
        }
        Look();
    }//zzzzzzz


    void Look() //tuto ytb pour la souris
    {

        pitch -= Input.GetAxisRaw("Mouse Y") * _sensitivity;
        pitch = Mathf.Clamp(pitch, -90.0f, 90.0f);
        yaw += Input.GetAxisRaw("Mouse X") * _sensitivity;
        Camera.main.transform.localRotation = Quaternion.Euler(pitch, yaw, 0);
    }

    private void FixedUpdate()
    {
        if (_isBoosted == false)
        {
            Movement();
        }

    }

    void Movement() //tuto ytb pour le mouvement (fonctionne en changant directement la vélocité) 
    {
        Vector2 axis = new Vector2(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal")).normalized * _speed; 
        Vector3 forward = new Vector3(-Camera.main.transform.right.z, 0.0f, Camera.main.transform.right.x);
        Vector3 direction = forward * axis.x + Camera.main.transform.right * axis.y + Vector3.up * _rb.velocity.y;
        _rb.velocity = direction;
    }

    private void OnTriggerEnter(Collider Liquid)
    {
        if (Liquid.gameObject.tag == "BoostLiquid")
        {
            _rb.useGravity = false; //enlève la gravité dans le liquide de boost 
            _isBoosted = true; // annule la possibilité de mouvement 
            originalVelocity = _rb.velocity;
        }
    }

    private void OnTriggerStay(Collider Liquid)
    {
        if (Liquid.gameObject.tag == "BoostLiquid")
        {
            _sensitivity = Mathf.Min(_sensitivity / 1.05f, 1f); //réduit la sensi de la souris dans le liquide de boost (pas néscessaire) 
            _rb.AddForce(_rb.velocity * _boostSpeed, ForceMode.Force); //boost de la velocité 
        }
    }

    private void OnTriggerExit(Collider Liquid)
    {
        if (Liquid.gameObject.tag == "BoostLiquid")
        {
            _isBoosted = false;
            _sensitivity = 2; 
            _rb.useGravity = true; 
        }
    }
}
