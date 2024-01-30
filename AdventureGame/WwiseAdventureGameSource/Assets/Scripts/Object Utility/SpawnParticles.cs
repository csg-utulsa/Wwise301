////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnParticles : MonoBehaviour {

    public GameObject ParticlesToSpawn;
    public bool alignRotationToParent = true;
    public bool AutomaticDestroy = true;
    [ShowIf("AutomaticDestroy", true)]
    public float DestroyDelay = 5f;

    [Space(15f)]
    public AK.Wwise.Event SpawnSound;
	
    public void SpawnParticlesNow()
    {
        GameObject vfx = Instantiate(ParticlesToSpawn, transform.position, alignRotationToParent ? transform.rotation : Quaternion.identity, transform);
        SpawnSound.Post(gameObject);

        if (AutomaticDestroy)
        {
            Destroy(vfx, DestroyDelay);
        }
    }
}
