////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using UnityEngine;

public class LockFPS : MonoBehaviour
{
    public void LockFPSToggle(bool lockFps)
    {
#if UNITY_IOS || UNITY_ANDROID
        if (lockFps)
        {
            Application.targetFrameRate = 30;
        }
        else
        {
            Application.targetFrameRate = -1;
        }
#endif
    }
}
