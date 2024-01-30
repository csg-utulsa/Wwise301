////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DefaultToggleValueSetterMobile : MonoBehaviour
{
    public Toggle toggleToSet;
    public bool defaultValue = true;

#if UNITY_IOS || UNITY_ANDROID
    void OnEnable()
    {
        toggleToSet.isOn = defaultValue;
    }
#endif
}
