using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpBumper : MonoBehaviour
{
    public float lerpSpeed = 0.05f;

    void Update()
    {
        if(transform.GetChild(0).localScale != new Vector3(1f, 1f, 1f))
        {
            transform.GetChild(0).localScale = Vector3.Lerp(transform.GetChild(0).localScale, new Vector3(1f, 1f, 1f), lerpSpeed);
        } 
    }
}
