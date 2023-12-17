using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class CameraController : MonoBehaviour
{
    private Rigidbody rb;

    [SerializeField] private float sensitivity;
    private float yaw = 0.0f, pitch = 0.0f;

    public float smoothTime = 0.5f;

    private Vector3 refVelocity = Vector3.zero;

    [Header("Camera")]
    [Range(0.0f, 70.0f)] public float cameraDistance;
    [Range(-2.0f, 2.0f)] public float cameraHeight;
    public GameObject cameraPivot;
    public GameObject cameraTarget;

    private float velocityf = 0f;
    private Vector3 velocity = Vector3.zero;

    // Start is called before the first frame update

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    private void Update()
    {
        LookValues();

    }

    void FixedUpdate()
    {

        Look();
        
    }

    void LookValues()
    {
        pitch -= Input.GetAxisRaw("Mouse Y") * sensitivity;
        pitch = Mathf.Clamp(pitch, -90.0f, 90.0f);
        yaw += Input.GetAxisRaw("Mouse X") * sensitivity;
    }

    void Look() //tuto ytb pour la souris (fonctionnel)
    {
        transform.localRotation = Quaternion.Euler(0, yaw, 0);
        cameraPivot.transform.localRotation = Quaternion.Euler(pitch, 0, 0);
    }
}
