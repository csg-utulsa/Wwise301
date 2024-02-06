using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript007 : MonoBehaviour
{
    public AK.Wwise.Event DestroyEvent;
    // Start is called before the first frame update
    void Start()
    {
        DestroyEvent.Post(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
