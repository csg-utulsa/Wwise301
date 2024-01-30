////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using UnityEngine;

public class SliderControlledRTPC : MonoBehaviour
{
    public AK.Wwise.RTPC RTPC;

    public void SetRTPC(float value)
    {
        if (Menu.isOpen)
        {
            RTPC.SetGlobalValue(value);
        }
    }

}
