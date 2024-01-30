////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using UnityEngine;

public class MobileUseCollider : MonoBehaviour
{
    void OnMouseDown()
    {
        if (MobileEvents.OnMobileUse != null)
        {
            MobileEvents.OnMobileUse();
        }
    }

    void OnMouseUp()
    {
        if (MobileEvents.OnMobileUseUp != null)
        {
            MobileEvents.OnMobileUseUp();
        }
    }
}
