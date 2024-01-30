////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetFogColorOnEnable : MonoBehaviour
{
    public Color fogColor;

    private void OnEnable()
    {
        RenderSettings.fogColor = fogColor;
    }
}
