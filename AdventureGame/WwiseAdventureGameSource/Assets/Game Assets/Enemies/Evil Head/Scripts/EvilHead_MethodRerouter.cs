////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script basically just makes the Evil Head stop its charge whenever it hits an object.
/// </summary>
public class EvilHead_MethodRerouter : MonoBehaviour
{
    [SerializeField]
    private EvilHeadAI parentScript;

    private void Awake()
    {
        Physics.IgnoreCollision(GetComponent<Collider>(), parentScript.GetComponent<Collider>());
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Agent"))
        {
            parentScript.StopCharge();
        }
    }
}
