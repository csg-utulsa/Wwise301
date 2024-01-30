////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class ImageEffectsOnLowHP : MonoBehaviour, IDamageable
{
    public bool enableChromaticAberration;

    [ShowIf("enableChromaticAberration", true)]
    public VignetteAndChromaticAberration chromAb;

    [ShowIf("enableChromaticAberration", true), Range(0, 100)]
    public float chromaticAberrationThreshold = 60f;

    public bool enableColorCorrection;

    [ShowIf("enableColorCorrection", true)]
    public ColorCorrectionCurves colorCorr;

    [ShowIf("enableColorCorrection", true), Range(0, 100)]
    public float colorCorrectionThreshold = 60f;

    private Camera mainCam;

    void OnEnable()
    {
        mainCam = Camera.main;

        if (mainCam != null)
        {
            if (enableChromaticAberration && chromAb == null)
            {
                chromAb = mainCam.gameObject.GetComponent<VignetteAndChromaticAberration>();
                if (chromAb == null)
                {
                    this.enabled = false;
                    Debug.LogWarning("ImageEffectsOnLowHP: No 'Vignette and Chromatic Aberration' component could be found! Disabling script...");
                }
            }
            if (enableColorCorrection && colorCorr == null)
            {
                colorCorr = mainCam.gameObject.GetComponent<ColorCorrectionCurves>();
                if (colorCorr == null)
                {
                    this.enabled = false;
                    Debug.LogWarning("ImageEffectsOnLowHP: No 'Color Correction Curves' component could be found! Disabling script...");
                }
            }
            if (chromAb != null)
            {
                chromAb.enabled = false;
            }
            if (colorCorr != null)
            {
                colorCorr.enabled = false;
            }
        }

    }

    public void OnDamage(Attack a)
    {
        PlayerManager.Instance.CamShake(new PlayerCamera.CameraShake(a.damage / 100f, a.damage / 100f));
    }

    void Update()
    {
        if (enableChromaticAberration && chromAb != null)
        {
            if (PlayerManager.Instance.HealthOfPlayer < chromaticAberrationThreshold)
            {
                chromAb.enabled = true;
                chromAb.chromaticAberration = Mathf.Lerp(chromAb.chromaticAberration, chromaticAberrationThreshold - PlayerManager.Instance.HealthOfPlayer, Time.deltaTime);
            }
            else
            {
                if (chromAb.chromaticAberration > 0.1)
                {
                    chromAb.chromaticAberration = Mathf.Lerp(chromAb.chromaticAberration, 0, Time.deltaTime);
                }
                else
                {
                    chromAb.enabled = false;
                }
            }
        }
        if (enableColorCorrection && colorCorr != null)
        {
            if (PlayerManager.Instance.HealthOfPlayer < colorCorrectionThreshold)
            {
                colorCorr.enabled = true;
                float test = Mathf.Clamp(Mathf.Lerp(colorCorr.saturation, (PlayerManager.Instance.HealthOfPlayer / colorCorrectionThreshold), Time.deltaTime), 0, 1);
                colorCorr.saturation = test;
            }
            else
            {
                if (colorCorr.saturation < 1f)
                {
                    colorCorr.saturation = Mathf.Lerp(colorCorr.saturation, 1, Time.deltaTime);
                }
                else
                {
                    colorCorr.saturation = 1f;
                    colorCorr.enabled = false;
                }
            }
        }
    }

}
