using UnityEngine;

public class BubbleManager : MonoBehaviour
{
    private Rigidbody rb;

    private bool isBoosted;
    private Vector3 originalVelocity;

    [Header("Liquid")]
    public float _liquidSpeed; //multiplicateur de vitesse dans la bulle. 1 signifie que la vitesse est la même dans la bulle qu'a l'exterieur. 
    public float _liquidLook; //Sensivité de la caméra dans la bulle (ne pas mettre une sensibilité au dessus de la sensibilité de base) 

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnTriggerEnter(Collider Liquid)
    {
        if (Liquid.gameObject.tag == "BoostLiquid")
        {
            rb.useGravity = false; //enlève la gravité dans le liquide de boost 
            isBoosted = true; // annule la possibilité de mouvement 
            originalVelocity = rb.velocity;
        }
    }

    void OnTriggerStay(Collider Liquid)
    {
        if (Liquid.gameObject.tag == "BoostLiquid")
        {
            //_sensitivity = Mathf.Min(_sensitivity / 1.05f, _liquidLook); //réduit la sensi de la souris dans le liquide de boost (pas néscessaire) 
            rb.AddForce(rb.velocity * _liquidSpeed, ForceMode.Force); //boost de la velocité 
        }
    }

    void OnTriggerExit(Collider Liquid)
    {
        if (Liquid.gameObject.tag == "BoostLiquid")
        {
            isBoosted = false;
            rb.useGravity = true;
        }
    }
}
