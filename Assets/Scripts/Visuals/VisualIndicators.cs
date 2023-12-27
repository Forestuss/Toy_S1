using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualIndicators : MonoBehaviour
{
    public GameObject player;

    public GameObject indicatorPad1;
    public GameObject indicatorPad2;
    public GameObject indicatorPad3;

    public GameObject indicatorRing1;
    public GameObject indicatorRing2;
    public GameObject indicatorRing3;
    public GameObject indicatorRing4;

    // Update is called once per frame
    void Update()
    {
        if(player.GetComponent<SpawnBouncer>().bumperCharge == 3)
        {
            indicatorPad1.transform.GetComponent<Renderer>().enabled = true;
            indicatorPad2.transform.GetComponent<Renderer>().enabled = true;
            indicatorPad3.transform.GetComponent<Renderer>().enabled = true;
        } 
        if(player.GetComponent<SpawnBouncer>().bumperCharge == 2)
        {
            indicatorPad1.transform.GetComponent<Renderer>().enabled = false;
        }
        if(player.GetComponent<SpawnBouncer>().bumperCharge == 1)
        {
            indicatorPad2.transform.GetComponent<Renderer>().enabled = false;
        }
        if(player.GetComponent<SpawnBouncer>().bumperCharge == 0)
        {
            indicatorPad3.transform.GetComponent<Renderer>().enabled = false;
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
