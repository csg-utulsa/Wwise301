////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using System.Collections;
using UnityEngine;

public class ObjectOutline : MonoBehaviour
{
    public AnimatedObjectActiveHandler animatedObjectActiveHandler;
    public float transitionTime_IN = 3f;
    public float transitionTime_OUT = 1f;

    [HideInInspector]
    public bool isEnabled = false;

    public void EnableOutline()
    {
        if (animatedObjectActiveHandler != null)
        {
            animatedObjectActiveHandler.EnableObject(transitionTime_IN);
            isEnabled = true;
        }
    }

    public void DisableOutline()
    {
        if (animatedObjectActiveHandler != null)
        {
            animatedObjectActiveHandler.DisableObject(transitionTime_OUT);
            isEnabled = false;
        }
    }

    public void DisableOutlineDelayed(float delay)
    {
        if (animatedObjectActiveHandler != null)
        {
            StartCoroutine(DisableDelayed(delay));
        }
    }

    IEnumerator DisableDelayed(float t)
    {
        yield return new WaitForSeconds(t);
        animatedObjectActiveHandler.DisableObject(transitionTime_OUT);
        isEnabled = false;
    }
}
