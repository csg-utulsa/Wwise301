using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class footstepstink : MonoBehaviour
{
    public AK.Wwise.Event Steps;

    // Start is called before the first frame update
    void footstepsmile()
    {
        Steps.Post(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
