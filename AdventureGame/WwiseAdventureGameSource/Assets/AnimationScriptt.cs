using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationScriptt : MonoBehaviour
{
    // Declare
    public AK.Wwise.Event Step;
    // Start is called before the first frame update
    void PlayFootstepSound()
    {
        Step.Post(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}