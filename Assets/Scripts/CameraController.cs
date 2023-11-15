using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class CameraController : MonoBehaviour
{
    private Rigidbody rb;

    [SerializeField] private float sensitivity;
    private float yaw = 0.0f, pitch = 0.0f;

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
    void Update()
    {
        Look();
        
        //cameraDistance = Mathf.Clamp(cameraDistance,70f, 90f);
        // cameraDistance = Mathf.SmoothDamp(cameraDistance, rb.velocity.magnitude, ref velocityf, 0.001f);
        Camera.main.transform.localPosition = new Vector3(0, cameraHeight, -cameraDistance);

        //Camera.main.transform.localPosition = Vector3.SmoothDamp(Camera.main.transform.localPosition, cameraTarget.transform.position, ref velocity, 0.1f);
        //Camera.main.transform.localRotation = cameraTarget.transform.rotation;
        
    }

    void Look() //tuto ytb pour la souris (fonctionnel)
    {
        pitch -= Input.GetAxisRaw("Mouse Y") * sensitivity;
        pitch = Mathf.Clamp(pitch, -90.0f, 90.0f);
        yaw += Input.GetAxisRaw("Mouse X") * sensitivity;
        transform.localRotation = Quaternion.Euler(0, yaw, 0);
        cameraPivot.transform.localRotation = Quaternion.Euler(pitch, 0, 0);
    }
}
