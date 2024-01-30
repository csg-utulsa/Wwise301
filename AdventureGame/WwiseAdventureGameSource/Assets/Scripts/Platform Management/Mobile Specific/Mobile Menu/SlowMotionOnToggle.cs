////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowMotionOnToggle : MonoBehaviour
{
    public float SlowMoTimeScale = 0.3f;

    public void SlowMoToggle(bool on)
    {
        if (on)
        {
            GameManager.Instance.gameSpeedHandler.SetGameSpeed(gameObject.GetInstanceID(), SlowMoTimeScale, 0.1f, 0.3f, Mathf.Infinity);
        }
        else
        {
            GameManager.Instance.gameSpeedHandler.ReleaseGameSpeed(gameObject.GetInstanceID());
        }
    }

}
