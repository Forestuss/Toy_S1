using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTimer : MonoBehaviour
{
    public float tempsAvantDestruct = 5f;
    public float tempsDestruct;

    // Start is called before the first frame update
    void Start()
    {
        tempsAvantDestruct = tempsAvantDestruct * 60f; // converti le temps reseigné en minutes
        tempsDestruct = tempsAvantDestruct;
    }

    // Update is called once per frame
    void Update()
    {
        tempsDestruct -= Time.deltaTime;

        if(tempsDestruct < 0)
        {
            Destroy(gameObject);
        }
    }
}
