////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class ImageOverlayOnHit : MonoBehaviour, IDamageable
{

    public ScreenOverlay screenOverlay;
    public float threshold = 30f;
    public float fadeInTime, fadeOutTime;

    #region private variables
    private float i;
    private IEnumerator overlayRoutine;
    #endregion 

    void Start()
    {
        Init();
    }

    public void OnDamage(Attack a)
    {
        if (overlayRoutine != null)
        {
            StopCoroutine(overlayRoutine);
        }
        overlayRoutine = OverlayFlash(a);
        StartCoroutine(overlayRoutine);
    }

    IEnumerator OverlayFlash(Attack a)
    {
        screenOverlay.enabled = true;
        a.damage = Mathf.Clamp(a.damage, 0, 100);

        //Fade in
        for (float t = i; t < a.damage / threshold; t += Time.deltaTime / fadeInTime)
        {
            i += Time.deltaTime / fadeInTime;
            screenOverlay.intensity = t;
            yield return null;
        }

        //Fade out
        for (float t = i; t > 0; t -= Time.deltaTime / fadeOutTime)
        {
            screenOverlay.intensity = t;
            yield return null;
        }
        screenOverlay.enabled = false;
    }

    void Init()
    {
        Camera mainCam = Camera.main;

        if (mainCam != null)
        {
            if (screenOverlay == null)
            { //initialise if null
                screenOverlay = mainCam.gameObject.GetComponent<ScreenOverlay>();
            }
        }

        if (screenOverlay == null)
        { //if it's still null (camera doesn't have the required component)
            enabled = false;
        }

        if (screenOverlay != null)
        {
            screenOverlay.enabled = false;
        }
    }
}
