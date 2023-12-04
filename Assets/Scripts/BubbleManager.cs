using Unity.VisualScripting;
using UnityEngine;

public class BubbleManager : MonoBehaviour
{
    private Rigidbody rb;

    private float _originalFOV = 80f;
    private float _newFOV = 110f;
    private float _tempoFOV;

    private float velocity = 0f;

    public float smoothTime = 0.5f;

    public bool _inBubble = false;

    [SerializeField] private GameObject bubble;
    [SerializeField] private GameObject Target;

    [SerializeField] private GameObject player;

    private GameObject instanceBubble;

    public bool isBoosted;
    private Vector3 originalVelocity;

    [Header("Liquid")]
    public float _liquidSpeed; //multiplicateur de vitesse dans la bulle. 1 signifie que la vitesse est la même dans la bulle qu'a l'exterieur. 
    public float _liquidLook; //Sensivité de la caméra dans la bulle (ne pas mettre une sensibilité au dessus de la sensibilité de base) 
    [Range(20f ,50f)] public float maxSizeBubble;
    [Range(0f, 30)] public float minSizeBubble;

    public float randomSize;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;

        
    }

    void Update()
    {
        if(_inBubble)
        {
            _tempoFOV = (Mathf.SmoothDamp(Camera.main.fieldOfView, _newFOV, ref velocity, smoothTime));
            
        }
        else
        {
            _tempoFOV = (Mathf.SmoothDamp(Camera.main.fieldOfView, _originalFOV, ref velocity, smoothTime));
           
        }
        Camera.main.fieldOfView = _tempoFOV;

        if (Input.GetMouseButtonDown(0))
        {
            randomSize = Random.Range(minSizeBubble, maxSizeBubble);
            instanceBubble = Instantiate(bubble, Target.transform.position, Quaternion.identity);
            
            instanceBubble.transform.localScale = new Vector3(randomSize ,randomSize ,randomSize);
        }




        //if (Input.GetMouseButtonDown(0))
        //{
        //    instanceBubble = Instantiate(bubble, Target.transform.position, Quaternion.identity, player.transform);
        //}

        //if (Input.GetMouseButton(0) && instanceBubble.transform.localScale.x < maxSizeBubble)
        //{
        //    instanceBubble.transform.localScale = instanceBubble.transform.localScale + new Vector3(0.1f, 0.1f, 0.1f);
        //}

        //if (Input.GetMouseButton(0) && instanceBubble.transform.localScale.x < maxSizeBubble)
        //{
        //    instanceBubble.transform.rotation = Camera.main.transform.rotation;
        //    instanceBubble.transform.position = Target.transform.position;
        //}
        
        //if (Input.GetMouseButtonUp(0))
        //{
        //    instanceBubble.transform.SetParent(null);
        //}
    }
    void OnTriggerEnter(Collider Liquid)
    {
        if (Liquid.gameObject.tag == "BoostLiquid")
        {
            rb.useGravity = false; //enlève la gravité dans le liquide de boost 
            //isBoosted = true; // annule la possibilité de mouvement 
            originalVelocity = rb.velocity;
            _inBubble = true;
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
            //isBoosted = false;
            rb.useGravity = true;
            _inBubble = false;
        }
    }
}
