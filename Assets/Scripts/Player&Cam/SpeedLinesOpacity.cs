using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedLinesOpacity : MonoBehaviour
{
    public ParticleSystem particle;
    public Material speedLinesMaterial;
    public GameObject player;

    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = player.transform.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        var emission = particle.emission;
        var vitesse = rb.velocity.magnitude;

        if (vitesse > 100)
            speedLinesMaterial.color = new Color(speedLinesMaterial.color.r, speedLinesMaterial.color.g, speedLinesMaterial.color.b, vitesse/300);
        else
            speedLinesMaterial.color = new Color(speedLinesMaterial.color.r, speedLinesMaterial.color.g, speedLinesMaterial.color.b, 0);



        Mathf.Clamp(vitesse, 100, 300);

        emission.rateOverTime = vitesse;
    }
}
