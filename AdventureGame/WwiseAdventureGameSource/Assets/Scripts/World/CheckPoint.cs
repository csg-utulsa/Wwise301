////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using UnityEngine;
using System.Collections;

public class CheckPoint : MonoBehaviour
{
    public Light checkPointLight;
    private ParticleSystem particles;
    private float origIntensity;
    private IEnumerator fadeRoutine;

    public AK.Wwise.Event FireplaceStartEvent;
    public AK.Wwise.Event FireplaceStopEvent;

    void Awake()
    {
        particles = GetComponentInChildren<ParticleSystem>();
        if (checkPointLight == null)
        {
            checkPointLight = GetComponentInChildren<Light>();
        }
        if (checkPointLight != null)
        {
            origIntensity = checkPointLight.intensity;
            checkPointLight.intensity = 0;
            checkPointLight.enabled = false;
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (!particles.isPlaying && col.CompareTag("Player"))
        {
            particles.Play();
            FireplaceStartEvent.Post(gameObject);

            if (checkPointLight != null)
            {
                fadeRoutine = FadeInLight();
                StartCoroutine(fadeRoutine);
            }
            else
            {
                Debug.LogFormat("CheckPoint: No light attached to {0}!", gameObject.name);
            }
            PlayerManager.Instance.SetRespawn(gameObject);
        }
    }

    IEnumerator FadeInLight()
    {
        checkPointLight.enabled = true;
        for (float t = 0; t < origIntensity; t += Time.deltaTime / 1.5f)
        {
            checkPointLight.intensity = t;
            yield return null;
        }
    }

    public void DisableCheckPoint()
    {
        if (FireplaceStopEvent.Validate()) {
            FireplaceStopEvent.Post(gameObject);
        }
        particles.Stop();
    }

}
