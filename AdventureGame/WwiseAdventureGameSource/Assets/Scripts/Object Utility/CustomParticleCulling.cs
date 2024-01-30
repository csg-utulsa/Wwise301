////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomParticleCulling : MonoBehaviour
{

    [Header("Setup")]
    public List<ParticleSystem> particlesToOcclude = new List<ParticleSystem>();
    public float OcclusionRadius = 10f;
    public float checkInterval = 0.5f;

    #region private variables
    private Transform player;
    private bool isPlaying;
    #endregion

    void Start()
    {
        if (particlesToOcclude.Count == 0)
        {
            particlesToOcclude.AddRange(GetComponentsInChildren<ParticleSystem>());

            if (particlesToOcclude == null)
            {
                this.enabled = false;
            }
        }

        player = PlayerManager.Instance.playerTransform;

        StartCoroutine(CheckDistance(checkInterval));
    }

    IEnumerator CheckDistance(float interval)
    {
        while (true)
        {
            float sqrDistance = (transform.position - player.position).sqrMagnitude;
            float sqrThreshold = OcclusionRadius * OcclusionRadius;

            if (sqrDistance > sqrThreshold)
            {
                foreach (ParticleSystem p in particlesToOcclude)
                {
                    if (p.isPlaying)
                    {
                        p.Stop();
                    }
                }
                isPlaying = false;
            }
            else
            {
                foreach (ParticleSystem p in particlesToOcclude)
                {
                    if (p.isStopped)
                    {
                        p.Play();
                    }
                }
                isPlaying = true;
            }
            yield return new WaitForSecondsRealtime(interval);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = (isPlaying ? Color.yellow : Color.red) * new Color(1f, 1f, 1f, 0.5f);
        Gizmos.DrawSphere(transform.position, OcclusionRadius);
    }
}
