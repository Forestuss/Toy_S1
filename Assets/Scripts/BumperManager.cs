using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BumperManager : MonoBehaviour
{
    private bool isBumping;
    private Rigidbody rb;
    private Vector3 bumperVelocity;

    [SerializeField] private GameObject bloc;
    [SerializeField] private GameObject Target;
    [SerializeField] private GameObject Player;

    [Header("Bump")]
    public float _bumpForce; //puissance factorielle du bumper. 1 signifie que le bumper renvoie la m�me force que l'on lui donne. Valeur de base 0.9 (l�g�re perte de puissance de v�locit� � chaque saut de bumper)
    public float _bumpInfluence; //influence de la direction du bumper sur la trajectoire du joueur. Uniquement mettre une valeur float entre 0 et 1. 0 signifie que le player rebondit comme sur un miroir, et 1 qu'il suit compl�tement la direction du bumper.
    public float _bumpMIN; //Le minimum de base est de 50. Emp�che le joueur de faire de touts petits sauts par erreurs.
    public float _bumpMAX; //le maximum de base est 100. Emp�che le joueur de stacker trop de vitesse apr�s au saut. 


    private GameObject instancePad;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        /* Preview placement bumper
        if (Input.GetMouseButtonDown(1))
        {
            instancePad = Instantiate(bloc, Target.transform.position, Camera.main.transform.rotation, Player.transform);
            instancePad.GetComponent<Collider>().enabled = false;
        }

        if (Input.GetMouseButton(1))
        {
            instancePad.transform.rotation = Camera.main.transform.rotation;
            instancePad.transform.position = Target.transform.position;
        }

        if (Input.GetMouseButtonUp(1))
        {
            instancePad.GetComponent<Collider>().enabled = true;
            instancePad.transform.SetParent(null);
        }
        */
    }


    void OnCollisionEnter(Collision Bumper)
    {
        if (Bumper.gameObject.tag == "Bouncer")
        {
            var _bumpSpeed = Mathf.Clamp(bumperVelocity.magnitude * _bumpForce, _bumpMIN, _bumpMAX); //r�cup�re la vitesse (magnitude) du player et la factorise avec la puissance que le bump va renvoyer, et d�fini une distance de saut de bumper Min et Max 
            Vector3 _mirrorDirection = Vector3.Reflect(bumperVelocity.normalized, Bumper.contacts[0].normal); //Vecteur Miroir r�fl�chi sur la normale du bumper.
            Vector3 _bumpDirection = Vector3.Lerp(_mirrorDirection, Bumper.contacts[0].normal, _bumpInfluence); //D�finition de l'influence de la direction du bumper (max 1) sur le vecteur miroir (min 0) r�fl�chi dessus.

            rb.velocity = _bumpDirection * _bumpSpeed; //application de la v�locit� sur le player

            //_velovityBug = _bumpDirection * _bumpSpeed; //Debug
            //Debug.Log("Bumper Direction: " + _bumpDirection);
            //Debug.Log("Reflect Result: " + _velovityBug);
            //Debug.Log("Final Velocity: " + _rb.velocity);
            //_debugIsFrameCollide = true;
        }
    }
}
