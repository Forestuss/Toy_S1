using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpPad : MonoBehaviour
{
    public float lerpSpeed = 0.05f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.GetChild(0).localScale != new Vector3(1f, 1f, 1f))
        {
            transform.GetChild(0).localScale = Vector3.Lerp(transform.GetChild(0).localScale, new Vector3(1f, 1f, 1f), lerpSpeed);
        }
        
    }
}
