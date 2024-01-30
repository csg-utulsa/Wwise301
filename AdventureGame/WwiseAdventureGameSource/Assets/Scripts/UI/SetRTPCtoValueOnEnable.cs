////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetRTPCtoValueOnEnable : MonoBehaviour
{
    public AK.Wwise.RTPC RTPC;
    public float value;

    public bool global = true;
    [ShowIf("global", false)]
    public GameObject ObjectReference;

    private void Start()
    {
        if(global){
            RTPC.SetGlobalValue(value);
        }else{
            RTPC.SetValue(ObjectReference != null ? ObjectReference : gameObject, value);
        }
    }
}
