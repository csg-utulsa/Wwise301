using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetRTPCToDistance : MonoBehaviour
{
    public AK.Wwise.RTPC GameParameter;
    public Transform OtherTransform;
    float distance;

    // Update is called once per frame
    private void Update()
    {
        distance = Vector3.Distance(OtherTransform.position, transform.position);
        GameParameter.SetGlobalValue(distance);
    }
}