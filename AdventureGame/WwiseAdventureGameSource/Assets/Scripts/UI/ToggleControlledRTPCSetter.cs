////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleControlledRTPCSetter : MonoBehaviour
{
    public AK.Wwise.RTPC RTPCToSet;
    public float OffValue = 0f;
    public float OnValue = 100f;

    public void SetValue(bool active)
    {
        RTPCToSet.SetGlobalValue(active ? OnValue : OffValue);
    }
}
