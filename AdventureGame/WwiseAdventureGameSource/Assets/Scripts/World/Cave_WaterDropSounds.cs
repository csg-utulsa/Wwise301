////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Cave_WaterDropSounds : MonoBehaviour
{

    public AK.Wwise.Event waterDropSound;
    public GameObject waterdropPrefab;
    private ParticleSystem particles;
    private List<ParticleCollisionEvent> colEvents = new List<ParticleCollisionEvent>();
    private List<GameObject> drops = new List<GameObject>();
    private bool stopped = false;

    void Awake()
    {
        particles = GetComponent<ParticleSystem>();
        if (particles == null)
        {
            Destroy(this);
        }
    }

    private void OnDestroy()
    {
        particles.Stop();
        foreach(var go in drops)
        {
            Destroy(go);
        }
        stopped = true;
    }

    void OnParticleCollision(GameObject other)
    {
        if (!stopped) {
            particles.GetCollisionEvents(other, colEvents);

            for (int i = 0; i < colEvents.Count; i++)
            {
                if (colEvents[i].intersection != Vector3.zero)
                {
                    //TODO: Defintely pool these sound objects!

                    GameObject drop = Instantiate(waterdropPrefab, colEvents[i].intersection, Quaternion.identity, transform) as GameObject;
                    drops.Add(drop);

                    if (drop.GetComponent<AkGameObj>() == null) {
                        drop.AddComponent<AkGameObj>();
                    }

                    waterDropSound.Post(drop);
                    Destroy(drop, 1.5f);
                }
            }
        }
    }

    private IEnumerator DelayedDestroy(GameObject go)
    {
        yield return new WaitForSecondsRealtime(0.5f);

        if(go != null)
        {
            if (drops.Contains(go))
            {
                drops.Remove(go);
            }
            Destroy(go);
        }
    }
}
