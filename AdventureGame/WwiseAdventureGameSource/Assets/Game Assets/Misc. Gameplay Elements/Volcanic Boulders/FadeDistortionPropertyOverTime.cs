////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeDistortionPropertyOverTime : MonoBehaviour {

    public float fadeTime = 0.2f;

    float origValue;
    Material particleMaterial;

    private void OnEnable()
    {
        particleMaterial = GetComponent<ParticleSystemRenderer>().material;
        origValue = particleMaterial.GetFloat("_BumpAmt");

        StartCoroutine(FadeValueOverSeconds(fadeTime));
    }

    IEnumerator FadeValueOverSeconds(float s){
        for (float t = 0; t < 1; t += Time.deltaTime/s){
            float value = Mathf.Lerp(origValue, 0f, t);
            particleMaterial.SetFloat("_BumpAmt", value);
            yield return null;
        }
        particleMaterial.SetFloat("_BumpAmt", 0f);
    }
}
