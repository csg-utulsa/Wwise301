////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using UnityEngine;

public class UnscaledTimeParticleSystem : MonoBehaviour
{
    private ParticleSystem particles;

    void OnEnable()
    {
        particles = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        if (Time.timeScale < 0.9f)
        {
            particles.Simulate(Time.unscaledDeltaTime, true, false);
        }
    }
}
