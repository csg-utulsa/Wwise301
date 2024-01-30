////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode, RequireComponent(typeof(Light))]
public class DayNightCycle : MonoBehaviour
{

    [Header("Setup")]
    public bool autoplay;

    [Range(-360, 360)]
    public float rotationOffset = 45f;
    [Range(0, 100)]
    public float timeSpeed;
    private float origTimeSpeed;
    public bool variableTimeSpeed = false;

    [ShowIf("variableTimeSpeed", true)]
    public AnimationCurve SpeedMultiplier = new AnimationCurve(new Keyframe[] { new Keyframe(0, 1), new Keyframe(1, 1) });

    public AnimationCurve sunIntensityCurve;
    public float sunLightIntensityMultiplier = 1f;
    public Gradient ambientColor;

    public bool differentFogColor;
    [ShowIf("differentFogColor", true)]
    public Gradient fogColor;

    //Moon
    public Light moonLight;
    public float moonLightIntensityMultiplier = 1f;

    [Header("Current Time of Day")]
    [Range(0f, 24f)]
    public float timeOfDay;

    private Slider menuSlider;

    private float timeOfDayInDegrees;
    private Light sunLight;

    [HideInInspector]
    public bool setLightIntensity = true;
    [HideInInspector]
    public bool setAmbientColor = true;
    [HideInInspector]
    public bool setFogColor = true;

    GameManager GM;

    private const string sunSliderString = "SunSlider";

    void OnValidate()
    {
        origTimeSpeed = timeSpeed;
    }

    void OnEnable()
    {
        sunLight = GetComponent<Light>();

        //Set the light in the Lighting Settings
        RenderSettings.sun = sunLight;

        origTimeSpeed = timeSpeed;

        // Set up any SunSlider
        GameObject SunsliderObj = GameObject.Find(sunSliderString);
        if (SunsliderObj != null)
        {
            menuSlider = SunsliderObj.GetComponent<Slider>();
        }

        if (menuSlider != null)
        {
            menuSlider.onValueChanged.AddListener(delegate { UpdateDayNightCycle(menuSlider.value); });
        }
    }

    void OnDisable()
    {
        if (menuSlider != null)
        {
            menuSlider.onValueChanged.RemoveListener(delegate
            {
                UpdateDayNightCycle(menuSlider.value);
            });
        }

    }

    void UpdateDayNightCycle(float tod)
    {
        if (tod < 24f)
        {
            timeOfDay = tod;
        }

        foreach (Image i in menuSlider.GetComponentsInChildren<Image>())
        { //TODO: Cache all Images in menuSlider children
            i.color = ambientColor.Evaluate(timeOfDay / 24);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (sunLight == null)
        {
            sunLight = GetComponent<Light>();
        }


        if (Application.isPlaying && autoplay)
        {
            if (variableTimeSpeed && SpeedMultiplier != null)
            {
                timeSpeed = origTimeSpeed * SpeedMultiplier.Evaluate(timeOfDay / 24f);
                timeOfDay += (Time.deltaTime / 50) * (origTimeSpeed * SpeedMultiplier.Evaluate(timeOfDay / 24f));
            }
            else
            {
                timeSpeed = origTimeSpeed;
                timeOfDay += (Time.deltaTime / 50) * origTimeSpeed;
            }
        }

        

        if (timeOfDay > 24)
        {
            timeOfDay = 0;
        }
        else if (timeOfDay < 0)
        {
            timeOfDay = 24;
        }
        if (Application.isPlaying)//because of execute in editor
        { 
            GameManager.SetTimeOfDay(timeOfDay);
        }

        float lightLevel = sunIntensityCurve.Evaluate(timeOfDay / 24f);

        if (setLightIntensity)
        {
            timeOfDayInDegrees = Utility.Map(timeOfDay, 0, 24, -90, 270);
            transform.localRotation = Quaternion.Euler(timeOfDayInDegrees, rotationOffset, 0);

            sunLight.intensity = lightLevel * sunLightIntensityMultiplier;
            moonLight.intensity = Mathf.Clamp((1 - lightLevel) * moonLightIntensityMultiplier, 0, 99);
        }

        if (menuSlider)
        {
            menuSlider.value = timeOfDay;
        }

        if (setAmbientColor)
        {
            RenderSettings.ambientLight = ambientColor.Evaluate(timeOfDay / 24);
        }

        if (setFogColor)
        {
            if (differentFogColor)
            {
                RenderSettings.fogColor = fogColor.Evaluate(timeOfDay / 24);
            }
            else
            {
                RenderSettings.fogColor = ambientColor.Evaluate(timeOfDay / 24);
            }
        }
    }

    public float GetCurrentSunIntensity(){
        float intensity = sunIntensityCurve.Evaluate(timeOfDay / 24f);
        return intensity * sunLightIntensityMultiplier;
    }

    public Light GetSunLight()
    {
        return sunLight;
    }
}
