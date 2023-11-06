using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float sensitivity;
    private float yaw = 0.0f, pitch = 0.0f;

    [Header("Camera")]
    [Range(0.0f, 70.0f)] public float cameraDistance;
    [Range(-2.0f, 2.0f)] public float cameraHeight;
    public GameObject cameraPivot;

    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {
        Look();
        Camera.main.transform.localPosition = new Vector3(0, cameraHeight, -cameraDistance);
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
