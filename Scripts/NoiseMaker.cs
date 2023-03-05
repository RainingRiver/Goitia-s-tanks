using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseMaker : MonoBehaviour
{
    public float volumeDistance;
    public float decayPerFrame;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(volumeDistance > 0)
        {
            volumeDistance -= decayPerFrame;
        }
    }

}
