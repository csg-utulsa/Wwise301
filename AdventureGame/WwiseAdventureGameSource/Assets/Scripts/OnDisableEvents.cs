////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDisableEvents : AkTriggerBase
{
    void OnDisable()
    {
        if (triggerDelegate != null)
        {
            triggerDelegate(this.gameObject);
        }
    }
}
