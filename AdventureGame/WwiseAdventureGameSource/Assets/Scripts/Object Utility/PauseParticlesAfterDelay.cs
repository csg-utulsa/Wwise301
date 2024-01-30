////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseParticlesAfterDelay : MonoBehaviour {
	public float delay = 0f;

    #region private variables
    private ParticleSystem[] particles;
    #endregion

    void Start () {
        particles = GetComponentsInChildren<ParticleSystem>();
		StartCoroutine (WaitAndPause(delay));
	}

	IEnumerator WaitAndPause(float s){
		yield return new WaitForSeconds (s);
        foreach(ParticleSystem p in particles)
        {
            p.Pause();
        }
	}
}
