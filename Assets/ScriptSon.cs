using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptSon : MonoBehaviour
{

    private FMOD.Studio.EventInstance Music;
    // Start is called before the first frame update
    void Start()
    {
        Music = FMODUnity.RuntimeManager.CreateInstance("event:/WorldBehave/Ambiance");
        Music.start();
    }

    // Update is called once per frame
    void Update()
    {

        Music.setParameterByName("Hateur", transform.position.y);
    }
}
