using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostWwiseEvent : MonoBehaviour
{
    public AK.Wwise.Event Step; 
    void PlayFootstepSound()
    {
        PlayerManager.foot_L.GetMaterial().SetValue(gameObject);
        Step.Post(gameObject);
    }


}
