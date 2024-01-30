////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleAnimation : MonoBehaviour
{
    public bool useUnscaledTime = false;

    public float animationScale = 1f;
    public float frequencyScale = 1f;
    public bool randomOffset = true;

    #region private variables
    private float offset = 0f;
    private Vector3 origScale;
    private Transform trn;
    #endregion

    void Awake()
    {
        trn = transform;
        origScale = trn.localScale;

        if (randomOffset)
        {
            offset = Random.Range(0f, 1000f);
        }

        StartCoroutine(Animation());
    }

    private IEnumerator Animation()
    {
        while (true)
        {
            trn.localScale = origScale + Vector3.one * (Mathf.Sin((Time.time + offset) * frequencyScale) * animationScale * (useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime));
            yield return null;
        }
    }
}
