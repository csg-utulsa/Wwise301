////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using UnityEngine;
using UnityEngine.EventSystems;


public class MobileUseScreen : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        if (MobileEvents.OnMobileUse != null)
        {
            MobileEvents.OnMobileUse();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (MobileEvents.OnMobileUseUp != null)
        {
            MobileEvents.OnMobileUseUp();
        }
    }
}
