using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DefaultSliderValueSetterMobile : MonoBehaviour
{
    public Slider sliderToSet;
    public float defaultValue;

#if UNITY_IOS || UNITY_ANDROID
    void Start()
    {
        sliderToSet.value = defaultValue;
    }
#endif
}
