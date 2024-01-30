////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using UnityEngine;

public class ArcaneBlastCharge : Spell
{
    public GameObject ChargeEffect;

    private GameObject instantiatedEffect;

    public override void Activate()
    {
        instantiatedEffect = Instantiate(ChargeEffect, transform.position, Quaternion.identity) as GameObject;
    }

    public override void Deactivate()
    {
        if (instantiatedEffect != null)
        {
            foreach (ParticleSystem p in instantiatedEffect.GetComponentsInChildren<ParticleSystem>())
            {
                p.Stop();
            }
            Destroy(instantiatedEffect, 5f);
        }
    }
}

