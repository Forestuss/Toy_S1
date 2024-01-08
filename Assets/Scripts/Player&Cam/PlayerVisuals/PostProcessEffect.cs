using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessEffect : MonoBehaviour
{

    [SerializeField] private Volume _volume;
    [SerializeField] private GameObject _player;
    private LensDistortion _distortion;

    private Rigidbody _rb;
    private float vitesse;

    // Start is called before the first frame update
    void Start()
    {
        _volume.profile.TryGet<LensDistortion>(out _distortion);
        _volume = GetComponent<Volume>();
        _rb = _player.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        vitesse = (_rb.velocity.magnitude - 200);
        vitesse = Mathf.Clamp(vitesse, 0, 100);

        Debug.Log(vitesse);
    }
}
