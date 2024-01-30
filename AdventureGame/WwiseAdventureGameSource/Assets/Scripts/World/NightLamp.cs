////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using UnityEngine;
using System.Collections;

public class NightLamp : MonoBehaviour
{

    public Light lampLight;
    public float fadeTime = 2f;
    private float origIntensity;

    void Awake()
    {
        origIntensity = lampLight.intensity;
    }

    void OnEnable()
    {
        GameManager.DayNightCall += SetDayTime;
    }

    void OnDisable(){
        GameManager.DayNightCall -= SetDayTime;
    }

    void SetDayTime(bool condition)
    {
        if (condition)
        {
            StartCoroutine(SetLightStrength(0f));
        }
        else
        {
            StartCoroutine(SetLightStrength(origIntensity));
        }
    }

    IEnumerator SetLightStrength(float intensity)
    {
        float current = lampLight.intensity;

        for (float t = 0f; t < 1; t += Time.deltaTime / fadeTime)
        {
            lampLight.intensity = Mathf.Lerp(current, intensity, t);
            yield return null;
        }
        lampLight.intensity = intensity;
    }
}
