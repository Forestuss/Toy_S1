using UnityEngine;

public class BubbleManager : MonoBehaviour
{
    private Rigidbody rb;

    [SerializeField] private GameObject bubble;
    [SerializeField] private GameObject Target;

    [SerializeField] private GameObject player;

    private GameObject instanceBubble;

    public bool isBoosted;
    private Vector3 originalVelocity;

    [Header("Liquid")]
    public float _liquidSpeed; //multiplicateur de vitesse dans la bulle. 1 signifie que la vitesse est la m�me dans la bulle qu'a l'exterieur. 
    public float _liquidLook; //Sensivit� de la cam�ra dans la bulle (ne pas mettre une sensibilit� au dessus de la sensibilit� de base) 
    public float maxSizeBubble = 20;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            instanceBubble = Instantiate(bubble, Target.transform.position, Camera.main.transform.rotation);
        }

        /*if (Input.GetMouseButtonDown(0))
        {
            instanceBubble = Instantiate(bubble, Target.transform.position, Quaternion.identity, player.transform);
        }

        if (Input.GetMouseButton(0) && instanceBubble.transform.localScale.x < maxSizeBubble)
        {
            instanceBubble.transform.localScale = instanceBubble.transform.localScale + new Vector3(0.1f, 0.1f, 0.1f);
        }

        if (Input.GetMouseButton(0) && instanceBubble.transform.localScale.x < maxSizeBubble)
        {
            instanceBubble.transform.rotation = Camera.main.transform.rotation;
            instanceBubble.transform.position = Target.transform.position;
        }
        
        if (Input.GetMouseButtonUp(0))
        {
            instanceBubble.transform.SetParent(null);
        }*/
    }
    void OnTriggerEnter(Collider Liquid)
    {
        if (Liquid.gameObject.tag == "BoostLiquid")
        {
            rb.useGravity = false; //enl�ve la gravit� dans le liquide de boost 
            //isBoosted = true; // annule la possibilit� de mouvement 
            originalVelocity = rb.velocity;
        }
    }

    void OnTriggerStay(Collider Liquid)
    {
        if (Liquid.gameObject.tag == "BoostLiquid")
        {
            //_sensitivity = Mathf.Min(_sensitivity / 1.05f, _liquidLook); //r�duit la sensi de la souris dans le liquide de boost (pas n�scessaire) 
            rb.AddForce(rb.velocity * _liquidSpeed, ForceMode.Force); //boost de la velocit� 
        }
    }

    void OnTriggerExit(Collider Liquid)
    {
        if (Liquid.gameObject.tag == "BoostLiquid")
        {
            //isBoosted = false;
            rb.useGravity = true;
        }
    }
}
