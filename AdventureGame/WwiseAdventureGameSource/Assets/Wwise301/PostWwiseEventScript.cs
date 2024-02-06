using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostWwiseEventScript : MonoBehaviour
{
    // Start is called before the first frame update
    public AK.Wwise.Event Event;
    void Start()
    {
        Event.Post(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
