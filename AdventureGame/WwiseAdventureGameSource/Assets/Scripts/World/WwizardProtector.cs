////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using System.Collections;
using UnityEngine;

public class WwizardProtector : MonoBehaviour
{
    static public WwizardProtector instance;

    public static ParticleSystem PS;
    public static GameObject Target;
    public float BeamTime = 1f;

    void Start()
    {
        instance = this;
        PS = transform.Find("EnemyDestroyer").GetComponent<ParticleSystem>();
        Target = transform.Find("Target").gameObject;
    }

    static public void SetBeam(GameObject G)
    {
        Target = G;
        if (Target != null)
        {
            instance.StartCoroutine(instance.Beam());
        }
    }

    IEnumerator Beam()
    {
        PS.Play();

        for (float t = 0; t < 1 && Target != null; t += Time.deltaTime / instance.BeamTime)
        {
            PS.gameObject.transform.position = Target.transform.position;
            yield return null;
        }

        PS.Stop();
        yield return null;
    }
}
