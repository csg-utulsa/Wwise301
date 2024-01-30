////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using UnityEngine;

public class ZoneTrigger : MonoBehaviour
{
    public AK.Wwise.State MusicState;
    public bool isSafeZone = false;

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            GameManager.Instance.EnterZone(this);
        }
        else
        {
            if (isSafeZone && col.gameObject.layer == LayerMask.NameToLayer("Agent"))
            {
                Protector(col);
            }
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            GameManager.Instance.LeaveZone(this);
        }
    }

    void Protector(Collider col)
    {
        Creature C = col.gameObject.GetComponent<Creature>();

        if (C != null)
        {
            if (!C.isFriendly && C is IDamageable)
            {
                GameManager.DamageObject(col.gameObject, new Attack(1000f));

                WwizardProtector.SetBeam(C.gameObject);
            }
        }
    }
}
