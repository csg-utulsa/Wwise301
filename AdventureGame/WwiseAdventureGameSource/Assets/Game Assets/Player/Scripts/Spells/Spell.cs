////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using UnityEngine;
using System.Collections;

public class Spell : MonoBehaviour
{
    [Header("Spell Type")]
    public float ChargeValue = 0f;

    public AnimationCurve ImpactVsChargelevel;

    [HideInInspector] //TODO: why?
    public float Damage = 10f;

    [HideInInspector]
    public float ImpactVel = 1f;

    public virtual void Activate() { }
    public virtual void Deactivate() { }
}
