using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetRTPCToDistance : MonoBehaviour
{
    public AK.Wwise.RTPC GameParameter;
    public Transform OtherTransform;
    float distance;

    // Start is called before the first frame update
    

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(OtherTransform.position, transform.position);
        GameParameter.SetGlobalValue(distance);
    }
}
