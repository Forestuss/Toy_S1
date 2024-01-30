using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacingParticles : MonoBehaviour
{
    [SerializeField] private GameObject _particles;
    // Start is called before the first frame update

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("CloudZone"))
            _particles.transform.position = new Vector3(transform.position.x, _particles.transform.position.y, transform.position.z);
    }
}
