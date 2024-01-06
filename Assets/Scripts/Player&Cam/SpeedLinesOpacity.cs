using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedLinesOpacity : MonoBehaviour
{
    public ParticleSystem particle;
    public Material speedLinesMaterial;
    public GameObject player;

    private Rigidbody _rb;

    public float radiusTempo;
    // Start is called before the first frame update
    void Start()
    {
        _rb = player.transform.GetComponent<Rigidbody>();
        
    }

    // Update is called once per frame
    void Update()
    {
        var emission = particle.emission;
        var vitesse = _rb.velocity.magnitude / 300;
        var radius = particle.shape;
        
        
        if (vitesse > 0.2)
            speedLinesMaterial.color = new Color(speedLinesMaterial.color.r, speedLinesMaterial.color.g, speedLinesMaterial.color.b, vitesse);
        else
            speedLinesMaterial.color = new Color(speedLinesMaterial.color.r, speedLinesMaterial.color.g, speedLinesMaterial.color.b, 0);

        emission.rateOverTime = vitesse * 600;

        radius.radius =  Mathf.Clamp((- vitesse * 12) + 22, 12.8f ,15);

        radiusTempo = radius.radius;
    }
}
