using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lesson4Script : MonoBehaviour
{
    public AK.Wwise.Event Step;
    // Start is called before the first frame update
    void PlayFootstepSound()
    {
        Step.Post(gameObject);
    }
}
