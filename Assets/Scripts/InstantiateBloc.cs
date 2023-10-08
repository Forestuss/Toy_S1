using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateBloc : MonoBehaviour
{
    [SerializeField]
    private GameObject bloc;
    [SerializeField]
    private GameObject Target;
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Instantiate(bloc, Target.transform.position, Quaternion.identity);
        }
    }
}
