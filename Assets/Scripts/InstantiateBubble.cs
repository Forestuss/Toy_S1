using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateBubble : MonoBehaviour
{
    [SerializeField] private GameObject bubble;
    [SerializeField] private GameObject Target;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Instantiate(bubble, Target.transform.position, Quaternion.identity);
        }
    }
}
