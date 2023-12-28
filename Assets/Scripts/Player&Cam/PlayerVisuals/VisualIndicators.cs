using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualIndicators : MonoBehaviour
{
    public GameObject player;

    public GameObject indicatorBumper1;
    public GameObject indicatorBumper2;
    public GameObject indicatorBumper3;

    public GameObject indicatorRing1;
    public GameObject indicatorRing2;
    public GameObject indicatorRing3;
    public GameObject indicatorRing4;

    void Update()
    {
        if(player.GetComponent<SpawnBumper>().bumperCharge == 3)
        {
            indicatorBumper1.transform.GetComponent<Renderer>().enabled = true;
            indicatorBumper2.transform.GetComponent<Renderer>().enabled = true;
            indicatorBumper3.transform.GetComponent<Renderer>().enabled = true;
        } 
        if(player.GetComponent<SpawnBumper>().bumperCharge == 2)
        {
            indicatorBumper1.transform.GetComponent<Renderer>().enabled = false;
        }
        if(player.GetComponent<SpawnBumper>().bumperCharge == 1)
        {
            indicatorBumper2.transform.GetComponent<Renderer>().enabled = false;
        }
        if(player.GetComponent<SpawnBumper>().bumperCharge == 0)
        {
            indicatorBumper3.transform.GetComponent<Renderer>().enabled = false;
        }


        if (player.GetComponent<SpawnRing>().ringCharge == 4)
        {
            indicatorRing1.transform.GetComponent<Renderer>().enabled = true;
            indicatorRing2.transform.GetComponent<Renderer>().enabled = true;
            indicatorRing3.transform.GetComponent<Renderer>().enabled = true;
            indicatorRing4.transform.GetComponent<Renderer>().enabled = true;
        }
        if (player.GetComponent<SpawnRing>().ringCharge == 3)
        {
            indicatorRing1.transform.GetComponent<Renderer>().enabled = false;
        } 
        if (player.GetComponent<SpawnRing>().ringCharge == 2)
        {
            indicatorRing2.transform.GetComponent<Renderer>().enabled = false;
        }
        if (player.GetComponent<SpawnRing>().ringCharge == 1)
        {
            indicatorRing3.transform.GetComponent<Renderer>().enabled = false;
        }
        if (player.GetComponent<SpawnRing>().ringCharge == 0)
        {
            indicatorRing4.transform.GetComponent<Renderer>().enabled = false;
        }
    }
}
