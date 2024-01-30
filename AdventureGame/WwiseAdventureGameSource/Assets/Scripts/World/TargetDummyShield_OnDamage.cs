////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using UnityEngine;

public class TargetDummyShield_OnDamage : MonoBehaviour, IDamageable
{

    Rigidbody rb;

    void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void OnDamage(Attack a)
    {
        rb.AddForce(a.attackDir * a.knockbackStrength * 20, ForceMode.Impulse);
    }
}
